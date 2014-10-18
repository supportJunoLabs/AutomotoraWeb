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

namespace AutomotoraWeb.Controllers.Sales {
    public class VendedoresController : SalesController, IMaintenance {

        public static string CONTROLLER = "vendedores";


        public const string FILE_RANDOM_NAME = "fileRandomName";
        public const string ACTUAL_PHOTO_FILE_NAME = "actualPhotoFileName";
        public const string PHOTO_FOLDER_TMP = "~/Content/Images/tmp/";
        public const string PHOTO_FOLDER = "~/Content/Images/vendedores/";
        private const int MAX_ANCHO_FOTO = 250;
        public const string IMAGEN_SIN_FOTO = "_sinFoto.png";


        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            ViewBag.NombreEntidad = "Vendedor";
            ViewBag.NombreEntidades = "Vendedores";

        }

        public ActionResult Show([ModelBinder(typeof(DevExpressEditorsBinder))] Vendedor vendedor) {
            return View(_listaVendedores());
        }

        public ActionResult listVendedores() {
            return PartialView("_listVendedores", _listaVendedores());
        }


        //------------------------------------------------------------------------------------------

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Details(int id) {
            ViewBag.SoloLectura = true;
            try {
                string s = SessionUtils.generarIdVarSesion("MtoVendedores", getUserName()) + "|";
                Vendedor vend = getVendedor(id, true, s);
                VendedorModel mvend = new VendedorModel();
                mvend.Vendedor = vend;
                mvend.idsesion = s;
                return View(mvend);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Create() {
            VendedorModel v = new VendedorModel();
            string s = SessionUtils.generarIdVarSesion("MtoVendedores", getUserName()) + "|";
            v.idsesion = s;
            Session[s + FILE_RANDOM_NAME] = null;
            return View(v);
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Edit(int id) {
            string s = SessionUtils.generarIdVarSesion("MtoVendedores", getUserName()) + "|";
            Session[s + FILE_RANDOM_NAME] = null;
            try {
                Vendedor vend = getVendedor(id, false, s);
                VendedorModel mvend = new VendedorModel();
                mvend.Vendedor = vend;
                mvend.idsesion = s;
                return View(mvend);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

         [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Delete(int id) {
            ViewBag.SoloLectura = true;
            try {
                string s = SessionUtils.generarIdVarSesion("MtoVendedores", getUserName()) + "|";
                Vendedor vend = getVendedor(id, true, s);
                VendedorModel mvend = new VendedorModel();
                mvend.idsesion = s;
                mvend.Vendedor = vend;
                return View(mvend);
            } catch (UsuarioException exc) {
                ViewBag.ErrorCode = exc.Codigo;
                ViewBag.ErrorMessage = exc.Message;
                return View();
            }
        }

        public ActionResult Cancelar(string s, int id) {

            if (Session[s+FILE_RANDOM_NAME] != null && (string)Session[s+FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME])) {
                    System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME]);
                }
            }
            Session[s+FILE_RANDOM_NAME] = null;
            return RedirectToAction(BaseController.DETAILS, new { id = id });
        }

        //-----------------------------------------------------------------------------------------------------


        private Vendedor getVendedor(int id, bool fotosinimagen, string idsesion) {

            Vendedor vendedor = new Vendedor();
            vendedor.Codigo = id;
            vendedor.Consultar();

            if (fotosinimagen) {
                if (vendedor.Foto == null || vendedor.Foto.Trim() == "") {
                    vendedor.Foto = IMAGEN_SIN_FOTO;
                }
            }
            Session[idsesion + ACTUAL_PHOTO_FILE_NAME] = vendedor.Foto;
            return vendedor;
        }

        private List<Vendedor> _listaVendedores() {
            return Vendedor.Vendedores(Vendedor.VEND_TIPO_LISTADO.TODOS);
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Create(VendedorModel mvendedor) {

            if (ModelState.IsValid) {
                try {
                    string s = mvendedor.idsesion;
                    if (Session[s + FILE_RANDOM_NAME] != null && (string)Session[s+ FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                        string filePhotoName = (string)(Session[s+FILE_RANDOM_NAME]);
                        mvendedor.Vendedor.Foto = filePhotoName;
                    }

                    mvendedor.Vendedor.setearAuditoria(mvendedor.UserName, mvendedor.IP);
                    mvendedor.Vendedor.Agregar(); // Si hay foto le cambia el nombre

                    if (Session[s+FILE_RANDOM_NAME] != null && (string)Session[s+FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                        // Movemos del directorio temporario al de vendedores y se actualiza el nombre

                        if (Session[s+ACTUAL_PHOTO_FILE_NAME] != null) { //si antes habia se borra
                            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME])) {
                                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME]);
                            }
                        }
                        //el nuevo se mueve a la carpeta destiono
                        System.IO.File.Move((Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME]),
                                            Server.MapPath(PHOTO_FOLDER) + mvendedor.Vendedor.Foto);

                        Session[s+FILE_RANDOM_NAME] = null;
                    }

                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(mvendedor);
                }
            }

