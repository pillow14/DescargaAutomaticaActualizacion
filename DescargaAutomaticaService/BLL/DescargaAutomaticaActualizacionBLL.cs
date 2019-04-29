using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium.Firefox;
using DescargaAutomaticaService.Utilitario;
using System.Configuration;
using OpenQA.Selenium;
using System.Threading;

namespace DescargaAutomaticaService.BLL
{
    public class DescargaAutomaticaActualizacionBLL
    {
        Utiles utiles = new Utiles();
        FileInfo[] files = null;

        /// <summary>
        /// Extrae descargables en caso de que exista actualizacion
        /// </summary>
        /// <returns>objeto tipo FileInfo</returns>
        public FileInfo [] ExtraerInformacionPaginaSoporte()
        {
            try
            {
                FirefoxOptions profile = new FirefoxOptions();
                profile.SetPreference("browser.download.folderlist", 2);
                profile.SetPreference("browser.download.dir", ConfigurationManager.AppSettings["carpetaDescargar"].ToString());
                profile.SetPreference("browser.helperApps.neverAsk.saveToDisk","application/octet-stream");

                FirefoxDriver driver = new FirefoxDriver(@"\\Mac\Home\Downloads\geckodriver-v0.23.0-win64",profile);
                string rutEncriptado = Utilitario.CCryptorEngine.Encriptar(ConfigurationManager.AppSettings["rut"].ToString());
                string claveEncriptado = Utilitario.CCryptorEngine.Encriptar(ConfigurationManager.AppSettings["clave"].ToString());

                driver.Navigate().GoToUrl("http://www.cepet.cl/soporte/soportelogin.php");
                driver.FindElementById("rutclie").SendKeys(Utilitario.CCryptorEngine.Desencriptar(rutEncriptado));
                driver.FindElementById("numclie").SendKeys(Utilitario.CCryptorEngine.Desencriptar(claveEncriptado));

                driver.FindElement(By.XPath("//img[@src='imagenes/ingresar.png']")).Click();

                driver.Navigate().GoToUrl("http://www.cepet.cl/soporte/menu_dinamico.php");

                IList<IWebElement> listaTodosTd = driver.FindElements(By.XPath(".//td[@class='Texto_verdana']//ul[@class='Texto_verdana']//li//img[@src='imagenes/new2.gif']"));

                if (listaTodosTd.Count > 0)
                {
                    foreach (var lista in listaTodosTd)
                    {
                        lista.FindElement(By.XPath("//td[@class='Texto_verdana']//ul[@class='Texto_verdana']//li//a")).Click();

                        if (!Directory.Exists(ConfigurationManager.AppSettings["carpetaDescargar"].ToString()))
                        {
                            Directory.CreateDirectory(ConfigurationManager.AppSettings["carpetaDescargar"].ToString());
                        }

                        IWebElement elemento = driver.FindElement(By.XPath("//input[@type='submit']"));

                        elemento.Click();

                        DirectoryInfo d = new DirectoryInfo(ConfigurationManager.AppSettings["carpetaDescargar"].ToString());//Assuming Test is your Folder
                        files = d.GetFiles("*.exe"); //Getting Text files

                        driver.Close();

                    }
                }
                else
                {
                    driver.Close();                
                }

                return files;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }    
        
    }
}
