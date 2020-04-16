using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HelloServices.DataModels;
using HelloServices.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HelloServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Use Autofac.
        
        /*public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var builder = new ContainerBuilder();
            builder.RegisterType<Message>().As<IMessage>();
            builder.Register(m => new Message("test")).As<IMessage>();
            builder.Populate(services);
            var applicationContainer = builder.Build();

            return new AutofacServiceProvider(applicationContainer);
        }*/

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var jwtSetting = new JwtSetting();
            Configuration.Bind("JwtSetting", jwtSetting);

            services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.IncludeErrorDetails = true;
                  var keybytes = Encoding.UTF8.GetBytes(jwtSetting.SecurityKey);
                  var signKey = new SymmetricSecurityKey(keybytes);
                  options.TokenValidationParameters = new TokenValidationParameters
                  {                     
                      ValidateIssuer = true,
                      ValidIssuer = jwtSetting.Issuer,
                      ValidAudience = jwtSetting.Audience,
                      IssuerSigningKey = signKey
                  };
              });

            services.Configure<JwtSetting>(Configuration.GetSection("JwtSetting"));

            services.AddSingleton(factory => {
                Func<string, IMessage> accessor = key =>
                {
                    if (key.Equals("message"))
                    {
                        return new Message();
                    }
                    else if (key.Equals("anotherMessage"))
                    {
                        return new AnotherMessage();
                    }
                    else
                    {
                        throw new Exception("not support");
                    }
                };

                return accessor;
            });
            //services.AddTransient<IMessage, Message>();
            services.AddScoped<IMessage, Message>();
            services.AddSingleton<IJWTUtility, JWTUtility>();
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // lifetime hook
            lifeTime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("Application Started");
            });

            lifeTime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("Application Stopping");
            });

            lifeTime.ApplicationStopped.Register(() =>
            {
                Console.WriteLine("Application stopped");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
