using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotelReview.Authorization.Age;
using HotelReview.Authorization.MultipleHotels;
using HotelReview.Authorization.ResourceOperation;
using HotelReview.Entities;
using HotelReview.Middleware;
using HotelReview.Models;
using HotelReview.Models.Hotels;
using HotelReview.Models.ParkingPlace;
using HotelReview.Models.User;
using HotelReview.Models.Validators;
using HotelReview.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelReview
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("DateOfBirth", x => x.RequireClaim("DateOfBirth"));
                option.AddPolicy("AtLeast18", x => x.AddRequirements(new MinimumAgeRequirement(18)));


                option.AddPolicy("CreatedAtLeast2Hotels",
                    x => x.AddRequirements(new CreatedMultipleHotelsRequirement(2)));

                option.AddPolicy("EmailContact", x => x.RequireClaim("EmailContact"));
            });
            services.AddScoped<IAuthorizationHandler, CreatedMultipleHotelsRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<HotelDbContext>();
            services.AddScoped<HotelSeeder>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<SampleQuery>, HotelQueryValidator>();
            services.AddScoped<IValidator<ParkingPlaceQuery>, ParkingPlaceValidator>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IHotelRoomService, HotelRoomService>();
            services.AddScoped<IParkingService, ParkingService>();
            services.AddScoped<IParkingPlaceService, ParkingPlaceService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelReview", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HotelSeeder seeder)
        {
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelReview v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
