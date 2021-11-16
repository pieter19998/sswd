using System;
using Core;
using Dashboard.Services;
using DomainServices;
using EF_Datastore;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using IF_Datastore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 100;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.AddDbContext<PracticeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("EFDEFAULT")));

            services.AddDbContextPool<AppIdentityDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("IFDEFAULT")));


            services.AddScoped<IGraphQLClient>(s =>
                new GraphQLHttpClient(Configuration.GetConnectionString("GRAPHQLPHYSIO"), new NewtonsoftJsonSerializer()));
            services.AddScoped<UserServiceApi>();
            services.AddScoped<AppointmentApiService>();
            services.AddScoped<EmployeeApiService>();
            services.AddScoped<IntakeApiService>();
            services.AddScoped<PatientApiService>();
            services.AddScoped<AvailabilityApiService>();
            services.AddScoped<SessionApiService>();
            services.AddScoped<DiagnoseApiService>();
            services.AddScoped<OperationApiService>();
            services.AddScoped<NoteApiService>();

            services.AddHttpClient<AppointmentApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<UserServiceApi>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<EmployeeApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<IntakeApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<SessionApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<PatientApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<TreatmentApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<AvailabilityApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<DossierApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<OperationApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });
            services.AddHttpClient<NoteApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("API"));
            });

            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IDossierRepository, DossierRepository>();
            services.AddTransient<IIntakeRepository, IntakeRepository>();
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<ISessionRepository, SessionRepository>();
            services.AddTransient<ITreatmentPlanRepository, TreatmentPlanRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IOperationRepository, ApiOperationRepository>();
            services.AddTransient<IAvailabilityRepository, AvailabilityRepository>();

            services.AddControllersWithViews();

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCookiePolicy();
        }
    }
}