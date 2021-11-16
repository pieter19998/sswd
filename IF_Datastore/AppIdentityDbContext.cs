using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IF_Datastore
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var physioTherapist = new User
            {
                Id = "b74ccc14-6340-4840-95c2-db12554843e5",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                PhoneNumber = "1234567890",
                UserId = 0
            };

            var patient = new User
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e6",
                Email = "user@gmail.com",
                UserName = "user@gmail.com",
                PhoneNumber = "1234567810",
                UserId = 1
            };

            var passwordHasher = new PasswordHasher<User>();
            patient.PasswordHash = passwordHasher.HashPassword(physioTherapist, "Admin");
            physioTherapist.PasswordHash = passwordHasher.HashPassword(patient, "Patient");
            builder.Entity<User>().HasData(physioTherapist);
            builder.Entity<User>().HasData(patient);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = Role.PHYSIO_THERAPIST.ToString(),
                    NormalizedName = Role.PHYSIO_THERAPIST.ToString()
                },
                new IdentityRole
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = Role.PATIENT.ToString(),
                    NormalizedName = Role.PATIENT.ToString()
                },
                new IdentityRole
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7331", Name = Role.STUDENT_EMPLOYEE.ToString(),
                    NormalizedName = Role.STUDENT_EMPLOYEE.ToString()
                }
            );
        }

        //
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                    {RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ccc14-6340-4840-95c2-db12554843e5"},
                new IdentityUserRole<string>
                    {RoleId = "c7b013f0-5201-4317-abd8-c211f91b7330", UserId = "b74ddd14-6340-4840-95c2-db12554843e6"}
                // new IdentityUserRole<string>()
                //     {RoleId = "c7b013f0-5201-4317-abd8-c211f91b7331"}
            );
        }
    }
}