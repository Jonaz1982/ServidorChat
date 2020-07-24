using System;
using System.IO;
using System.Drawing;


public class Archivo
{
    // Método estático para abrir archivos planos
    public static StreamReader AbrirArchivo(String NombreArchivo)
    {
        // Existe el archivo?
        if (File.Exists(NombreArchivo))
        {
            /* captura de error estructurada
             * Intentar realizar la instrucción de apertura del archivo.
             * Es susceptible de no poder realizarse */
            try
            {
                /* Apertura del archivo plano
                 * La clase StreamReader permite manipular secuencia de caracteres.
                 * La clase abstracta File ofrece funcionalidad para operar con archivos */
                StreamReader sr = File.OpenText(NombreArchivo);
                return sr;
            }
            catch (IOException ex)
            {
                /* Sucedió un error que se captura mediante la clase IOException
                 * encargada de manipular errores de entrada y salida */
                throw new ArgumentException("Error al abrir archivo:\n" + ex.Message);
            }
        }
        else return null;
    }

    // Método estático para abrir archivos planos
    public static Boolean GuardarArchivo(String NombreArchivo, String[] Lineas)
    {
        if (Lineas != null)
        {
            /* captura de error estructurada. Intenta realizar la instrucción de
             * escritura del archivo. Es susceptible de no poder realizarse */
            try
            {
                //Abrir el archivo para escritura
                StreamWriter sw = new StreamWriter(NombreArchivo);
                for (int i = 0; i < Lineas.Length; i++)
                {
                    //Guardar cada linea
                    sw.WriteLine(Lineas[i]);
                }
                //Cerrar el archivo
                sw.Close();
                return true;
            }
            catch (IOException ex)
            {
                /* Sucedió un error que se captura mediante la clase IOException
                 * encargada de manipular errores de entrada y salida */
                throw new ArgumentException("Error al guardar archivo:\n" + ex.Message);
            }
        }
        else
        {
            return false;
        }
    }

    // Método estático para abrir imágenes desde un archivo
    public static Image AbrirImagen(String NombreArchivo)
    {
        // Existe el archivo?
        if (File.Exists(NombreArchivo))
        {
            try
            {
                /* La clase abstracta Image ofrece funcionalidad para operar con imágenes */
                return Image.FromFile(NombreArchivo);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al abrir imagen:\n" + ex.Message);
            }
        }
        else return null;
    }

    // Método estático para cargar imágenes desde un archivo a un vector de octetos
    public static Byte[] CargarImagen(String NombreArchivo)
    {
        // Existe el archivo?
        if (File.Exists(NombreArchivo))
        {
            try
            {
                //La clase FILESTREAM leerá la imagen
                FileStream fs = new FileStream(@NombreArchivo, FileMode.Open, FileAccess.Read);
                //La imagen seré leida en un vector de octetos
                Byte[] Imagen = new Byte[fs.Length];
                fs.Read(Imagen, 0, Convert.ToInt32(fs.Length));
                //Cerrar el archivo
                fs.Close();
                //Retornar el vector con los octetos
                return Imagen;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al abrir imagen:\n" + ex.Message);
            }
        }
        else return null;
    }
}