            return View(mvendedor);
        }

        //------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Edit(VendedorModel mvendedor) {

            if (ModelState.IsValid) {
                try {
                    String s = mvendedor.idsesion;
                    if (Session[s+FILE_RANDOM_NAME] != null) { //hice algun cambio
                        if ((string)(Session[s+FILE_RANDOM_NAME]) == IMAGEN_SIN_FOTO) {
                            mvendedor.Vendedor.Foto = null;  //borre la foto
                        } else {
                            mvendedor.Vendedor.Foto = (string)(Session[s+FILE_RANDOM_NAME]); //agrege una foto
                        }
                    } else {
                        //no hice ningun cambio,mantener lo anterior.
                        mvendedor.Vendedor.Foto = (string)Session[s+ACTUAL_PHOTO_FILE_NAME]; //a sea foto o null
                    }

                    mvendedor.Vendedor.setearAuditoria(mvendedor.UserName, mvendedor.IP);
                    mvendedor.Vendedor.ModificarDatos(); //me asigna el nuevo nombre de la foto

                    if (Session[s+FILE_RANDOM_NAME] == null) {
                        //No hice ningun cambio
                        return RedirectToAction(BaseController.SHOW);
                    }

                    if ((string)Session[s+FILE_RANDOM_NAME] == IMAGEN_SIN_FOTO) {
                        //borre la foto anterior si la habia, y no puse ninguna
                        if (Session[s+ACTUAL_PHOTO_FILE_NAME] != null) {
                            if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME])) {
                                System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME]);
                            }
                        }
                        return RedirectToAction(BaseController.SHOW);
                    }

                    //puse uno nuevo, debo pasarlo al dir destino y borrar el anterior si lo hay

                    if (Session[s+ACTUAL_PHOTO_FILE_NAME] != null) { //si antes habia se borra
                        if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME])) {
                            System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + Session[s+ACTUAL_PHOTO_FILE_NAME]);
                        }
                    }
                    //el nuevo se mueve a la carpeta destiono
                    System.IO.File.Move(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME],
                                        Server.MapPath(PHOTO_FOLDER) + mvendedor.Vendedor.Foto);


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
                    return View(mvendedor);
                }
            }

            return View(mvendedor);
        }



        [HttpPost]
        public ActionResult Delete(VendedorModel mvendedor) {
            ViewBag.SoloLectura = true;
            if (ModelState.IsValid) {
                try {
                    string s = mvendedor.idsesion;
                    string userName = getUserName();
                    string IP = getIP();
                    string photoFileName = mvendedor.Vendedor.Foto;
                    mvendedor.Vendedor.Eliminar(userName, IP);

                    if (!String.IsNullOrEmpty(photoFileName)) {
                        if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER) + photoFileName)) {
                            System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER) + photoFileName);
                        }
                    }

                    return RedirectToAction(BaseController.SHOW);
                } catch (UsuarioException exc) {
                    ViewBag.ErrorCode = exc.Codigo;
                    ViewBag.ErrorMessage = exc.Message;
                    return View(mvendedor);
                }
            }

            return View(mvendedor);
        }


        #region reporte

        //--------------------------------------    REPORT    ----------------------------------------------
        public ActionResult Report() {
            // Add a report to the view data. 
            //DXReportVendedores rep = new DXReportVendedores();
            ////setParamsToReport(rep);
            //ViewData["Report"] = rep;
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

        #endregion

        //-----------------------------------------------------------------------------------------------------

       
        #region foto
        [HttpPost]
        public JsonResult DeleteFoto(string s) {
            //FALTA MANDAR EL STRING SESION DESDE EL AJAX

            //Si ya se sugio alguna foto anterior al tmp, la voy borrando
            if (Session[s+FILE_RANDOM_NAME] != null && (string)Session[s+FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME])) {
                    System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME]);
                }
            }

            Session[s+FILE_RANDOM_NAME] = IMAGEN_SIN_FOTO;
            return Json(null);
        }


        [HttpPost]
        public JsonResult Upload(FormCollection col) {
            string fileName = "";
            string fileRandomName = "";

            if (Request.Files.Count > 0) {

                //for (int i = 0; i < Request.Files.Count; i++) {

                int i = 0; //solo considero el primero, si son varios, esta logica no me sirve

                string s = col["idsesion"];

                //ir borrando el subido anteriormente para que no vayan quedando si los hay
                if (Session[s + FILE_RANDOM_NAME] != null && (string)Session[s + FILE_RANDOM_NAME] != IMAGEN_SIN_FOTO) {
                    if (System.IO.File.Exists(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME])) {
                        System.IO.File.Delete(Server.MapPath(PHOTO_FOLDER_TMP) + Session[s+FILE_RANDOM_NAME]);
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
                fileRandomName = System.IO.Path.GetRandomFileName() + extension;
                file.SaveAs(Server.MapPath(PHOTO_FOLDER_TMP) + fileRandomName);
                Session[s + FILE_RANDOM_NAME] = fileRandomName;

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
            //se corrige porque no andaba bien cuando hay un punto como parte del nombre del archivo, ademas del que separa la extension. 
            //se debe tomar el ultimo punto
            string extension = "";
            if (fileName.Contains('.')) {
                int pos = fileName.LastIndexOf('.');
                extension = "." + fileName.Substring(pos);
            }
            return extension;
        }


        #endregion


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

        //#region SeleccionDeVendedor

        //[HttpPost]
        //public JsonResult details(int codigo) {
        //    try {
        //        Vendedor vendedor = new Vendedor();
        //        vendedor.Codigo = codigo;
        //        vendedor.Consultar();

        //        return Json(new {
        //            Result = "OK",
        //            Vendedor = new {
        //                Nombre = vendedor.Nombre,
        //                Cedula = vendedor.Mail,
        //                Telefono = vendedor.Telefono
        //            }
        //        }
        //        );
        //    } catch (UsuarioException exc) {
        //        return Json(new { Result = "ERROR", ErrorCode = exc.Codigo, ErrorMessage = exc.Message });
        //    }
        //}

        //#endregion

    }
}
