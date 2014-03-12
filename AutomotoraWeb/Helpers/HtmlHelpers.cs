using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AutomotoraWeb.Helpers {
    public static class HtmlHelpers {
        public static MvcHtmlString Truncate(this HtmlHelper helper, string input, int length) {
            if (input.Length <= length) {
                return new MvcHtmlString(input);
            } else {
                return new MvcHtmlString(input.Substring(0, length) + "...");
            }
         }

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

    }
}