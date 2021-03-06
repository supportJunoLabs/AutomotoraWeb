﻿using AutomotoraWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DLL_Backend;

namespace AutomotoraWeb.Helpers {
    public static class HtmlHelpers {

        //--------------------------------------------------------------------------------



        public static MvcHtmlString Truncate(this HtmlHelper helper, string input, int length) {

            if (input.Length <= length) {
                return new MvcHtmlString(input);
            } else {
                return new MvcHtmlString(input.Substring(0, length) + "...");
            }
        }

        //---------- Para decidir entre comandos escribibles o de consulta en los mantenimientos que usan la misma partial view

        public static MvcHtmlString EditorOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura, object htmlAttributes = null) {

            if (!(soloLectura ?? false)) {
                return htmlHelper.EditorFor(expression, htmlAttributes);
            } else {

                return MvcHtmlString.Create(
                    htmlHelper.DisplayFor(expression, htmlAttributes).ToString() +
                    htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
                );
            }
        }


        public static MvcHtmlString DdlOrDisplayFor<TModel, TProperty, TProperty1>(this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression_ddl, bool? soloLectura,
           Expression<Func<TModel, TProperty1>> expression_label,
           IEnumerable<SelectListItem> selectList,
            string optionLabel = null,
            object htmlAttributes = null) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.DropDownListFor(expression_ddl, selectList, optionLabel, htmlAttributes);
            } else {
                return MvcHtmlString.Create(
                     htmlHelper.DisplayFor(expression_label, htmlAttributes).ToString() +
                      htmlHelper.HiddenFor(expression_ddl, htmlAttributes).ToString()
                );
            }
        }


        public static MvcHtmlString TextAreaOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
            object htmlAttributes = null) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextAreaFor(expression, htmlAttributes);
            } else {
                return MvcHtmlString.Create(
                     htmlHelper.DisplayFor(expression, htmlAttributes).ToString() +
                      htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
                );
            }
        }


        public static MvcHtmlString CheckBoxOrDisplayFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, bool? soloLectura, object htmlAttributes = null) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.CheckBoxFor(expression, htmlAttributes);
            } else {
                return MvcHtmlString.Create(
                     htmlHelper.DisplayFor(expression, htmlAttributes).ToString() +
                     htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
                 );

                //bool valor = false;
                //if (expression != null) {
                //    try {
                //        Func<TModel, bool> deleg = expression.Compile();
                //        var result = deleg(htmlHelper.ViewData.Model);
                //        valor = (bool)result;
                //    } catch (NullReferenceException) { }
                //}
                //if (valor) {
                //    return MvcHtmlString.Create("SI" + htmlHelper.HiddenFor(expression, htmlAttributes).ToString());
                //} else {
                //    return MvcHtmlString.Create("NO" + htmlHelper.HiddenFor(expression, htmlAttributes).ToString());
                //}
            }
        }

        //public static MvcHtmlString RadioButtonForOrDisplayFor<TModel>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, bool>> expression, bool? soloLectura, String valueSelected, object htmlAttributes = null) {
        //    if (!(soloLectura ?? false)) {
        //        return htmlHelper.RadioButtonFor(expression, valueSelected, htmlAttributes);
        //    } else {
        //        return MvcHtmlString.Create(
        //             htmlHelper.RadioButtonFor(expression, valueSelected, htmlAttributes).ToString() +
        //             htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
        //         );
        //    }
        //}

        public static MvcHtmlString TextBoxOrdisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
            string format, object htmlAttributes) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextBoxFor(expression, format, htmlAttributes);
            } else {
                return MvcHtmlString.Create(
                    htmlHelper.DisplayFor(expression, format, htmlAttributes).ToString() +
                    htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
                );
            }
        }

        public static MvcHtmlString TextBoxOrdisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
            object htmlAttributes) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextBoxFor(expression, htmlAttributes);
            } else {
                return MvcHtmlString.Create(
                   htmlHelper.DisplayFor(expression, htmlAttributes).ToString() +
                   htmlHelper.HiddenFor(expression, htmlAttributes).ToString()
               );
            }
        }


        //--------------------------------------------------------------------------------


        public static MvcHtmlString BotonTexto(this HtmlHelper helper, string accion, string controlador, object parametros, string clase, string tooltip, string texto) {
            //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo
            //se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro.


            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            String url = urlHelper.Action(accion, controlador, parametros);

            //crear la div que va dentro del link anchor
            TagBuilder tagBuilder = new TagBuilder("a");
            if (!string.IsNullOrWhiteSpace(clase)) {
                tagBuilder.MergeAttribute("class", clase);
            }
            if (!string.IsNullOrWhiteSpace(tooltip)) {
                tagBuilder.MergeAttribute("title", tooltip);
            }
            tagBuilder.InnerHtml = texto;
            tagBuilder.MergeAttribute("href", url);
            string sa = tagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(sa);
        }


        public static MvcHtmlString BotonImagen(this HtmlHelper helper, string accion, string controlador, object parametros, string clase, string tooltip) {
            //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo
            //se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro.


            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            String url = urlHelper.Action(accion, controlador, parametros);

            //crear la div que va dentro del link anchor
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttribute("class", clase);
            tagBuilder.MergeAttribute("title", tooltip);
            string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

            StringBuilder html = new StringBuilder();
            html.Append("<a href=\"");
            html.Append(url);
            html.Append("\">");
            html.Append(sdiv);
            html.Append("</a>");

            return MvcHtmlString.Create(html.ToString());
        }
        //--------------------------------------------------------------------------------

        //public static MvcHtmlString BotonAjaxImagen(this HtmlHelper helper, string accion, string controlador, int id, string clase, string tooltip) {
        //    //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo
        //    //se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro (vía ajax).


        //    UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
        //    String url = urlHelper.Action(accion, controlador, id);

        //    //crear la div que va dentro del link anchor
        //    TagBuilder tagBuilder = new TagBuilder("div");
        //    tagBuilder.MergeAttribute("class", clase);
        //    tagBuilder.MergeAttribute("title", tooltip);
        //    tagBuilder.MergeAttribute("style", "cursor:pointer");
        //    tagBuilder.MergeAttribute("id", "btn_" + id + "_" + accion);
        //    string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

        //    StringBuilder html = new StringBuilder();
        //    html.Append(sdiv);

        //    return MvcHtmlString.Create(html.ToString());
        //}

        public static MvcHtmlString BotonAjaxImagen(this HtmlHelper helper, string accion, string controlador, object parametros, string clase, string tooltip) {
            //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo
            //se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro (vía ajax).

            //UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            //String url = urlHelper.Action(accion, controlador, parametros);

            //crear la div que va dentro del link anchor
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttribute("class", clase);
            tagBuilder.MergeAttribute("title", tooltip);
            tagBuilder.MergeAttribute("style", "cursor:pointer");
            string sid = parametros.GetType().GetProperty("id").GetValue(parametros, null).ToString();
            tagBuilder.MergeAttribute("id", "btn_" + sid + "_" + accion);
            string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

            StringBuilder html = new StringBuilder();
            html.Append(sdiv);

            return MvcHtmlString.Create(html.ToString());
        }

        //--------------------------------------------------------------------------------

        public static MvcHtmlString BotonAjaxTexto(this HtmlHelper helper, string accion, string controlador, object parametros, string clase, string tooltip, string texto) {
            //genera una div con un texto tipo link
            //se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro (vía ajax).

            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            String url = urlHelper.Action(accion, controlador, parametros);

            //crear la div que va dentro del link anchor
            TagBuilder tagBuilder = new TagBuilder("div");
            if (!string.IsNullOrWhiteSpace(clase)){
                tagBuilder.MergeAttribute("class", clase);
            }
            if(!string.IsNullOrWhiteSpace(tooltip)){
                tagBuilder.MergeAttribute("title", tooltip);
            }
            tagBuilder.InnerHtml = texto;
            tagBuilder.MergeAttribute("style", "cursor:pointer");
            string sid = parametros.GetType().GetProperty("id").GetValue(parametros, null).ToString();
            tagBuilder.MergeAttribute("id", "btn_" + sid + "_" + accion);

            string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

            StringBuilder html = new StringBuilder();
            html.Append(sdiv);

            return MvcHtmlString.Create(html.ToString());
        }


        //public static MvcHtmlString BotonAjaxTexto(this HtmlHelper helper, string accion, string controlador, int id, string clase, string tooltip, string texto) {
        ////    genera una div dentro de un anchor con un estilo que le pone la imagen de fondo
        ////    se usa por ejemplo en las grillas devexpress de los mantenimientos para consulta, eliminar, modificar en cada registro (vía ajax).


        //    UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
        //    String url = urlHelper.Action(accion, controlador, id);

        //    crear la div que va dentro del link anchor
        //    TagBuilder tagBuilder = new TagBuilder("div");
        //    if (!string.IsNullOrWhiteSpace(clase)) {
        //        tagBuilder.MergeAttribute("class", clase);
        //    }
        //    if (!string.IsNullOrWhiteSpace(tooltip)) {
        //        tagBuilder.MergeAttribute("title", tooltip);
        //    }
        //    tagBuilder.InnerHtml = texto;
        //    tagBuilder.MergeAttribute("style", "cursor:pointer");
        //    tagBuilder.MergeAttribute("id", "btn_" + id + "_" + accion);
        //    string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

        //    StringBuilder html = new StringBuilder();
        //    html.Append(sdiv);

        //    return MvcHtmlString.Create(html.ToString());
        //}

        

        //--------------------------------------------------------------------------------

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = "") {
            return LabelHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression), labelText);
        }


        private static MvcHtmlString LabelHelper(HtmlHelper html,
            ModelMetadata metadata, string htmlFieldName, string labelText) {
            if (string.IsNullOrEmpty(labelText)) {
                labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            }

            if (string.IsNullOrEmpty(labelText)) {
                return MvcHtmlString.Empty;
            }


            bool isRequired = false;

            if (metadata.ModelType.Equals(true.GetType())) {//para los boolean que van con checkbox no le pongo * nunca
                isRequired = false;
            } else {
                isRequired = metadata.IsRequired;
            }

            //if ((metadata.ModelType.IsValueType && metadata.ModelType.GetCustomAttributes(typeof(RequiredAttribute), true).Any()) ||
            //      (!metadata.ModelType.IsValueType && metadata.IsRequired)) {
            //        isRequired = true;
            //}

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add(
                "for",
                TagBuilder.CreateSanitizedId(
                    html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)
                )
            );

            //tag.Attributes.Add("class", "display-label");
            if (isRequired) {
                //label_required que estaba antes no existe
                //tag.Attributes.Add("style", "display: inline !important");
            }

            if (isRequired) {
                tag.SetInnerText(labelText + " *");
            } else {
                tag.SetInnerText(labelText);
            }

            var output = tag.ToString(TagRenderMode.Normal);


            //if (isRequired) {
            //    var asteriskTag = new TagBuilder("span");
            //    asteriskTag.Attributes.Add("class", "display-label");
            //    //asteriskTag.Attributes.Add("style", "color: red");
            //    asteriskTag.SetInnerText(" *");
            //    output += asteriskTag.ToString(TagRenderMode.Normal);
            //}
            return MvcHtmlString.Create(output);
        }

        public static MvcHtmlString DdlOrDisplayImporteFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, Importe>> expression,  bool? soloLectura, bool? monedaFija=false) {
                return htmlHelper.DdlOrDisplayImporteFor(expression, null, soloLectura, monedaFija);
        }


        //Cuando esta en un segundo nivel, tengo que pasar el nombre de la property completa, porque el metodo solo me da el primer nivel
        //ej: para Transaccion.Importe no funciona si no le paso el string completo.
        public static MvcHtmlString DdlOrDisplayImporteFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, Importe>> expression, string nombreProperty,  bool? soloLectura, bool? monedaFija=false) {

            Importe imp = null;
            if (expression != null) {
                try {
                    Func<TModel, Importe> deleg = expression.Compile();
                    var result = deleg(htmlHelper.ViewData.Model);
                    imp = (Importe)result;
                } catch (NullReferenceException) { }
            }
            if (imp == null) {
                imp = new Importe(Moneda.MonedaDefault, 0);
            }
            string propertyName = nombreProperty;
            if (string.IsNullOrWhiteSpace(propertyName)){
                var data = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
                propertyName = data.PropertyName;
            }

            string controlName = propertyName.Replace(".", "");

            bool escritura = !(soloLectura ?? false);
            bool monFija = monedaFija ?? false;

            if (escritura) {
                string output = "";
                if (!monFija) {

                    var builder = new TagBuilder("select");
                    builder.Attributes["data-val"] = "true";
                    builder.Attributes["data-val-number"] = "El campo Codigo debe ser un número";
                    builder.Attributes["data-val-required"] = "El campo Codigo es obligatorio";
                    builder.Attributes["id"] = "ddl" + controlName + "Moneda";
                    builder.Attributes["name"] = propertyName + ".Moneda.Codigo";
                    builder.Attributes["style"] = "width: 150px";
                    builder.Attributes["class"] = "valid";

                    StringBuilder opciones = new StringBuilder();
                    foreach (Moneda m in Moneda.Monedas) {
                        TagBuilder optionBuilder = new TagBuilder("option");
                        optionBuilder.MergeAttribute("value", m.Codigo.ToString());
                        optionBuilder.InnerHtml = m.Nombre;
                        if (m.Codigo == imp.Moneda.Codigo) {
                            optionBuilder.MergeAttribute("selected", "selected");
                        }
                        opciones.Append(optionBuilder.ToString());
                        builder.InnerHtml = opciones.ToString();
                    }
                    output = builder.ToString(TagRenderMode.Normal);
                } else {
                    if (imp.Moneda != null && imp.Moneda.Nombre != null) {
                        output = imp.Moneda.Nombre;
                    }
                }

                var builder2 = new TagBuilder("input");
                builder2.Attributes["data-val"] = "true";
                builder2.Attributes["data-val-number"] = "El campo Monto debe ser un número";
                builder2.Attributes["data-val-required"] = "El importe es requerido";
                builder2.Attributes["id"] = "tx" + controlName + "Monto";
                builder2.Attributes["name"] = propertyName + ".Monto";
                builder2.Attributes["type"] = "text";
                builder2.Attributes["value"] = imp.Monto.ToString();
                builder2.Attributes["class"] = "alinearDerecha";
                builder2.Attributes["style"] = "margin-left: 10px";
                var output2 = builder2.ToString(TagRenderMode.Normal);

                return MvcHtmlString.Create(output + "  " + output2);
            } else {

                var builder3 = new TagBuilder("input");
                builder3.Attributes["data-val"] = "true";
                builder3.Attributes["id"] = "hd" + controlName + "Moneda";
                builder3.Attributes["name"] = propertyName + ".Moneda.Codigo";
                builder3.Attributes["type"] = "hidden";
                builder3.Attributes["value"] = imp.Moneda.Codigo.ToString();
                var output3 = builder3.ToString(TagRenderMode.Normal);

                var builder4 = new TagBuilder("input");
                builder4.Attributes["data-val"] = "true";
                builder4.Attributes["id"] = "hd" + controlName + "Monto";
                builder4.Attributes["name"] = propertyName + ".Monto";
                builder4.Attributes["type"] = "hidden";
                builder4.Attributes["value"] = imp.Monto.ToString();
                var output4 = builder4.ToString(TagRenderMode.Normal);

                return MvcHtmlString.Create(imp.ToString() + " " + output3 + " " + output4);
            }
        }



    }
}