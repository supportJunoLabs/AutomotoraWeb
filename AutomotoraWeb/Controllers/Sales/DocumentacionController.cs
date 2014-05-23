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

namespace AutomotoraWeb.Controllers.Sales {
    public class DocumentacionController : SalesController {

        public static string CONTROLLER = "Documentacion";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Documentacion";
            ViewBag.NombreEntidades = "Documentacion";

        }

        public ActionResult Index() {
            return View();
        }


        //--------------------------METODOS PARA LISTADOS DE Documentacion   -----------------------------

        #region Listados

        public ActionResult List() {

            ListadoDocumentacionModel model = new ListadoDocumentacionModel();
            string s = SessionUtils.generarIdVarSesion("ListadoDocumentacion", Session[SessionUtils.SESSION_USER].ToString());
            Session[s] = model;
            model.idParametros = s;
            ViewData["idParametros"] = model.idParametros;
            model.EstadosPosibles = EstadoDocumento.EstadosDocumentoListables();
            model.Resultado = _listaElementos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ListadoDocumentacionModel model, string btnSubmit) {
            ListadoDocumentacionModel modelsesion = (ListadoDocumentacionModel) Session[model.idParametros]; 
            var x = CheckBoxListExtension.GetSelectedValues<int>("cbl_estados"); //hay que hacer esto porque por ahora no lo guarda en model el devexpress
            modelsesion.EstadosConsultar = x;
            ViewData["idParametros"] = modelsesion.idParametros;
            
            if (ModelState.IsValid) {
                if (btnSubmit == "Imprimir") {
                    return this.Report(modelsesion);
                }
                modelsesion.Resultado = _listaElementos(modelsesion);
            }
            return View(modelsesion);
        }

        public ActionResult ReportGrilla(string idParametros) {

            ListadoDocumentacionModel model = (ListadoDocumentacionModel)Session[idParametros];
            model.Resultado = _listaElementos(model);
            ViewData["idParametros"] = model.idParametros;
            return PartialView("_reportGrilla", model);
        }

        private List<DocAuto> _listaElementos(ListadoDocumentacionModel model) {
            model.AcomodarFiltro();
            List<EstadoDocumento> ll = new List<EstadoDocumento>();
            if (model.EstadosConsultar != null) {
                foreach (int i in model.EstadosConsultar) {
                    EstadoDocumento aux = new EstadoDocumento { Codigo = i };
                    ll.Add(aux);
                }
            }
            return DocAuto.DocsAutos(ll);
        }

        #endregion
        //---------- METODOS PARA REPORTES DE LISTADOS DE Documentacion  -----------------------------

        #region Reportes
        public ActionResult Report(ListadoDocumentacionModel model) {
            return View("report", model);
        }

        public ActionResult ReportPartial(string idParametros) {
            ListadoDocumentacionModel model = null;
            XtraReport rep = new DXListadoDocumentacion();
            model = (ListadoDocumentacionModel)Session[idParametros];
            rep.DataSource = _listaElementos(model);
            setParamsToReport(rep, model); // lo hago despues  de _listaElementos porque listaElementos acomoda los filtros en model
            Session[idParametros] = model;
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        private void setParamsToReport(XtraReport report, ListadoDocumentacionModel model) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "detalleFiltros";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = model.detallesFiltro();
            paramSystemName.Description = "Detalle Filtros";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);
        }

        public ActionResult ReportExport(string idParametros) {
            ListadoDocumentacionModel model = null;
            XtraReport rep = new DXListadoDocumentacion();
            model = (ListadoDocumentacionModel)Session[idParametros];
            setParamsToReport(rep, model);
            rep.DataSource = _listaElementos(model);
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);

        }
        #endregion



    }
}
