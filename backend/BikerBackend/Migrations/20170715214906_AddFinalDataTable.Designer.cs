using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BikerBackend.DAL;

namespace BikerBackend.Migrations
{
    [DbContext(typeof(BikerDbContext))]
    [Migration("20170715214906_AddFinalDataTable")]
    partial class AddFinalDataTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikerBackend.Entities.FinalData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<double>("SurfaceDistortionRatio");

                    b.HasKey("Id");

                    b.ToTable("FinalDatas");
                });

            modelBuilder.Entity("BikerBackend.Entities.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BeginTime");

                    b.Property<double>("EndLocationLatitude");

                    b.Property<double>("EndLocationLongitude");

                    b.Property<DateTime>("EndTime");

                    b.Property<double>("StartLocationLatitude");

                    b.Property<double>("StartLocationLongitude");

                    b.Property<int>("UserId");

                    b.HasKey("RouteId");

                    b.HasIndex("UserId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("BikerBackend.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Surname");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BikerBackend.Entities.VibrationData", b =>
                {
                    b.Property<int>("VibrationDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("LocationLatitude");

                    b.Property<double>("LocationLongitude");

                    b.Property<int>("RouteId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("VibrationDataId");

                    b.HasIndex("RouteId");

                    b.ToTable("VibrationDatas");
                });

            modelBuilder.Entity("BikerBackend.Entities.Route", b =>
                {
                    b.HasOne("BikerBackend.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BikerBackend.Entities.VibrationData", b =>
                {
                    b.HasOne("BikerBackend.Entities.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
