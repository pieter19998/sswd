﻿// <auto-generated />
using System;
using EF_Datastore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EF_Datastore.Migrations
{
    [DbContext(typeof(PracticeDbContext))]
    [Migration("20211021143649_note")]
    partial class note
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppointmentType")
                        .HasColumnType("int");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IntakeId")
                        .HasColumnType("int");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("SessionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("AppointmentId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("IntakeId")
                        .IsUnique()
                        .HasFilter("[IntakeId] IS NOT NULL");

                    b.HasIndex("PatientId");

                    b.HasIndex("SessionId")
                        .IsUnique()
                        .HasFilter("[SessionId] IS NOT NULL");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Core.Availability", b =>
                {
                    b.Property<int>("AvailabilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AvailableFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AvailableTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("AvailabilityId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("Core.Dossier", b =>
                {
                    b.Property<int>("DossierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<long>("Age")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ApplicationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiagnoseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiagnoseDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DismissalDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("HeadPractitionerId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("PatientType")
                        .HasColumnType("int");

                    b.Property<int?>("TreatmentPlanId")
                        .HasColumnType("int");

                    b.HasKey("DossierId");

                    b.HasIndex("HeadPractitionerId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.HasIndex("TreatmentPlanId")
                        .IsUnique()
                        .HasFilter("[TreatmentPlanId] IS NOT NULL");

                    b.ToTable("Dossiers");
                });

            modelBuilder.Entity("Core.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("BigNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("StudentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Core.Intake", b =>
                {
                    b.Property<int>("IntakeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("IntakeById")
                        .HasColumnType("int");

                    b.Property<string>("IntakeSuperVisor")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntakeId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IntakeById");

                    b.ToTable("Intakes");
                });

            modelBuilder.Entity("Core.Notes", b =>
                {
                    b.Property<int>("NoticeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DossierId")
                        .HasColumnType("int");

                    b.Property<int>("NoteType")
                        .HasColumnType("int");

                    b.Property<int?>("SessionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("NoticeId");

                    b.HasIndex("DossierId");

                    b.HasIndex("SessionId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Core.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("DayOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IdentityPatientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PatientNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("StudentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("PatientId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Core.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("DossierId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.Property<DateTime>("SessionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SessionEmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SessionId");

                    b.HasIndex("DossierId");

                    b.HasIndex("PatientId");

                    b.HasIndex("SessionEmployeeId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Core.TreatmentPlan", b =>
                {
                    b.Property<int>("TreatmentPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<double>("SessionDuration")
                        .HasColumnType("float");

                    b.Property<int>("SessionsPerWeek")
                        .HasColumnType("int");

                    b.HasKey("TreatmentPlanId");

                    b.ToTable("TreatmentPlans");
                });

            modelBuilder.Entity("Core.Appointment", b =>
                {
                    b.HasOne("Core.Employee", "EfEmployee")
                        .WithMany("Appointment")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Intake", "Intake")
                        .WithOne("Appointment")
                        .HasForeignKey("Core.Appointment", "IntakeId");

                    b.HasOne("Core.Patient", "Patient")
                        .WithMany("Appointment")
                        .HasForeignKey("PatientId");

                    b.HasOne("Core.Session", "Session")
                        .WithOne("Appointment")
                        .HasForeignKey("Core.Appointment", "SessionId");

                    b.Navigation("EfEmployee");

                    b.Navigation("Intake");

                    b.Navigation("Patient");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Core.Availability", b =>
                {
                    b.HasOne("Core.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Core.Dossier", b =>
                {
                    b.HasOne("Core.Employee", "HeadPractitioner")
                        .WithMany("HeadPractitioner")
                        .HasForeignKey("HeadPractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Patient", "Patient")
                        .WithOne("Dossier")
                        .HasForeignKey("Core.Dossier", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.TreatmentPlan", "TreatmentPlan")
                        .WithOne("Dossier")
                        .HasForeignKey("Core.Dossier", "TreatmentPlanId");

                    b.Navigation("HeadPractitioner");

                    b.Navigation("Patient");

                    b.Navigation("TreatmentPlan");
                });

            modelBuilder.Entity("Core.Intake", b =>
                {
                    b.HasOne("Core.Employee", "IntakeBy")
                        .WithMany("IntakeBy")
                        .HasForeignKey("IntakeById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IntakeBy");
                });

            modelBuilder.Entity("Core.Notes", b =>
                {
                    b.HasOne("Core.Dossier", "Dossier")
                        .WithMany("Notices")
                        .HasForeignKey("DossierId");

                    b.HasOne("Core.Session", "Session")
                        .WithMany("Notices")
                        .HasForeignKey("SessionId");

                    b.Navigation("Dossier");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Core.Session", b =>
                {
                    b.HasOne("Core.Dossier", "Dossier")
                        .WithMany("Sessions")
                        .HasForeignKey("DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Patient", "Patient")
                        .WithMany("Sessions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Employee", "SessionEmployee")
                        .WithMany("Sessions")
                        .HasForeignKey("SessionEmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Dossier");

                    b.Navigation("Patient");

                    b.Navigation("SessionEmployee");
                });

            modelBuilder.Entity("Core.Dossier", b =>
                {
                    b.Navigation("Notices");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Core.Employee", b =>
                {
                    b.Navigation("Appointment");

                    b.Navigation("HeadPractitioner");

                    b.Navigation("IntakeBy");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Core.Intake", b =>
                {
                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("Core.Patient", b =>
                {
                    b.Navigation("Appointment");

                    b.Navigation("Dossier");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Core.Session", b =>
                {
                    b.Navigation("Appointment");

                    b.Navigation("Notices");
                });

            modelBuilder.Entity("Core.TreatmentPlan", b =>
                {
                    b.Navigation("Dossier");
                });
#pragma warning restore 612, 618
        }
    }
}
