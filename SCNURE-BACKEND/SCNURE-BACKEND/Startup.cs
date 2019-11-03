using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SCNURE_BACKEND.Data;
using SCNURE_BACKEND.Helpers;
using SCNURE_BACKEND.Services.Email;
using SCNURE_BACKEND.Services.Users;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCNURE_BACKEND
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUserService, UserService>();
			services.AddScoped<IEmailService, SmtpService>();
            services.AddScoped<IStartupService, StartupServiceImpl>();


			services.AddDbContext<SCContext>(options => options.UseMySql(Configuration.GetConnectionString("MainMySqlConn")));

            ConfigureJwtAuthorization(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = "SCNURE-BACKEND", Version = "v.0.0.1" });


				c.AddSecurityDefinition("Bearer", new ApiKeyScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = "header",
					Type = "apiKey"
				});
				c.AddSecurityRequirement(
					new Dictionary<string, IEnumerable<string>>
					{
						{ "Bearer", new string[] { } },
					}
				);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

			app.UseAuthentication();

			app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SCNURE-BACKEND");
                c.RoutePrefix = string.Empty;
            });
        }

        private void ConfigureJwtAuthorization(IServiceCollection services)
        {
            var jwtSettingsSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = await userService.GetByIdAsync(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        await Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
