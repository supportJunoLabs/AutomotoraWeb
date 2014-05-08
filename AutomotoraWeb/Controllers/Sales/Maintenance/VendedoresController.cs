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
using System.Drawing;

namespace AutomotoraWeb.Controllers.Sales.Maintenance {
    public class VendedoresController : SalesController, IMaintenance {

        public static string CONTROLLER = "vendedores";

        public const string FILE_RANDOM_NAME = "fileRandomName";
        public const string ACTUAL_PHOTO_FILE_NAME = "actualPhotoFileName";
        public const string PHOTO_FOLDER_TMP = "~/Content/Images/tmp/";
        public const string PHOTO_FOLDER = "~/Content/Images/vendedores/";
        private const int MAX_ANCHO_FOTO = 250;
        public const string IMAGEN_SIN_FOTO = "_sinFoto.jpg";


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
            //setParamsToReport(rep);
            ViewData["Report"] = rep;
            return View();
        }

        public ActionResult ReportPartial() {
            DXReportVendedores rep = new DXReportVendedores();
            //setParamsToReport(rep);
            rep.DataSource = _listaVendedores();
            ViewData["Report"] = rep;
            return PartialView("_reportList");
        }

        public ActionResult ReportExport() {
            DXReportVendedores rep = new DXReportVendedores();
            //setParamsToReport(rep);
            rep.DataSource = _listaVendedores();
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(rep);
        }

        //private void setParamsToReport(XtraReport report) {
        //    Parameter paramSystemName = new Parameter();
        //    paramSystemName.Name = "SystemName";
        //    paramSystemName.Type = typeof(string);
        //    paramSystemName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_SYSTEM_NAME]);
        //    paramSystemName.Description = "Nombre de la empresa";
        //    paramSystemName.Visible = false;
        //    report.Parameters.Add(paramSystemName);

        //    Parameter paramCompanyName = new Parameter();
        //    paramCompanyName.Name = "CompanyName";
        //    paramCompanyName.Type = typeof(string);
        //    paramCompanyName.Value = (string)(HttpContext.Application.Contents[SessionUtils.APPLICATION_COMPANY_NAME]);
        //    paramCompanyName.Description = "Nombre de la compania";
        //    paramCompanyName.Visible = false;
        //    report.Parameters.Add(paramCompanyName);
        //}

        //--------------------------------------------------------------------------------------------------

        public ActionResult Details(int id) {
            return getVendedor(id, true);
        }

        public ActionResult Create() {
            Session[FILE_RANDOM_NAME] = null;
            return View();
        }

        public ActionResult Edit(int id) {
            Session[FILE_RANDOM_NAME] = null;
            return getVendedor(id, false);
        }

        public ActionResult Delete(int id) {
            return getVendedor(id, true);
        }

