using AutomotoraWeb.Models;
using AutomotoraWeb.Services;
using DLL_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AutomotoraWeb.Controllers {
    public class AuthenticationController : Controller {

        public static string AUTHENTICATION = "authentication";
        public static string LOGIN = "login";
        public static string CHANGE_PASSWORD = "changePassword";

        public static string SESSION_USER_NAME = "userName";

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
                    if (SecurityManager.Instance.login(model.UserName, model.Password, Request.UserHostAddress)) {

                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        Session[SESSION_USER_NAME] = model.UserName;

                        if (!String.IsNullOrEmpty(returnUrl)) {
                            return Redirect(returnUrl);
                        } else {
                            return RedirectToAction(HomeController.INDEX, HomeController.HOME);
                        }
                    }
                } catch (UsuarioException exc) {
                    model.ErrorCode = "U001";
                    model.ErrorMessage = exc.Message;
                    return View(LOGIN, model);
                } catch (Exception exc) {
                    model.ErrorCode = "U002";
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
        public ActionResult ChangePassword(ChangePasswordModel model, string returnUrl) {
            if (ModelState.IsValid) {
                try {
                    string userName = (string)(Session[SESSION_USER_NAME]);
                    SecurityManager.Instance.changePassword(userName, model.ActualPassword, model.NewPassword, model.RepeatNewPassword);

                    if (!String.IsNullOrEmpty(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        return RedirectToAction(HomeController.INDEX, HomeController.HOME);
                    }
                } catch (UsuarioException exc) {
                    model.ErrorCode = "U001";
                    model.ErrorMessage = exc.Message;
                    return View(CHANGE_PASSWORD, model);
                } catch (Exception exc) {
                    model.ErrorCode = "U002";
                    model.ErrorMessage = exc.Message;
                    return View(CHANGE_PASSWORD, model);
                }
            }

            return View(model);
        }


        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult ForgetPassword() {
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            Session.Remove(SESSION_USER_NAME);
            return View(LOGIN);
        }

        //------------------------------------------------------------------------------------------------------------------------

        //
        // POST: /Authentication/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

    }
}
