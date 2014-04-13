using AutomotoraWeb.Models;
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

        //--------------------------------------------------------------------------------

        public static MvcHtmlString BotonImagen(this HtmlHelper helper,  string accion, string controlador, object parametros, string clase, string tooltip) {
            //genera una div dentro de un anchor con un estilo que le pone la imagen de fondo


            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            String url = urlHelper.Action(accion, controlador, parametros);    

            //crear la div que va dentro del link anchor
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttribute("class", clase);
            tagBuilder.MergeAttribute("title", tooltip);
            string sdiv = tagBuilder.ToString(TagRenderMode.Normal);

            StringBuilder html=new StringBuilder();
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

        public static IEnumerable<SelectListItem> getListSelectListItemCustomerMaritalStatus(){
            Type type = typeof(CustomerModel.CustomerMaritalStatus);
            System.Reflection.PropertyInfo[] propertiesinfo = type.GetProperties();

            List<SelectListItem> listSelectListItemCustomerMaritalStatus = new List<SelectListItem>();

            foreach (string value in Enum.GetNames(type))
            {
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