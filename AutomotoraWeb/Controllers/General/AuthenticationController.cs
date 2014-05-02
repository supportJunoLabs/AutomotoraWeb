using AutomotoraWeb.Controllers.Sistema;
using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using AutomotoraWeb.Utils;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AutomotoraWeb.Controllers.General {
    public class AuthenticationController : Controller {

        public static string CONTROLLER = "authentication";
        public static string LOGIN = "login";
        public static string LOGOUT = "logout";
        public static string CHANGE_PASSWORD = "changePassword";

        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Login() {
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl) {
            if (ModelState.IsValid) {
                try {
                    String IP = Request.UserHostAddress;
                    if (SecurityService.Instance.login(model.UserName, model.Password, IP)) {

                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        Session[SessionUtils.SESSION_USER_NAME] = model.UserName;

                        // Se obtienen opciones del menu
                        Session[SessionUtils.SESSION_MENU_OPTIONS] = MenuService.Instance.getMenuOptions(model.UserName, IP);

                        if (!String.IsNullOrEmpty(returnUrl)) {
                            return Redirect(returnUrl);
                        } else {
                            return RedirectToAction(SistemaController.INDEX, SistemaController.BCONTROLLER);
                        }
                    }
                } catch (UsuarioException exc) {
                    model.ErrorCode = exc.Codigo;
                    model.ErrorMessage = exc.Message;
                    return View(LOGIN, model);
                } 
            }

            return View(model);
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult ChangePassword() {
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)(Session[SessionUtils.SESSION_USER_NAME]);
                    SecurityService.Instance.changePassword(userName, model.ActualPassword, model.NewPassword, model.RepeatNewPassword, Request.UserHostAddress);
                    //return RedirectToAction(SistemaController.INDEX, SistemaController.BCONTROLLER);
                    Destino dest = BaseController.DestinoIndexUltimoModulo(Session[SessionUtils.ULTIMO_MODULO]);
                    return RedirectToAction(dest.Accion, dest.Controlador);
                } catch (UsuarioException exc) {
                    model.ErrorCode = exc.Codigo;
                    model.ErrorMessage = exc.Message;
                    return View(CHANGE_PASSWORD, model);
                }
            }

            return View(model);
        }


        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            Session.Remove(SessionUtils.SESSION_USER_NAME);
            return View(LOGIN);
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult CancelarCambioPwd() {
            Destino dest = BaseController.DestinoIndexUltimoModulo(Session[SessionUtils.ULTIMO_MODULO]);
            return RedirectToAction(dest.Accion, dest.Controlador);
        }
    }
}
