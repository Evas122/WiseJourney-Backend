﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WiseJourneyBackend.Infrastructure.Data;

#nullable disable

namespace WiseJourneyBackend.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241202154413_Entities_refactor")]
    partial class Entities_refactor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Auth.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RevokedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Auth.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.Geometry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("PlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId")
                        .IsUnique();

                    b.ToTable("Geometries");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.OpeningHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("OpenNow")
                        .HasColumnType("bit");

                    b.Property<string>("PlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId")
                        .IsUnique();

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.Place", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriceLevel")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("ShortAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRatingsTotal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.PlaceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("PlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.ToTable("PlacesType");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.WeeklyHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CloseTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<DateTime>("OpenTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OpeningHourId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OpeningHourId");

                    b.ToTable("WeeklyHours");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.TripDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.ToTable("TripDays");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.TripPlace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("PlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ScheduleTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TripDayId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.HasIndex("TripDayId");

                    b.ToTable("TripPlaces");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Auth.RefreshToken", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Auth.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.Geometry", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Places.Place", null)
                        .WithOne("Geometry")
                        .HasForeignKey("WiseJourneyBackend.Domain.Entities.Places.Geometry", "PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.OpeningHour", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Places.Place", null)
                        .WithOne("OpeningHour")
                        .HasForeignKey("WiseJourneyBackend.Domain.Entities.Places.OpeningHour", "PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.PlaceType", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Places.Place", null)
                        .WithMany("PlaceTypes")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.WeeklyHour", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Places.OpeningHour", "OpeningHour")
                        .WithMany("WeeklyHours")
                        .HasForeignKey("OpeningHourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OpeningHour");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.Trip", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Auth.User", "User")
                        .WithMany("Trips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.TripDay", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Trips.Trip", "Trip")
                        .WithMany("TripDays")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.TripPlace", b =>
                {
                    b.HasOne("WiseJourneyBackend.Domain.Entities.Places.Place", "Place")
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WiseJourneyBackend.Domain.Entities.Trips.TripDay", "TripDay")
                        .WithMany("TripPlaces")
                        .HasForeignKey("TripDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("TripDay");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Auth.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.OpeningHour", b =>
                {
                    b.Navigation("WeeklyHours");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Places.Place", b =>
                {
                    b.Navigation("Geometry")
                        .IsRequired();

                    b.Navigation("OpeningHour")
                        .IsRequired();

                    b.Navigation("PlaceTypes");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.Trip", b =>
                {
                    b.Navigation("TripDays");
                });

            modelBuilder.Entity("WiseJourneyBackend.Domain.Entities.Trips.TripDay", b =>
                {
                    b.Navigation("TripPlaces");
                });
#pragma warning restore 612, 618
        }
    }
}