        public ActionResult Cancelar() {
             if (Session[FILE_RANDOM_NAME] != null && (string)Session[FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME])) {
                    System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME]);
                }
            }
            Session[FILE_RANDOM_NAME] = null;
            return RedirectToAction(BaseController.SHOW);
        }

        //-----------------------------------------------------------------------------------------------------


        private ActionResult getVendedor(int id, bool fotosinimagen) {
            try {
                Vendedor vendedor = _getVendedor(id);
                if (fotosinimagen) {
                    if (vendedor.Foto == null || vendedor.Foto.Trim() == "") {
                        vendedor.Foto = IMAGEN_SIN_FOTO;
                    }
                }
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
                    if (Session[FILE_RANDOM_NAME] != null && (string)Session[FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                        string filePhotoName = (string)(Session[FILE_RANDOM_NAME]);
                        vendedor.Foto = filePhotoName;
                    }

                    vendedor.Agregar(); // Si hay foto le cambia el nombre

                    if (Session[FILE_RANDOM_NAME] != null && (string)Session[FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                        // Movemos del directorio temporario al de vendedores y se actualiza el nombre

                        if (Session[ACTUAL_PHOTO_FILE_NAME] != null) { //si antes habia se borra
                            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[ACTUAL_PHOTO_FILE_NAME])) {
                                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + Session[ACTUAL_PHOTO_FILE_NAME]);
                            }
                        }
                        //el nuevo se mueve a la carpeta destiono
                        System.IO.File.Move((Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME]),
                                            Server.MapPath(PHOTO_FOLDER) + vendedor.Foto);

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
                    if (Session[FILE_RANDOM_NAME] != null) { //hice algun cambio
                        if ((string)(Session[FILE_RANDOM_NAME]) == IMAGEN_SIN_FOTO) {
                            vendedor.Foto = null;  //borre la foto
                        } else {
                            vendedor.Foto = (string)(Session[FILE_RANDOM_NAME]); //agrege una foto
                        }
                    } else {
                        //no hice ningun cambio,mantener lo anterior.
                        vendedor.Foto = (string)Session[ACTUAL_PHOTO_FILE_NAME]; //a sea foto o null
                    }

                    vendedor.ModificarDatos(); //me asigna el nuevo nombre de la foto

                    if (Session[FILE_RANDOM_NAME] == null) {
                        //No hice ningun cambio
                        return RedirectToAction(BaseController.SHOW);
                    }

                    if ((string)Session[FILE_RANDOM_NAME] == IMAGEN_SIN_FOTO) {
                        //borre la foto anterior si la habia, y no puse ninguna
                        if (Session[ACTUAL_PHOTO_FILE_NAME] != null) {
                            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[ACTUAL_PHOTO_FILE_NAME])) {
                                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + Session[ACTUAL_PHOTO_FILE_NAME]);
                            }
                        }
                        return RedirectToAction(BaseController.SHOW);
                    }

                    //puse uno nuevo, debo pasarlo al dir destino y borrar el anterior si lo hay

                    if (Session[ACTUAL_PHOTO_FILE_NAME] != null) { //si antes habia se borra
                        if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[ACTUAL_PHOTO_FILE_NAME])) {
                            System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) +Session[ACTUAL_PHOTO_FILE_NAME]);
                        }
                    }
                    //el nuevo se mueve a la carpeta destiono
                    System.IO.File.Move(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME],
                                        Server.MapPath(PHOTO_FOLDER) + vendedor.Foto);


                    //string actualFilePhotoName = (string)(Session[ACTUAL_PHOTO_FILE_NAME]);
                    //if (Session[FILE_RANDOM_NAME] != null) {
                    //    // Renombramos el Actual (para poder eliminarlo)
                    //    if (!String.IsNullOrEmpty(actualFilePhotoName)) {
                    //        this.renamePhotoFileName(actualFilePhotoName, actualFilePhotoName + "_TO_DELETE");
                    //    }

                    //    // Movemos del directorio temporario al de vendedores y se actualiza el nombre
                    //    string filePhotoName = (string)(Session[FILE_RANDOM_NAME]);
                    //    this.moveAndRenamePhotoFile(filePhotoName, vendedor.Foto);
                    //    Session[FILE_RANDOM_NAME] = null;
                    //}

                    //if (!String.IsNullOrEmpty(actualFilePhotoName)) {
                    //    this.deletePhotoFile(actualFilePhotoName + "_TO_DELETE");
                    //    Session[ACTUAL_PHOTO_FILE_NAME] = null;
                    //}

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
                            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + photoFileName)) {
                                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + photoFileName);
                            }
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
        public JsonResult DeleteFoto() {
            //Si ya se sugio alguna foto anterior al tmp, la voy borrando
            if (Session[FILE_RANDOM_NAME] != null && (string)Session[FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME])) {
                    System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME]);
                }
            }

            Session[FILE_RANDOM_NAME] = IMAGEN_SIN_FOTO;
            return Json(null);
        }


        [HttpPost]
        public JsonResult Upload() {
            string fileName = "";
            string fileRandomName = "";

            if (Request.Files.Count > 0) {

                //for (int i = 0; i < Request.Files.Count; i++) {

                int i = 0; //solo considero el primero, si son varios, esta logica no me sirve

                //ir borrando el subido anteriormente para que no vayan quedando si los hay
                if (Session[FILE_RANDOM_NAME] != null && (string)Session[FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                    if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME])) {
                        System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[FILE_RANDOM_NAME]);
                    }
                }

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

                //Achicar el tamanio de la foto a 250 x 250:
                Bitmap image1 = (Bitmap)Image.FromFile(Server.MapPath(PHOTO_FOLDER_TMP) + fileRandomName, true);
                Bitmap image2 = (Bitmap)ImageUtils.CambiarTamanio(image1, MAX_ANCHO_FOTO, 0, 0);
                image2.Save(Server.MapPath(PHOTO_FOLDER_TMP) + fileRandomName);


            } else {
                fileRandomName = null;
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

        //private void moveAndRenamePhotoFile(string filePhotoName, string newPhotoFileName) {
        //    string sourceFile = Server.MapPath(PHOTO_FOLDER_TMP) + filePhotoName;
        //    string destinationFile = Server.MapPath(PHOTO_FOLDER) + newPhotoFileName;
        //    if (System.IO.File.Exists(destinationFile)){
        //        System.IO.File.Delete(destinationFile);
        //    }
        //    System.IO.File.Move(sourceFile, destinationFile);
        //}

        //private void renamePhotoFileName(string filePhotoName, string newPhotoFileName) {
        //    string sourceFile = Server.MapPath(PHOTO_FOLDER) + filePhotoName;
        //    string destinationFile = Server.MapPath(PHOTO_FOLDER) + newPhotoFileName;
        //    if (System.IO.File.Exists(destinationFile)) {
        //        System.IO.File.Delete(destinationFile);
        //    }
        //    System.IO.File.Move(sourceFile, destinationFile);
        //}

        //private void deletePhotoFile(string photoFileName) {
        //    if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + photoFileName)) {
        //        System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + photoFileName);
        //    }
        //}

        //-----------------------------------------------------------------------------------------------------

    }
}
