using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Configuration;

using DescargaAutomaticaService.ServicioWindows;

namespace DescargaAutomaticaService
{
    public partial class Service1 : ServiceBase
    {
        private static IScheduler sched;


        public Service1()
        {
            InitializeComponent();
        }

        public void StartFromDebugger(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            //proceso dos alerta reparos
            string tiempoEjecucionActualizacionAutomatica = ConfigurationManager.AppSettings["tiempoEjecucionActualizacionAutomatica"].ToString();
            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
            // jobs can be scheduled before sched.start() has been called
            // job 1 will run every 20 seconds
            IJobDetail job1 = JobBuilder.Create<DescargaAutomaticaActualizacion>()
                .WithIdentity("job1", "group1")
                .Build();

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                      .WithIdentity("trigger1", "group1")
                                                      .WithCronSchedule(tiempoEjecucionActualizacionAutomatica)
                                                      .Build();

            DateTimeOffset ft = sched.ScheduleJob(job1, trigger);

            //All of the jobs have been added to the scheduler, but none of the
            //jobs
            // will run until the scheduler has been started
            sched.Start();

        }

        protected override void OnStop()
        {

            sched.Shutdown(true);

        }
    }
}
