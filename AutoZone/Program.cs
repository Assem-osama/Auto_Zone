using System.Reflection.Emit;
using System.Text;
using AutoZone.Models;
using AutoZone.Services;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using static AutoZone.Models.Enum;

namespace AutoZone
{
    public class Program
    {
        public static void Main(string[] args)
        {       
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AutoZoneDbContext>(
              options => options.UseSqlServer(builder.Configuration.GetConnectionString("AutoZoneDbContext")));

            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IRentalService, RentalService>();




            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey)) throw new Exception("JWT key not set. Set JWT_KEY env var or Jwt:Key in configuration.");
            var key = Encoding.UTF8.GetBytes(jwtKey);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policy =>
                {
                    var origins = (Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? builder.Configuration["AllowedOrigins"]) ?? "";
                    if (!string.IsNullOrWhiteSpace(origins))
                    {
                        var originList = origins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        policy.WithOrigins(originList)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    }
                    else
                    {
                        // Fallback to conservative behaviour (you may change)
                        policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials();
                    }
                });
            });





            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<AutoZone.Middleware.ErrorHandlerMiddleware>();


            app.UseHttpsRedirection();

            app.UseCors("DefaultCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //Seed Data in Database in run time
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AutoZoneDbContext>();
                // نجيب البيئة الحالية (Development ولا Production)
                var env = app.Environment;

                if (env.IsDevelopment())
                {
                    // ✅ شغّل الـ Seeder بس في الـ Development وبعد كدا لما تشغل البرنامج فعليا في لبرودكشن ابقا ضيف الأدمن الحقيقين في has data()
                    DbSeeder.SeedData(db);
                }
            }



            app.Run();
        }
    }
}
