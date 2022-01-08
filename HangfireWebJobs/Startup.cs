using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Diagnostics;
using System;
using HangfireWebJobs.Models;
using HangfireWebJobs.Services;
[assembly: OwinStartupAttribute(typeof(HangfireWebJobs.Startup))]
namespace HangfireWebJobs
{
    public partial class Startup
    {
        CreatePriorityJob job = new CreatePriorityJob();
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
                GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection");
                app.UseHangfireDashboard();

                BackgroundJob.Schedule(() => job.GetPriorityList(), TimeSpan.FromMinutes(1));

                app.UseHangfireServer();
            }
            catch (Exception ex)
            {
                LoggingService service2 = new LoggingService("Startup/Configuration", ex.Message, System.DateTime.Now);
                service2.LogError();
            }
        }
    }
}
