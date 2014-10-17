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
using AutomotoraWeb.Services;

namespace AutomotoraWeb.Controllers.Sales {
    public class DocumentacionLegalController : SalesController {



        public static string atilde = "\\'e1";
        public static string etilde = "\\'e9";
        public static string itilde = "\\'ed";
        public static string otilde = "\\'f3";
        public static string utilde = "\\'fa";
        public static string enie = "\\'f1";
        public static string Atilde = "\\'c1";
        public static string Etilde = "\\'c9";
        public static string Itilde = "\\'cd";
        public static string Otilde = "\\'d3";
        public static string Utilde = "\\'da";
        public static string Enie = "\\'d1";
        public static string ordinal = "\\'b0";

        public static string CONTROLLER = "DocumentacionLegal";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Documentacion Legal";
            ViewBag.NombreEntidades = "Documentacion Legal";
        }

        private bool VentaConsultable(Venta v) {
            if (v == null || v.Codigo == 0) return true;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (!SecurityService.Instance.verInfoAntigua(usuario) && v.Antiguo) {
                return false;
            }
            return true;
        }

        private bool SeniaConsultable(Senia s) {
            if (s == null || s.Codigo == 0) return true;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (!SecurityService.Instance.verInfoAntigua(usuario) && s.Antiguo) {
                return false;
            }
            return true;
        }

        private bool ValeConsultable(Vale v) {
            if (v == null || string.IsNullOrWhiteSpace(v.Codigo)) return true;
            Usuario usuario = (Usuario)(Session[SessionUtils.SESSION_USER]);
            if (!SecurityService.Instance.verInfoAntigua(usuario) && v.Antiguo) {
                return false;
            }
            return true;
        }


        #region comprobanteVenta

        public ActionResult DocComprobanteVenta(int id) {
            Venta v = new Venta();
            v.Codigo = id;
            v.Consultar();
            if (!VentaConsultable(v)) {
                return View("_transaccionAntigua");
            }
            ViewData["idParametros"] = id;
            return View("ReportComprobanteVenta", v);
        }

        public ActionResult ReportComprobanteVentaPartial(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Venta v = new Venta();
            v.Codigo = idParametros;
            v.Consultar();
            if (!VentaConsultable(v)) {
                v = new Venta();
            }
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
            if (!VentaConsultable(v)) {
                v = new Venta();
            }
            XtraReport rep = new DXReciboVenta();
            List<Venta> ll = new List<Venta>();
            ll.Add(v);
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }

        #endregion

        #region comprobanteSenia

        public ActionResult DocComprobanteSenia(int id) {
            Senia s = new Senia();
            s.Codigo = id;
            s.Consultar();
            if (!SeniaConsultable(s)) {
                return View("_transaccionAntigua");
            }
            ViewData["idParametros"] = id;
            return View("ReportComprobanteSenia", s);
        }

        public ActionResult ReportComprobanteSeniaPartial(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Senia s = new Senia();
            s.Codigo = idParametros;
            s.Consultar();
            if (!SeniaConsultable(s)) {
                s = new Senia();
            }
            XtraReport rep = new DXReciboSenia();
            List<Senia> ll = new List<Senia>();
            ll.Add(s);
            rep.DataSource = ll;
            ViewData["Report"] = rep;
            return PartialView("_reportComprobanteSenia");
        }

        public ActionResult ReportComprobanteSeniaExport(int idParametros) {
            ViewData["idParametros"] = idParametros;
            Senia s = new Senia();
            s.Codigo = idParametros;
            s.Consultar();
            if (!SeniaConsultable(s)) {
                s = new Senia();
            }
            XtraReport rep = new DXReciboSenia();
            List<Senia> ll = new List<Senia>();
            ll.Add(s);
            rep.DataSource = ll;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }

        #endregion


        public FileStreamResult DocCompromisoSenia(int id) {
            return generarDocumento(0, id, "CV_SENIA", "CVS");
        }

        public FileStreamResult DocPromesaSenia(int id) {
            return generarDocumento(0, id, "PR_SENIA", "PROM");
        }

        public FileStreamResult DocCompromisoVenta(int id) {
            return generarDocumento(id, 0, "CV_VENTA", "CVV");
        }

        public FileStreamResult DocConformesVenta(int id) {
            return generarDocumento(id, 0, "CUOTA_VENTA", "CONF");
        }

        public FileStreamResult DocTituloVenta(int id) {
            return generarDocumento(id, 0, "TITULO_VENTA", "TIT");
        }

        public FileStreamResult DocValesVenta(int id) {
            return generarDocumento(id, 0, "VALE_VENTA", "VAL");
        }

        public ActionResult DocCuponesVenta(int id) {
            Venta v = new Venta();
            v.Codigo = id;
            v.Consultar();
            if (!VentaConsultable(v)) {
                return View("_transaccionAntigua");
            }
            ViewData["idParametros"] = id;
            return View("ReportCuponesVenta", v);
        }

        private XtraReport _reporteCupones(int idVenta) {
            List<VentaCupones> ll = new List<VentaCupones>();
            Venta v = new Venta();
            v.Codigo = idVenta;
            v.Consultar();
            if (VentaConsultable(v)) {
                DocumentoLegal doc = new DocumentoLegal();
                doc.Codigo = "CUPONES_VENTA";
                doc.Consultar();
                VentaCupones vc = doc.ObtenerVentaCupones(v);
                vc.Cabezal = vc.Cabezal.Replace("[RETURN]", Environment.NewLine);
                ll.Add(vc);
            }
            XtraReport rep = new DXCuponesVenta();
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

            string contenido = "";
            if (!ValeConsultable(val)) {
                contenido = "Transaccion antigua ya no se encuentra en linea";
            } else {
                DocumentoLegal doc = new DocumentoLegal();
                doc.Codigo = "VALE_VENTA";
                doc.Consultar();
                contenido = generarWord(doc.ObtenerContenido(val));
            }

            Response.ContentType = "application/word";
            Response.AddHeader("Content-disposition", "attachment; filename=" + "VAL" + "_" + val.Codigo + ".rtf");
            Response.Buffer = true;
            Response.Clear();
            Response.Write(contenido);
            Response.OutputStream.Flush();
            Response.End();

            return new FileStreamResult(Response.OutputStream, "application/word");
        }


        private FileStreamResult generarDocumento(int idVenta, int idSenia, string tipoDoc, string nomDoc) {
            DocumentoLegal doc = new DocumentoLegal();
            doc.Codigo = tipoDoc;
            doc.Consultar();

            string contenido = "";
            Vehiculo vhc = null;
            if (idVenta > 0) {
                Venta v = new Venta();
                v.Codigo = idVenta;
                v.Consultar();
                if (VentaConsultable(v)) {
                    contenido = generarWord(doc.ObtenerContenido(v));
                } else {
                    contenido = "Transaccion antigua ya no se encuentra en linea";
                }
                vhc = v.Vehiculo;

            } else {
                Senia s = new Senia();
                s.Codigo = idSenia;
                s.Consultar();
                if (SeniaConsultable(s)) {
                    contenido = generarWord(doc.ObtenerContenido(s));
                } else {
                    contenido = "Transaccion antigua ya no se encuentra en linea";
                }
                vhc = s.Vehiculo;
            }

            Response.ContentType = "application/word";
            Response.AddHeader("Content-disposition", "attachment; filename=" + nomDoc + "_" + vhc.Codigo + ".rtf");
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


            sbase = reemplazoPar(sbase, "[BOLD]", "\\b ", "\\b0 ");
            sbase = reemplazoPar(sbase, "[UNDERLINE]", "\\ul ", "\\ul0 ");
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
