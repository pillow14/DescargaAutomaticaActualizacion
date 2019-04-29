using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Web;

namespace DescargaAutomaticaService.Utilitario
{
    public class CCryptorEngine
    { 
    private static string key = ConfigurationManager.AppSettings["clave.privada"].ToString();

    /// <summary>
    /// Constructor de la clase CCryptorEngine
    /// </summary>
    public CCryptorEngine()
    {
        // Establecer una clave. La misma clave debe ser utilizada para descifrar los datos que 
        // son cifrados con esta clave. pueden ser los caracteres que uno desee
    }


    /// <summary>
    /// Cifrar una cadena utilizando el método de cifrado. Regresa un texto de cifrado.
    /// </summary>
    /// <param name="texto">cadena de caracteres que se va a encriptar</param>
    /// <returns></returns>
    /// 
    public static string Encriptar(string texto)
    {
        //arreglo de bytes donde guardaremos la llave
        byte[] keyArray;
        //arreglo de bytes donde guardaremos el texto que vamos a encriptar
        byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

        //se utilizan las clases de encriptacion proveidas por el Framework
        //Algritmo MD5
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        //se guarda la llave para que se le realice hashing
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        hashmd5.Clear();

        //Algoritmo 3DAS
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        //se empieza con la transformaion de la cadena
        ICryptoTransform cTransform = tdes.CreateEncryptor();

        //arreglo de bytes donde se guarda la cadena cifrada
        byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
        tdes.Clear();
        //se regresa el resultado en forma de una cadena
        return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
    }

    /// <summary>
    /// Desencripta un texto usando el metodo de deble cadena Regresa una cadena desencriptada. 
    /// </summary>
    /// <param name="cipherString">cadena encriptada</param>
    /// <param name="useHashing">Puedes usar el Hasing para encriptar estos datos? pasa true si la respuesta es si</param>
    /// <param name="keyToDecrypt">El nombre de la clave en el archivo app.config para desencriptar</param>
    /// <returns>the decrypted string</returns>
    public static string Desencriptar(string textoEncriptado)
    {
        byte[] keyArray;
        //convierte el texto en una secuencia de bytes
        byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

        //se llama a las clases ke tienen los algoritmos de encriptacion
        //se le aplica hashing
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        hashmd5.Clear();

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

        tdes.Clear();
        string res = UTF8Encoding.UTF8.GetString(resultArray);
        return res;
    }

}
}
