using ITI.Clavardons.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITI.Clavardons
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((arg) => true).AllowCredentials();
                });
            });

            services.AddSignalR().AddJsonProtocol();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseCors();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("");
            });

            //app.Use((context, next) =>
            //{
            //    context.Response.Body.Write(Encoding.UTF8.GetBytes("WebSocket only API."));
            //    context.Response.StatusCode = 400;

            //    return next();
            //});
        }
    }
}
