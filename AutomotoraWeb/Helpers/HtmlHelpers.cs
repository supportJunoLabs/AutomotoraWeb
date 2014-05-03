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

        //---------- Para decidir entre comandos escribibles o de consulta en los mantenimientos que usan la misma partial

        public static MvcHtmlString EditorOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura, object htmlAttributes = null) {

            if (!(soloLectura ?? false)) {
                return htmlHelper.EditorFor(expression, htmlAttributes);
            } else {
                return htmlHelper.DisplayFor(expression, htmlAttributes);
            }
        }

        public static MvcHtmlString DdlOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression_ddl, bool? soloLectura,
           Expression<Func<TModel, TProperty>> expression_label,
           IEnumerable<SelectListItem> selectList, string optionLabel = null, object htmlAttributes = null) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.DropDownListFor(expression_ddl, selectList, optionLabel, htmlAttributes);
            } else {
                return htmlHelper.DisplayFor(expression_label, htmlAttributes);
            }
        }

        public static MvcHtmlString DdlOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression_ddl, bool? soloLectura,
            Expression<Func<TModel, TProperty>> expression_label,
            IEnumerable<SelectListItem> selectList, object htmlAttributes) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.DropDownListFor(expression_ddl, selectList, htmlAttributes);
            } else {
                return htmlHelper.DisplayFor(expression_label, htmlAttributes);
            }
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
                object htmlAttributes = null) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextBoxFor(expression, htmlAttributes);
            } else {
                //por ahora, si es solo lectura pierde los atributos, o hay que usar el metodo con diccionario para poder agregar el readonly
                return htmlHelper.TextBoxFor(expression, new { @readonly = "true" });
            }
        }


        public static MvcHtmlString TextBoxOrdisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
            string format, object htmlAttributes) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextBoxFor(expression, format, htmlAttributes);
            } else {
                return htmlHelper.DisplayFor(expression, format, htmlAttributes);
            }
        }


        public static MvcHtmlString TextAreaOrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, bool? soloLectura,
            object htmlAttributes) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.TextAreaFor(expression, htmlAttributes);
            } else {
                return htmlHelper.DisplayFor(expression, htmlAttributes);
            }
        }


        public static MvcHtmlString CheckBoxOrDisplayFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, bool? soloLectura) {
            if (!(soloLectura ?? false)) {
                return htmlHelper.CheckBoxFor(expression);
            } else {
                bool valor = false;
                if (expression != null) {
                    try {
                        Func<TModel, bool> deleg = expression.Compile();
                        var result = deleg(htmlHelper.ViewData.Model);
                        valor = (bool)result;
                    } catch (NullReferenceException) { }
                }
                if (valor) {
                    return MvcHtmlString.Create("SI");
                } else {
                    return MvcHtmlString.Create("NO");
                }
            }
        }

        //--------------------------------------------------------------------------------

        public static MvcHtmlString BotonImagen(this HtmlHelper helper, string accion, string controlador, object parametros, string clase, string tooltip) {
            //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo


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

            if (metadata.ContainerType != null) {
                isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName)
                                .GetCustomAttributes(typeof(RequiredAttribute), false)
                                .Length == 1;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add(
                "for",
                TagBuilder.CreateSanitizedId(
                    html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)
                )
            );

            if (isRequired) {
                tag.Attributes.Add("class", "label-required");
                tag.Attributes.Add("style", "display: inline !important");
            }

            tag.SetInnerText(labelText);

            var output = tag.ToString(TagRenderMode.Normal);


            if (isRequired) {
                var asteriskTag = new TagBuilder("span");
                asteriskTag.Attributes.Add("class", "required");
                asteriskTag.Attributes.Add("style", "color: red");
                asteriskTag.SetInnerText("*");
                output += asteriskTag.ToString(TagRenderMode.Normal);
            }
            return MvcHtmlString.Create(output);
        }

        //--------------------------------------------------------------------------------

        public static IEnumerable<SelectListItem> getListSelectListItemCustomerMaritalStatus() {
            Type type = typeof(CustomerModel.CustomerMaritalStatus);
            System.Reflection.PropertyInfo[] propertiesinfo = type.GetProperties();

            List<SelectListItem> listSelectListItemCustomerMaritalStatus = new List<SelectListItem>();

            foreach (string value in Enum.GetNames(type)) {
                /// Get field info
                FieldInfo fi = type.GetField(value);

                /// Get description attribute
                object[] descriptionAttrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                //DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];

                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = value;//description.Description;
                selectListItem.Value = value;
                listSelectListItemCustomerMaritalStatus.Add(selectListItem);
            }

            return listSelectListItemCustomerMaritalStatus;
        }



    }
}