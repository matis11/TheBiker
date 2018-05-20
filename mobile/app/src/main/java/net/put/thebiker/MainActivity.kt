package net.put.thebiker

import android.Manifest
import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.content.pm.PackageManager
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.location.Location
import android.os.Bundle
import android.os.Looper
import android.support.annotation.StringRes
import android.support.design.widget.Snackbar
import android.support.v4.app.ActivityCompat
import android.support.v7.app.AppCompatActivity
import android.util.Log
import android.view.View
import com.google.android.gms.location.*
import kotlinx.android.synthetic.main.activity_main.*
import retrofit2.Retrofit
import retrofit2.adapter.rxjava.RxJavaCallAdapterFactory
import retrofit2.converter.gson.GsonConverterFactory
import rx.android.schedulers.AndroidSchedulers
import rx.schedulers.Schedulers
import rx.subjects.PublishSubject
import rx.subscriptions.CompositeSubscription
import java.text.SimpleDateFormat
import java.util.*


data class VibrationServerMessage(val x: Double,
                                  val y: Double,
                                  val z: Double,
                                  val timeStamp: String,
                                  val locationLatitude: Double = 0.0,
                                  val locationLongitude: Double = 0.0,
                                  val route: String? = null,
                                  val routeId: Int = 1)

val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS", Locale.getDefault())

class MainActivity : AppCompatActivity() {

    val BASE_URL = "http://biker.azurewebsites.net/"
    private val UPDATE_INTERVAL_IN_MILLISECONDS: Long = 1000
    private val FASTEST_UPDATE_INTERVAL_IN_MILLISECONDS = UPDATE_INTERVAL_IN_MILLISECONDS / 2
    val REQUEST_PERMISSIONS_REQUEST_CODE = 34


    lateinit var sensorManager: SensorManager
    lateinit var sensor: Sensor
    lateinit var rotationSensor: Sensor
    var accelerations = DoubleArray(5)


//    val presenter = MainPresenter()

    val subscription = CompositeSubscription()

    val sensorObservable: PublishSubject<DoubleArray> = PublishSubject.create()

    lateinit var locationClient: FusedLocationProviderClient
    lateinit var lastLocation: Location
    lateinit var mSettingsClient: SettingsClient
    lateinit var mLocationRequest: LocationRequest
    lateinit var mLocationSettingsRequest: LocationSettingsRequest
    lateinit var locationCallback: LocationCallback


    @SuppressLint("MissingPermission")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val retrofit = Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .addCallAdapterFactory(RxJavaCallAdapterFactory.create())
                .build()

        val apiService = retrofit.create(ApiService::class.java)

        sensorManager = getSystemService(Context.SENSOR_SERVICE) as (SensorManager)

