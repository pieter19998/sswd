using Core;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class PracticeDbContext : DbContext
    {
        public PracticeDbContext(DbContextOptions<PracticeDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Intake> Intakes { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Appointment
            modelBuilder.Entity<Appointment>().HasKey(a => new {a.AppointmentId});
            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.EfEmployee)
                .WithMany(p => p.Appointment)
                .HasForeignKey(pt => pt.EmployeeId);
            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.Patient)
                .WithMany(t => t.Appointment)
                .HasForeignKey(pt => pt.PatientId);
            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.Intake)
                .WithOne(t => t.Appointment);
            modelBuilder.Entity<Appointment>()
                .HasOne(pt => pt.Session)
                .WithOne(t => t.Appointment);
            // Patient
            modelBuilder.Entity<Patient>().HasKey(a => new {a.PatientId});
            modelBuilder.Entity<Patient>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Patient>().HasOne(d => d.Dossier).WithOne(d => d.Patient);
            modelBuilder.Entity<Patient>().HasMany(a => a.Sessions).WithOne(a => a.Patient)
                .HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.NoAction);
            // Employee
            modelBuilder.Entity<Employee>().HasKey(a => new {a.EmployeeId});
            modelBuilder.Entity<Employee>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Employee>().HasMany(a => a.HeadPractitioner).WithOne(a => a.HeadPractitioner)
                .HasForeignKey(k => k.HeadPractitionerId);
            modelBuilder.Entity<Employee>().HasMany(a => a.Intake).WithOne(a => a.IntakeBy)
                .HasForeignKey(k => k.IntakeId);
            modelBuilder.Entity<Employee>().HasMany(a => a.Sessions).WithOne(a => a.SessionEmployee)
                .HasForeignKey(k => k.SessionEmployeeId).OnDelete(DeleteBehavior.NoAction);
            // Dossier coupling
            modelBuilder.Entity<Dossier>().HasKey(d => new {d.DossierId});
            modelBuilder.Entity<Dossier>().HasOne(p => p.Patient).WithOne(d => d.Dossier);
            modelBuilder.Entity<Dossier>().HasOne(t => t.TreatmentPlan).WithOne(d => d.Dossier);
            modelBuilder.Entity<Dossier>().HasMany(n => n.Notices).WithOne(d => d.Dossier);
            modelBuilder.Entity<Dossier>().HasMany(s => s.Sessions).WithOne(d => d.Dossier);
            // Notice coupling
            modelBuilder.Entity<Notes>().HasKey(n => new {n.NoticeId});
            modelBuilder.Entity<Notes>().HasOne(d => d.Dossier).WithMany(d => d.Notices);
            modelBuilder.Entity<Notes>().HasOne(s => s.Session).WithMany(n => n.Notices);
            // Session coupling
            modelBuilder.Entity<Session>().HasKey(s => new {s.SessionId});
            modelBuilder.Entity<Session>().HasOne(n => n.Dossier).WithMany(d => d.Sessions);
            modelBuilder.Entity<Session>().HasOne(n => n.Dossier).WithMany(d => d.Sessions);
            modelBuilder.Entity<Session>().HasOne(n => n.SessionEmployee).WithMany(d => d.Sessions)
                .HasForeignKey(x => x.SessionEmployeeId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Session>().HasOne(n => n.Patient).WithMany(d => d.Sessions)
                .HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.NoAction);
            ;
            // Treatment coupling
            modelBuilder.Entity<TreatmentPlan>().HasKey(t => new {t.TreatmentPlanId});
            modelBuilder.Entity<TreatmentPlan>().HasOne(d => d.Dossier).WithOne(t => t.TreatmentPlan);
            // Intake coupling
            modelBuilder.Entity<Intake>().HasKey(t => new {t.IntakeId});
            modelBuilder.Entity<Intake>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Intake>().HasOne(d => d.Appointment).WithOne(t => t.Intake);
            modelBuilder.Entity<Intake>()
                .HasOne(pt => pt.IntakeBy)
                .WithMany(p => p.Intake)
                .HasForeignKey(pt => pt.IntakeById);
        }
    }
}