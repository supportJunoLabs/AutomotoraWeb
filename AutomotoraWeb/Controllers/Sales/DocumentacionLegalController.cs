using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomotoraWeb.Models;
using DLL_Backend;
using AutomotoraWeb.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.Web.Mvc;
using System.IO;
using System.Text;

namespace AutomotoraWeb.Controllers.Sales {
    public class DocumentacionLegalController : SalesController {

        
               
        public static string atilde="\\'e1";
        public static string etilde="\\'e9";
        public static string itilde="\\'ed";
        public static string otilde="\\'f3";
        public static string utilde="\\'fa";
        public static string enie="\\'f1";
        public static string Atilde="\\'c1";
        public static string Etilde="\\'c9";
        public static string Itilde="\\'cd";
        public static string Otilde="\\'d3";
        public static string Utilde="\\'da";
        public static string Enie="\\'d1";
        public static string ordinal = "\\'b0";

        public static string CONTROLLER = "DocumentacionLegal";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Documentacion Legal";
            ViewBag.NombreEntidades = "Documentacion Legal";
        }


        #region comprobante

        public ActionResult DocComprobanteVenta(int id) {
            Venta v = new Venta();
            v.Codigo = id;
            ////v.Consultar();
            ViewData["idParametros"] = id;
            return View("ReportComprobanteVenta", v);
        }

        public ActionResult ReportComprobanteVentaPartial(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Venta v = new Venta();
            v.Codigo = idParametros;
            v.Consultar();
            XtraReport rep = new DXReciboVenta();
            List<Venta> ll = new List<Venta>();
            ll.Add(v);
            rep.DataSource = ll;
            ViewData["Report"] = rep;
            return PartialView("_reportComprobanteVenta");
        }

        public ActionResult ReportComprobanteVentaExport(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Venta v = new Venta();
            v.Codigo = idParametros;
            v.Consultar();
            XtraReport rep = new DXReciboVenta();
            List<Venta> ll = new List<Venta>();
            ll.Add(v);
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }

        #endregion

        public FileStreamResult DocCompromisoVenta(int id) {
            return generarDocumento(id, "CV_VENTA", "CVV");
        }

         public FileStreamResult DocConformesVenta(int id) {
            return generarDocumento(id, "CUOTA_VENTA", "CONF");
        }

         public FileStreamResult DocTituloVenta(int id) {
             return generarDocumento(id, "TITULO_VENTA", "TIT");
        }

         public FileStreamResult DocValesVenta(int id) {
             return generarDocumento(id, "VALE_VENTA", "VAL");
        }

         public ActionResult DocCuponesVenta(int id) {
             Venta v = new Venta();
             v.Codigo = id;
             ////v.Consultar();
             ViewData["idParametros"] = id;
             return View("ReportCuponesVenta", v);
        }

         private XtraReport _reporteCupones(int idVenta) {
             DocumentoLegal doc = new DocumentoLegal();
             doc.Codigo = "CUPONES_VENTA";
             doc.Consultar();

             Venta v = new Venta();
             v.Codigo = idVenta;
             v.Consultar();

             VentaCupones vc = doc.ObtenerVentaCupones(v);
             vc.Cabezal = vc.Cabezal.Replace("[RETURN]", Environment.NewLine);
             XtraReport rep = new DXCuponesVenta();
             List<VentaCupones> ll = new List<VentaCupones>();
             ll.Add(vc);
             rep.DataSource = ll;

             return rep;
         }

         public ActionResult ReportCuponesVentaPartial(int idParametros) {
             ViewData["idParametros"] = idParametros;
             XtraReport rep = _reporteCupones(idParametros);
             ViewData["Report"] = rep;
             return PartialView("_reportCuponesVenta");
         }

         public ActionResult ReportCuponesVentaExport(int idParametros) {
             ViewData["idParametros"] = idParametros;
             ViewData["idParametros"] = idParametros;
             XtraReport rep = _reporteCupones(idParametros);
             return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

         }

         public FileStreamResult Vale(string id) {
             Vale val = new Vale();
             val.Codigo = id;
             val.Consultar();

             DocumentoLegal doc = new DocumentoLegal();
             doc.Codigo = "VALE_VENTA";
             doc.Consultar();

             string contenido = generarWord(doc.ObtenerContenido(val));

             Response.ContentType = "application/word";
             Response.AddHeader("Content-disposition", "attachment; filename=" + "VAL" + "_" + val.Codigo + ".rtf");
             Response.Buffer = true;
             Response.Clear();
             Response.Write(contenido);
             Response.OutputStream.Flush();
             Response.End();

             return new FileStreamResult(Response.OutputStream, "application/word");
         }
            

