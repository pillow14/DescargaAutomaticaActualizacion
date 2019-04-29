using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DescargaAutomaticaService.Utilitario
{
    public class Semaforo
    {
        private static Semaforo instance;
        private static Hashtable hsTable = new Hashtable();
        private bool estaProcesando = false;
        public string key { get; set; }

        public bool EstaProcesando
        {
            get { return estaProcesando; }
            set { estaProcesando = value; }
        }

        private int logEjecucionId = 0;

        public int LogEjecucionId
        {
            get { return logEjecucionId; }
            set { logEjecucionId = value; }
        }
        private int cantidadArchivosAProcesar = 0;

        public int CantidadArchivosAProcesar
        {
            get { return cantidadArchivosAProcesar; }
            set { cantidadArchivosAProcesar = value; }
        }
        private int cantidadArchivosProcesados = 0;

        public int CantidadArchivosProcesados
        {
            get { return cantidadArchivosProcesados; }
            set { cantidadArchivosProcesados = value; }
        }
        private int cantidadArchivosConError = 0;

        public int CantidadArchivosConError
        {
            get { return cantidadArchivosConError; }
            set { cantidadArchivosConError = value; }
        }

        private Semaforo()
        {

        }

        public static Semaforo getInstance(string key)
        {

            //get
            //{

            if (!hsTable.ContainsKey(key))
            {
                instance = new Semaforo();
                hsTable[key] = instance;
            }
            else
            {
                instance = (Semaforo)hsTable[key];
            }
            return instance;
            //}
        }

        public static Semaforo setInstance(string key)
        {
            //set {
            if (hsTable.ContainsKey(key))
            {
                instance = (Semaforo)hsTable[key];
                instance = null;
                hsTable.Remove(key);

            }
            return instance;

            //}
        }
    }

}

