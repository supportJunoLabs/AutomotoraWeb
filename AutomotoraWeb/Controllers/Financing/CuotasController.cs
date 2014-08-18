﻿using System;
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

namespace AutomotoraWeb.Controllers.Financing
{
    public class CuotasController : FinancingController
    {

        public static string CONTROLLER = "Cuotas";

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Cuota";
            ViewBag.NombreEntidades = "Cuotas";
        }


        #region CobrarCuota

        public ActionResult ReciboCuota(int id) {
            try {
                TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id);
                ViewData["idParametros"] = id;
                return View("ReciboCuota", tr);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        private XtraReport _generarReciboCuota(int id) {
            TRCuotaCobro tr = (TRCuotaCobro)Transaccion.ObtenerTransaccion(id);
            List<TRCuotaCobro> ll = new List<TRCuotaCobro>();
            ll.Add(tr);
            XtraReport rep = new DXReciboCobroCuotas();
            rep.DataSource = ll;
            return rep;
        }

        public ActionResult ReciboCuotaPartial(int idParametros) {
            XtraReport rep = _generarReciboCuota(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return PartialView("_reciboCuota");
        }

        public ActionResult ReciboCuotaExport(int idParametros) {
            XtraReport rep = _generarReciboCuota(idParametros);
            ViewData["idParametros"] = idParametros;
            ViewData["Report"] = rep;
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        #endregion

    }
}