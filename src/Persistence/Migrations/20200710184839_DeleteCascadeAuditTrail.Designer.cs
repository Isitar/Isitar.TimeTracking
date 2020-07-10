﻿// <auto-generated />
using System;
using Isitar.TimeTracking.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Isitar.TimeTracking.Persistence.Migrations
{
    [DbContext(typeof(TimeTrackingDbContext))]
    [Migration("20200710184839_DeleteCascadeAuditTrail")]
    partial class DeleteCascadeAuditTrail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.AuditTrailEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Instant>("When")
                        .HasColumnType("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("AuditTrailEntry");
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Instant?>("CreatedAt")
                        .HasColumnType("timestamp");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Instant?>("UpdatedAt")
                        .HasColumnType("timestamp");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.TimeTrackingEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Instant>("From")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Instant?>("To")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("TimeTrackingEntries");
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Acronym")
                        .HasColumnType("text");

                    b.Property<Instant?>("CreatedAt")
                        .HasColumnType("timestamp");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Locale")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Instant?>("UpdatedAt")
                        .HasColumnType("timestamp");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.AuditTrailEntry", b =>
                {
                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.Project", null)
                        .WithMany("AuditTrailEntries")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", null)
                        .WithMany("AuditTrailEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.Project", b =>
                {
                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.TimeTrackingEntry", b =>
                {
                    b.HasOne("Isitar.TimeTracking.Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Isitar.TimeTracking.Domain.Entities.User", b =>
                {
                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Isitar.TimeTracking.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });
#pragma warning restore 612, 618
        }
    }
}
