using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL_Backend;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace AutomotoraWeb.Controllers.Sales.Maintenance {
    public class VendedoresController : SalesController, IMaintenance {

        public static string CONTROLLER = "vendedores";

        public static string FILE_RANDOM_NAME = "fileRandomName";
        public static string ACTUAL_PHOTO_FILE_NAME = "actualPhotoFileName";
        public static string PHOTO_FOLDER_TMP = "~/Content/Images/tmp/";
        public static string PHOTO_FOLDER = "~/Content/Images/vendedores/";
        

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vendedor vendedor) {
            return View(_listaVendedores());
        }

        public ActionResult listVendedores() {
            return PartialView("_listVendedores", _listaVendedores());
        }

    
        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            // Add a report to the view data. 
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            rep.DataSource = _listaVendedores();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportVendedores rep = new DXReportVendedores();
            setParamsToReport(rep);
            rep.DataSource = _listaVendedores();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        private void setParamsToReport(XtraReport report) {
            Parameter paramSystemName = new Parameter();
            paramSystemName.Name = "SystemName";
            paramSystemName.Type = typeof(string);
            paramSystemName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME]);
            paramSystemName.Description = "Nombre de la empresa";
            paramSystemName.Visible = false;
            report.Parameters.Add(paramSystemName);

            Parameter paramCompanyName = new Parameter();
            paramCompanyName.Name = "CompanyName";
            paramCompanyName.Type = typeof(string);
            paramCompanyName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME]);
            paramCompanyName.Description = "Nombre de la compania";
            paramCompanyName.Visible = false;
            report.Parameters.Add(paramCompanyName);
        }

        //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            return getVendedor(id);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            return getVendedor(id);
        }

        public ActionResult Delete(int id) {
            return getVendedor(id);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult getVendedor(int id) {
            try {
                Vendedor vendedor = _getVendedor(id);
                Session[ACTUAL_PHOTO_FILE_NAME] = vendedor.Foto; 
                return View(vendedor);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            } 
        }

        private Vendedor _getVendedor(int id) {
            Vendedor vendedor = new Vendedor();
            vendedor.Codigo = id;
            vendedor.Consultar();
            return vendedor;
        }

        private List<Vendedor> _listaVendedores() {
            return Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(Vendedor vendedor) {

            if (ModelState.IsValid) {
                try {
                    if (Session[FILE_RANDOM_NAME] != null){
                        string filePhotoName = (string)(Session[FILE_RANDOM_NAME]);
                        vendedor.Foto = filePhotoName;
                    }

                    vendedor.Agregar();

                    if (Session[FILE_RANDOM_NAME] != null) {
                        string filePhotoName = (string)(Session[FILE_RANDOM_NAME]);
                        this.movePhotoFile(filePhotoName);
                        Session[FILE_RANDOM_NAME] = null;
                    }

                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } 
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    vendedor.ModificarDatos();

                    if (Session[ACTUAL_PHOTO_FILE_NAME] != null) {
                        string filePhotoName = (string)(Session[ACTUAL_PHOTO_FILE_NAME]);
                        this.deletePhotoFile(filePhotoName);
                        Session[ACTUAL_PHOTO_FILE_NAME] = null;
                    }

                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                } 
            }

            return View(vendedor);
        }

        

        [HttpPost]
        public ActionResult Delete(Vendedor vendedor) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)HttpContext.Session.Contents[SessionUtils.SESSION_USER_NAME];
                    string IP = HttpContext.Request.UserHostAddress;
                    string photoFileName = vendedor.Foto;
                    vendedor.Eliminar(userName, IP);

                    if (!String.IsNullOrEmpty(photoFileName)){
                        deletePhotoFile(photoFileName);
                    }

                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(vendedor);
                }
            }

            return View(vendedor);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public JsonResult Upload() {
            string fileName = "";
            string fileRandomName = "";
            for (int i = 0; i < Request.Files.Count; i++) {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                fileName = file.FileName;
                string mimeType = file.ContentType;
                Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                string extension = getExtensionFile(file.FileName);
                fileRandomName = System.IO.Path.GetRandomFileName().Split('.')[0] + extension;
                file.SaveAs(Server.MapPath(PHOTO_FOLDER_TMP) + fileRandomName);
                Session[FILE_RANDOM_NAME] = fileRandomName; 
            }
            return Json(new { fileName = fileRandomName });
        }


        //-----------------------------------------------------------------------------------------------------

        private string getExtensionFile(string fileName) {
            string extension = "";
            if (fileName.Contains('.')) {
                extension = "." + fileName.Split('.')[1];
            }
            return extension;
        }

        private void movePhotoFile(string filePhotoName) {
            string sourceFile = Server.MapPath(PHOTO_FOLDER_TMP) + filePhotoName;
            string destinationFile = Server.MapPath(PHOTO_FOLDER) + filePhotoName;
            System.IO.File.Move(sourceFile, destinationFile);
        }

        private void deletePhotoFile(string filePhotoName) {
            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + filePhotoName)) {
                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + filePhotoName);
            }
        }

        //-----------------------------------------------------------------------------------------------------

    }
}
