using System.Reflection;
using System.Text;
using AutoZone.Models;
using AutoZone.Services;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AutoZone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------ DbContext ------------------
            builder.Services.AddDbContext<AutoZoneDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("AutoZoneDbContext")));

            // ------------------ Services ------------------
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IRentalService, RentalService>();

            // ------------------ AutoMapper ------------------
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ------------------ JWT Authentication ------------------
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT key not set. Set JWT_KEY env var or Jwt:Key in configuration.");
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

            // ------------------ CORS ------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
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
                        policy.AllowAnyHeader()
                              .AllowAnyMethod()
                              .SetIsOriginAllowedToAllowWildcardSubdomains()
                              .AllowCredentials();
                    }
                });
            });

            // ------------------ Controllers ------------------
            builder.Services.AddControllers();

            // ------------------ Swagger with XML Documentation and JWT ------------------
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {

                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AutoZone API",
                    Version = "v1",
                    Description = "API for managing car sales and rentals",
                    Contact = new OpenApiContact
                    {
                        Name = "Assem Osama",
                        Email = "assemosama00@gmail.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
                });

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

            // ------------------ Build App ------------------
            var app = builder.Build();

            // ------------------ Swagger UI ------------------
            
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoZone API V1");
                });
            

            // ------------------ Middleware ------------------
            app.UseMiddleware<AutoZone.Middleware.ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowReact");

            app.UseAuthentication();
            app.UseAuthorization();

            // ------------------ Map Controllers ------------------
            try
            {
                app.MapControllers();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Console.WriteLine(loaderException.Message);
                }
                throw;
            }

            // ------------------ Seed Data ------------------
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AutoZoneDbContext>();
                var env = app.Environment;

                if (env.IsDevelopment())
                {
                    DbSeeder.SeedData(db);
                }
            }

            // ------------------ Run App ------------------
            app.Run();
        }
    }
}
