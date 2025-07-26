using HospitalAPI.Hospital.API.Middleware;
using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.Services.Accountant;
using HospitalAPI.Hospital.Application.Services.Appointment;
//using HospitalAPI.Hospital.Application.Services.Department;
//using HospitalAPI.Hospital.Application.Services.Doctor;
using HospitalAPI.Hospital.Application.Services.Inpatient_Admission;
using HospitalAPI.Hospital.Application.Services.Laboratory;
//using HospitalAPI.Hospital.Application.Services.Patient;
using HospitalAPI.Hospital.Application.Services.Room;
using HospitalAPI.Hospital.Application.Services.Staff;
using HospitalAPI.Hospital.Domain;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure;
using HospitalAPI.Hospital.Infrastructure.Data;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Accountant;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Appointment;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Billing;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Department;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Doctors;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Inpatient_Admission;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Laboratory;
//using HospitalAPI.Hospital.Infrastructure.Repositories.Patient;
using HospitalAPI.Hospital.Infrastructure.Repositories.Room;
using HospitalAPI.Hospital.Infrastructure.Repositories.Staff;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddIdentity<UserApplication, RoleApplication>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<HospitalContex>()
            .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = false;
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidIssuer = builder.Configuration["JWT:IssuerIP"],

                  ValidateAudience = true,
                  ValidAudience = builder.Configuration["JWT:AudienceIP"],

                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecritKey"])),
                  RoleClaimType = ClaimTypes.Role,
                  ValidateLifetime = true,
                  ClockSkew = TimeSpan.Zero
              };
          });
            // DbContext
            builder.Services.AddDbContext<HospitalContex>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Hospital"))
            );

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policyBuilder =>
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());
            });

            // Controllers
            builder.Services.AddControllers();

            // Repositories & Services
            //builder.Services.AddIdentity<UserApplication, IdentityRole>()
            //  .AddEntityFrameworkStores<HospitalContex>();
            builder.Services.AddScoped<IAccountantRepository, AccountantRepository>();
            builder.Services.AddScoped<IPatientsService, PatientsService>();
            builder.Services.AddScoped<IDoctorsService, DoctorsService>();
            builder.Services.AddScoped<IInpatientAdmissionService, InpatientAdmissionService>();
            builder.Services.AddScoped<IBillingService, BillingService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<IAccountantService, AccountantService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
            builder.Services.AddScoped<IInpatientAdmissionService, InpatientAdmissionService>();
            builder.Services.AddScoped<IBillingRepository, BillingRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IInpatient_AdmissionRepository, Inpatient_AdmissionRepository>();
            builder.Services.AddScoped<ILaboratoryRepository, LaboratoryRepository>();
            builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();

            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IStaffRepository, StaffRepository>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            #region Swagger Setting
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hospital System",
                    Description = " Hospital"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });
            #endregion


            var app = builder.Build();

            // Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleApplication>>();
                await IdentitySeeder.SeedRolesAsync(roleManager);
            }

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.Use(async (context, next) =>
            //{
            //    await next();

            //    if (context.Response.StatusCode == 400 && !context.Response.HasStarted)
            //    {
            //        context.Response.ContentType = "application/json";
            //        await context.Response.WriteAsync("{\"error\": \"Bad Request: Please check your input data or request format.\"}");
            //    }
            //    else if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
            //    {
            //        context.Response.ContentType = "application/json";
            //        await context.Response.WriteAsync("{\"error\": \"Unauthorized: Please login or provide a valid token.\"}");
            //    }
            //    else if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
            //    {
            //        context.Response.ContentType = "application/json";
            //        await context.Response.WriteAsync("{\"error\": \"Forbidden: You do not have permission to access this resource.\"}");
            //    }
            //});
            app.UseMiddleware<TokenRevocationMiddleware>();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.MapControllers();
            //app.Use(async (context, next) =>
            //{
            //    await next();

            //    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            //    {
            //        context.Response.ContentType = "application/json";
            //        await context.Response.WriteAsync("{\"error\": \"Not Found: The requested resource could not be found.\"}");
            //    }
            //});
            app.Run();
        }
    }
}
