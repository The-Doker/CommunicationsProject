using CommunicationsProject.Jobs;
using CommunicationsProject.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationsProject
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

            services.AddControllers();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                //Get config
                var jobName = "MessageSendJob";
                var cronSchedule = Configuration["Quartz:MessageSendJob"];
                if (string.IsNullOrEmpty(cronSchedule))
                {
                    throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {jobName}");
                }

                // Create a "key" for the job
                var jobKey = new JobKey(jobName);
                
                // Register the job with the DI container
                q.AddJob<MessageSender>(opts => opts.WithIdentity(jobKey));

                // Create a trigger for the job
                q.AddTrigger(opts => opts
                    .ForJob(jobKey) 
                    .WithIdentity("MessageSendJob-trigger") 
                    .WithCronSchedule(cronSchedule));

            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddTransient<IMessageProducer,RabbitMessageProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
