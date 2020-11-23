using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Diagnostics;
using System;
using HangfireWebJobs.Models;
[assembly: OwinStartupAttribute(typeof(HangfireWebJobs.Startup))]
namespace HangfireWebJobs
{
    public partial class Startup
    {
        CreatePriorityJob job = new CreatePriorityJob();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection");
            app.UseHangfireDashboard();

            BackgroundJob.Schedule(() => job.GetPriorityList(), TimeSpan.FromMinutes(1));
            
            app.UseHangfireServer();
        }
    }
}
