using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using SoSicencneSSHAgent.MicroServices;
using GrpcServiceForAngular.Services.DataBase;

namespace SoSicencneSSHAgent
{
    public class GrpcAgentStartUp
    {

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddGrpc();            
            //services.AddCors(o =>
            //{
            //    o.AddPolicy("MyPolicy", builder =>
            //    {
            //        builder.AllowAnyOrigin();
            //        builder.AllowAnyMethod();
            //        builder.AllowAnyHeader();
            //        builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
            //    });
            //});
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseCors("MyPolicy");
            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGrpcService<DataBaseService>().EnableGrpcWeb();
                endpoints.MapGrpcService<LoginServiceMicroserivces>().EnableGrpcWeb();

            });            
        }
    }
}