        private FileStreamResult generarDocumento(int idVenta, string tipoDoc, string nomDoc) {
            Venta v = new Venta();
            v.Codigo = idVenta;
            v.Consultar();

            DocumentoLegal doc = new DocumentoLegal();
            doc.Codigo = tipoDoc;
            doc.Consultar();

            string contenido = generarWord(doc.ObtenerContenido(v));

            Response.ContentType = "application/word";
            Response.AddHeader("Content-disposition", "attachment; filename=" + nomDoc + "_" + v.Vehiculo.Codigo + ".rtf");
            Response.Buffer = true;
            Response.Clear();
            Response.Write(contenido);
            Response.OutputStream.Flush();
            Response.End();

            return new FileStreamResult(Response.OutputStream, "application/word");
        }


        private string generarWord(string sbase) {

            //build the content for the dynamic Word document
            //in HTML alongwith some Office specific style properties. 
            StringBuilder strBody = new System.Text.StringBuilder("");

            //caracteres reservados de RTF
            sbase = sbase.Replace("{", "");
            sbase = sbase.Replace("}", "");
            sbase = sbase.Replace("\\", "");

            strBody.Append("{\\rtf1\\ansi\\deff0 ");

            //strBody.Append("<html " +
            //        "xmlns:o='urn:schemas-microsoft-com:office:office' " +
            //        "xmlns:w='urn:schemas-microsoft-com:office:word'" +
            //        "xmlns='http://www.w3.org/TR/REC-html40'>" +
            //        "<head><title>TITULO</title>");

            ////The setting specifies document's view after it is downloaded as Print
            ////instead of the default Web Layout
            //strBody.Append("<!--[if gte mso 9]>" +
            //                         "<xml>" +
            //                         "<w:WordDocument>" +
            //                         "<w:View>Print</w:View>" +
            //                         "<w:Zoom>90</w:Zoom>" +
            //                         "<w:DoNotOptimizeForBrowser/>" +
            //                         "</w:WordDocument>" +
            //                         "</xml>" +
            //                         "<![endif]-->");

            //strBody.Append("<style>" +
            //    "<!-- /* Style Definitions */" +
            //                        "@page Section1" +
            //                        "   {size:8.5in 11.0in; " +
            //                        "   margin:1.0in 1.25in 1.0in 1.25in ; " +
            //                        "   mso-header-margin:.5in; " +
            //                        "   mso-footer-margin:.5in; mso-paper-source:0;}" +
            //                        " div.Section1" +
            //                        "   {page:Section1;}" +
            //                        "-->" +
            //                       "</style></head>");

            //strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
            //                        "<div class=Section1>");


            sbase = reemplazoPar(sbase, "[BOLD]", "\\b ",  "\\b0 ");
            sbase = reemplazoPar(sbase, "[UNDERLINE]", "\\ul ",  "\\ul0 ");
            sbase = sbase.Replace("[RETURN]", "\\line ");
            sbase = sbase.Replace("[JUSTIFY]", "\\par\\qj ");
            sbase = sbase.Replace("[LEFT]", "\\par\\ql ");
            sbase = sbase.Replace("[CENTER]", "\\par\\qc ");
            sbase = sbase.Replace("[RIGHT]", "\\par\\qr ");
            sbase = sbase.Replace("[PAGINA]", "\\page ");

            sbase = sbase.Replace("á", atilde);
            sbase = sbase.Replace("é", etilde);
            sbase = sbase.Replace("í", itilde);
            sbase = sbase.Replace("ó", otilde);
            sbase = sbase.Replace("ú", utilde);
            sbase = sbase.Replace("ñ", enie);
            sbase = sbase.Replace("Á", Atilde);
            sbase = sbase.Replace("É", Etilde);
            sbase = sbase.Replace("Í", Itilde);
            sbase = sbase.Replace("Ó", Otilde);
            sbase = sbase.Replace("Ú", Utilde);
            sbase = sbase.Replace("Ñ", Enie);
            sbase = sbase.Replace("º", ordinal);
            sbase = sbase.Replace("°", ordinal);
            

            strBody.Append(sbase);

            //strBody.Append("</div></body></html>");

            strBody.Append("}");

            return strBody.ToString();
        }


        public string reemplazoPar(string sbase, string original, string comienzo, string fin) {
            string resultado = sbase;

            int pos = resultado.IndexOf(original);
            bool abrir = true;
            while (pos >= 0) {
                resultado = resultado.Remove(pos, original.Length);
                if (abrir) {
                    resultado = resultado.Insert(pos, comienzo);
                } else {
                    resultado = resultado.Insert(pos, fin);
                }
                abrir = !abrir;
                pos = resultado.IndexOf(original);
            }
            return resultado;
        }
    }
}
