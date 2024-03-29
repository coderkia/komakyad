using Kia.KomakYad.DataAccess;
using Kia.KomakYad.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Kia.KomakYad.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using Kia.KomakYad.Api.Helpers;
using Kia.KomakYad.Common.Services;
using Kia.KomakYad.Common.Configurations;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Kia.KomakYad.Api.Configs;

namespace Kia.KomakYad.Api
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
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthHelper.AdminPolicy, policy => policy.RequireRole(AuthHelper.AdminRole));
                options.AddPolicy(AuthHelper.ReadPolicy, policy => policy.RequireRole(AuthHelper.AdminRole, AuthHelper.MemberRole, AuthHelper.ModeratorRole, AuthHelper.ReporterRole));
            });

            services.Configure<EmailConfiguration>(Configuration.GetSection(typeof(EmailConfiguration).Name));
            services.Configure<ReCaptchaConfig>(Configuration.GetSection("ReCaptcha"));
            services.AddCors();
            services.AddHttpClient<IReCaptchaService, ReCaptchaService>();

            services.AddDbContext<DataContext>(c => {
                c.UseLazyLoadingProxies();
                c.UseSqlServer(Configuration.GetConnectionString("DefaultConnections")); }
            );

            services.AddAutoMapper(typeof(LeitnerRepository).Assembly, typeof(Program).Assembly, typeof(User).Assembly);
            services.AddTransient<ILeitnerRepository, LeitnerRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddSingleton<EmailService>();
            services.AddSingleton<IReCaptchaService, ReCaptchaService>();

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); ;
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
                app.UseExceptionHandler(builder => builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }
                }));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
