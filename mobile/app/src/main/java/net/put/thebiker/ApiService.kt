package net.put.thebiker

import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST
import rx.Observable

interface ApiService {

    @POST("api/vibrations")
    fun sendVibrations (@Body body: VibrationServerMessage): Observable<ResponseBody>
}