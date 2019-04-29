using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DescargaAutomaticaService.BLL;
using DescargaAutomaticaService.Utilitario;

namespace DescargaAutomaticaService.ServicioWindows
{
    public  class DescargaAutomaticaActualizacion : IJob
    {
        Logger log = LogManager.GetCurrentClassLogger();
        public Semaforo semaforo = Semaforo.getInstance("EnvioNotificacionActualizacion");

        public void Execute(IJobExecutionContext context)
        {
            if (semaforo.EstaProcesando == false)
            {
                semaforo.EstaProcesando = true;

                try
                {
                    DescargaAutomaticaActualizacionBLL descargaAutomaticaActualizacionBll = new DescargaAutomaticaActualizacionBLL();
                    EmailBLL emailBll = new EmailBLL();
                    Task tarea1 = new Task(() => descargaAutomaticaActualizacionBll.ExtraerInformacionPaginaSoporte());
                    Task tarea2 = new Task(() => emailBll.EnvioArchivoActualizacionSoftware());
                    tarea1.Start();
                    tarea2.Start();
                    Task.WaitAll(tarea1);
                }
                catch (Exception e)
                {
                    Logger log = LogManager.GetLogger("Error");
                    log.Error(e.Message.ToString());

                    EmailBLL managerEmail = new EmailBLL();
                    managerEmail.EnviarMailAdmin("ERROR: " + e.Message.ToString(), "Envío notificación Descarga Automatica CEPET");
                }
                finally
                {
                    if (semaforo.EstaProcesando == true)
                    {
                        Semaforo.setInstance("EnvioNotificacionDescargaAutomaticaActualizacion");
                        if (semaforo.CantidadArchivosConError > 0)

                            Semaforo.setInstance("EnvioNotificacionDescargaAutomaticaActualizacion");
                    }
                }
            }
        }
    }
}
