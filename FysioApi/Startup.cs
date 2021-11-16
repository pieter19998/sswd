using System;
using Core;
using DomainServices;
using EF_Datastore;
using FysioApi.Services;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using HotChocolate.Types;
using IF_Datastore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FysioApi
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
            services.AddHttpClient<StamApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetConnectionString("STAM"));
            });


            services.AddDbContext<PracticeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("EFDEFAULT")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IFDEFAULT")));

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            services.AddScoped<IDossierRepository, DossierRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IIntakeRepository, IntakeRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ITreatmentPlanRepository, TreatmentPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDossierRepository, DossierRepository>();
            services.AddScoped<IOperationRepository, ApiOperationRepository>();
            services.AddScoped<IDiagnoseRepository, ApiDiagnoseRepository>();
            services.AddScoped<IStamApiService, StamApiService>();

            services.AddScoped<IGraphQLClient>(s =>
                new GraphQLHttpClient(Configuration.GetConnectionString("GraphQLURI"), new NewtonsoftJsonSerializer()));
            services.AddScoped<Query>();

            services.AddGraphQLServer().BindRuntimeType<uint, UnsignedIntType>().AddQueryType<Query>().AddProjections()
                .AddFiltering();
            services.AddScoped<ApiDiagnoseRepository>();
            services.AddIdentity<User, IdentityRole>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "FysioApi", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FysioApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Physio Api"); });
                endpoints.MapGraphQL();
            });
        }
    }
}