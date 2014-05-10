using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace AutomotoraWeb.Utils {
    public class ImageUtils {
        public static Image CambiarTamanio(Image imagen, int ancho, int alto, int porcentaje) {
        
           //Si vienen alto y ancho mayor que cero, usar estos valores.
            // si vienen alto o ancho mayor que cero y el otro en cero, usar el valor positivo y el otro calcularlo para mantener la proporcion
            // si vienen los dos en cero, usar el porcentaje
            // si todo es cero, dejar el tamanio original y no hacer nada

            int nuevo_alto = imagen.Height;
            int nuevo_ancho = imagen.Width;

            if (ancho == 0 && alto == 0 && porcentaje == 0) {
                return imagen;
            }
            if (ancho < 0 || alto < 0 || porcentaje < 0) {
                return imagen;
            }


            if (ancho > 0 && alto == 0) {
                nuevo_ancho = ancho;
                double x = ((double)imagen.Height / (double)imagen.Width) * (double)nuevo_ancho;
                nuevo_alto = (int)x;
            }
            if (alto > 0 && ancho == 0) {
                nuevo_alto = alto;
                double x= ((double)imagen.Width / (double)imagen.Height) * (double)nuevo_alto;
                nuevo_alto = (int)x;
            }
            if (alto > 0 && ancho > 0) {
                nuevo_alto = alto;
                nuevo_ancho = ancho;
            }
            if (alto == 0 && ancho == 0) {
                double x = (double)alto * (double)porcentaje / (double)100;
                nuevo_alto = (int)x;

                x = (double)ancho * (double)porcentaje / (double)100;
                nuevo_ancho = (int)x;
            }

            Bitmap result = new Bitmap(nuevo_ancho, nuevo_alto);
            using (Graphics g = Graphics.FromImage(result)) {
                g.DrawImage(imagen, 0, 0, nuevo_ancho, nuevo_alto);
                g.Dispose();
            }
            imagen.Dispose();

            return result;
        }

    }
}