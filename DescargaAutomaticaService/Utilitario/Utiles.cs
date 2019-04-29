using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using DescargaAutomaticaService.Utilitario;
using System.Configuration;
using OpenQA.Selenium;
using System.IO;
using System.IO.Compression;

namespace DescargaAutomaticaService.Utilitario
{
    public class Utiles
    {
        /// <summary>
        /// comprobar si existe elemento en pagina
        /// </summary>
        /// <param name="by"></param>
        /// <param name="firefoxDriver"></param>
        /// <returns></returns>
        public Boolean ElementoEstaPresente(By by, FirefoxDriver firefoxDriver) 
        {
            try
            {
                firefoxDriver.FindElement(by);
                return true;
            }
            catch(NoSuchElementException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Compresion de archivos a ZIP
        /// </summary>
        /// <param name="fi"></param>
        public  void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            ////Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                            //    fi.Name, fi.Length.ToString(), outFile.Length.ToString());
                        }
                    }
                }
            }
        }
    }
}
