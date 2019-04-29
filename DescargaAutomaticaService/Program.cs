using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;


namespace DescargaAutomaticaService
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // We are running with a debugger attached, so start
                // the service directly.
                Service1 service = new Service1();
                string[] args = new string[] { "arg1", "arg2" };
                service.StartFromDebugger(args);
            }
            else
            {
                // We are running as a service so start normally.
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
            new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