        sensor = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER)
        rotationSensor = sensorManager.getDefaultSensor(Sensor.TYPE_GYROSCOPE)

        sensorManager.registerListener(AccelerationListener(), sensor, 50000000, 50000000)
        sensorManager.registerListener(RotationListener(), rotationSensor, 50000000, 50000000)

        locationClient = LocationServices.getFusedLocationProviderClient(this)
        mSettingsClient = LocationServices.getSettingsClient(this)

        createLocationCallback()
        createLocationRequest()
        buildLocationSettingsRequest()


        subscription.add(sensorObservable
                .subscribe {
                    apiService.sendVibrations(
                            VibrationServerMessage(
                                    it[0],
                                    it[1],
                                    it[2],
                                    dateFormat.format(System.currentTimeMillis()),
                                    it[3],
                                    it[4]
                            ))
                            .subscribeOn(Schedulers.io())
                            .observeOn(AndroidSchedulers.mainThread())
                            .subscribe({ }, {
                                Log.d("Network Error", it.cause.toString())
                            })
                }

        )
    }

    override fun onStart() {
        super.onStart()
        if (checkPermissions(this@MainActivity, REQUEST_PERMISSIONS_REQUEST_CODE, main_layout, R.string.permission_location, arrayOf(
                Manifest.permission.ACCESS_FINE_LOCATION))) {
            startLocationUpdates()

        }

    }

    private fun createLocationRequest() {
        mLocationRequest = LocationRequest()

        // Sets the desired interval for active location updates. This interval is
        // inexact. You may not receive updates at all if no location sources are available, or
        // you may receive them slower than requested. You may also receive updates faster than
        // requested if other applications are requesting location at a faster interval.
        mLocationRequest.interval = UPDATE_INTERVAL_IN_MILLISECONDS

        // Sets the fastest rate for active location updates. This interval is exact, and your
        // application will never receive updates faster than this value.
        mLocationRequest.fastestInterval = FASTEST_UPDATE_INTERVAL_IN_MILLISECONDS

        mLocationRequest.priority = LocationRequest.PRIORITY_HIGH_ACCURACY
    }

    private fun createLocationCallback() {
        locationCallback = object : LocationCallback() {
            override fun onLocationResult(locationResult: LocationResult) {
                super.onLocationResult(locationResult)

                lastLocation = locationResult.getLastLocation()
                accelerations[3] = lastLocation.latitude
                accelerations[4] = lastLocation.longitude
                Log.d("locationTask", lastLocation.latitude.toString())

            }
        }
    }

    private fun buildLocationSettingsRequest() {
        val builder = LocationSettingsRequest.Builder()
        builder.addLocationRequest(mLocationRequest)
        mLocationSettingsRequest = builder.build()
    }

    private fun startLocationUpdates() {
        // Begin by checking if the device has the necessary location settings.
        mSettingsClient.checkLocationSettings(mLocationSettingsRequest)
                .addOnSuccessListener(this) {
                    locationClient.requestLocationUpdates(mLocationRequest,
                            locationCallback, Looper.myLooper())
                }
    }

    @SuppressLint("MissingPermission")
    fun getLastLocation() {
        locationClient.lastLocation
                .addOnCompleteListener(this) { task ->
                    if (task.isSuccessful && task.result != null) {
                        lastLocation = task.result

                        Log.d("locationTask", lastLocation.latitude.toString())

                        accelerations[3] = lastLocation.latitude
                        accelerations[4] = lastLocation.longitude

                    } else {
                        Log.d("locationTask", "failed")
                    }
                }
    }

    inner class AccelerationListener : SensorEventListener {

        val gravityV = FloatArray(3)
        val alpha: Float = 0.8f
        var currentTime = 0L

        override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {}

        override fun onSensorChanged(event: SensorEvent) {

            gravityV[0] = alpha * gravityV[0] + (1 - alpha) * event.values[0]
            gravityV[1] = alpha * gravityV[1] + (1 - alpha) * event.values[1]
            gravityV[2] = alpha * gravityV[2] + (1 - alpha) * event.values[2]

            accelerations[0] = event.values[0] - gravityV[0].toDouble()
            accelerations[1] = event.values[1] - gravityV[1].toDouble()
            accelerations[2] = event.values[2] - gravityV[2].toDouble()


            sensorObservable.onNext(accelerations)

        }
    }

    inner class RotationListener : SensorEventListener {
        override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {}

        override fun onSensorChanged(event: SensorEvent) {
            var rotationMatrix = FloatArray(16)
            SensorManager.getRotationMatrixFromVector(
                    rotationMatrix, event.values)

            val remappedRotationMatrix = FloatArray(16)
            SensorManager.remapCoordinateSystem(rotationMatrix,
                    SensorManager.AXIS_X,
                    SensorManager.AXIS_Z,
                    remappedRotationMatrix)

// Convert to orientations
            val orientations = FloatArray(3)
            SensorManager.getOrientation(remappedRotationMatrix, orientations)

            for (i in 0..2) {
                orientations[i] = Math.toDegrees(orientations[i].toDouble()).toFloat()
            }

            accXTextView.text = orientations[0].toString()
            accYTextView.text = orientations[1].toString()
            accZTextView.text = orientations[2].toString()

        }
    }

    fun checkPermissions(activity: Activity, requestCode: Int,
                         snackBarParent: View, @StringRes explanationTextId: Int,
                         permissions: Array<String>): Boolean {
        val hasRequiredPermissions = permissions.none { ActivityCompat.checkSelfPermission(activity, it) != PackageManager.PERMISSION_GRANTED }

        if (hasRequiredPermissions) {
            return true
        } else {
            requestPermissions(activity, requestCode, snackBarParent,
                    permissions, explanationTextId)
            return false
        }
    }

    private fun requestPermissions(activity: Activity, requestCode: Int,
                                   snackBarParent: View, permissions: Array<String>,
                                   @StringRes explanationTextId: Int) {
        val shouldShowRequestPermissionRationale = permissions.any { ActivityCompat.shouldShowRequestPermissionRationale(activity, it) }

        if (shouldShowRequestPermissionRationale) {
            Snackbar.make(snackBarParent, explanationTextId, Snackbar.LENGTH_INDEFINITE)
                    .setAction(android.R.string.ok) { requestPermissions(activity, requestCode, permissions) }
                    .show()
        } else {
            requestPermissions(activity, requestCode, permissions)
        }
    }

    private fun requestPermissions(activity: Activity, requestCode: Int, permissions: Array<String>) {
        ActivityCompat.requestPermissions(activity, permissions, requestCode)
    }

    override fun onDestroy() {

        subscription.clear()
        super.onDestroy()
    }
}
