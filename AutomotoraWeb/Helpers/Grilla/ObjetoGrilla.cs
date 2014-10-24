using AutomotoraWeb.Controllers.General;
using AutomotoraWeb.Helpers;
using AutomotoraWeb.Models;
using AutomotoraWeb.Utils;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using DevExpress.Web.ASPxEditors;



namespace AutomotoraWeb.Helpers.Grilla {
    public class ObjetoGrilla {
        public List<MVCxGridViewColumn> listGridViewsColumn = new List<MVCxGridViewColumn>();
        public MVCxGridViewColumn listGridViewsBotones = null;
        public MVCxGridViewColumn listGridViewsBotones2 = null;
        public List<string> totalesSuma = new List<string>();
        private bool defineAnchos = false; //alguna de las columnas define su ancho?
        private InfoGrilla ig;
        private HtmlHelper<object> Html;
        private UrlHelper Url;
        private ViewContext ViewContext;

        public void inicializarObjetoGrilla(InfoGrilla pIG, ViewContext pViewContext, HtmlHelper<object> pHtml, UrlHelper pUrl) {
            ig = pIG;
            Html = pHtml;
            Url = pUrl;
            ViewContext = pViewContext;

            //if (ig.TrunkColumns == null) {
            //    ig.TrunkColumns = new Dictionary<string, int>();
            //}

            if (ig.DobleClick) {
                if (string.IsNullOrWhiteSpace(ig.ControladorDobleClick)) {
                    ig.ControladorDobleClick = ig.Controller;
                }
                if (string.IsNullOrWhiteSpace(ig.AccionDobleClick)) {
                    ig.AccionDobleClick = BaseController.DETAILS;
                }
            }

            this.generarColumnasReflection(); //si no vinieron columnas las busca por reflection y las carga en lista columnas ig.VisibleColumns
            this.generarColumnasContenido();  //carga columnas generadas en lista columnas listGridViewsColumn
            this.generarBotonesGrupo1(); //carga los botones necearios del grupo 1 en listGridViewsBotones
            this.generarBotonesGrupo2(); //carga los botones necearios del grupo 2 en listGridViewsBotones2
        }

        private void generarColumnasReflection() {
            //Si no vienen definidas las columnas, las busco por reflection y las agrego manualmente a la lista de columnas que vino nula o vacia
            if (ig.VisibleColumns == null || ig.VisibleColumns.Count == 0) {
                ig.VisibleColumns = new List<ColumnaGrilla>();

                //obtengo los campos por reflection
                PropertyInfo[] propertiesinfo = ig.TypeOfModel.GetProperties();
                foreach (PropertyInfo propertyInfo in propertiesinfo) {
                    if (ig.HiddenColumns == null || !ig.HiddenColumns.Contains(propertyInfo.Name)) {
                        ColumnaGrilla cg = new ColumnaGrilla();
                        cg.Campo = propertyInfo.Name;
                        ig.VisibleColumns.Add(cg);
                    }
                }
            }
        }

        private void generarColumnasContenido() {
            defineAnchos = ig.anchosDefinidos();
            System.Reflection.PropertyInfo[] propertiesinfo = ig.TypeOfModel.GetProperties();
            int col=0;
            foreach (ColumnaGrilla cg in ig.VisibleColumns) {
                //Ver que no haya venido como oculta
                if (ig.HiddenColumns != null && ig.HiddenColumns.Contains(cg.Campo)) {
                    continue; //seguir con la proxima porque esta no va
                }
                col++;
                //Buscar la propiedad asociada para obtener sus datos:
                DisplayAttribute displayAttribute = null;
                PropertyInfo propertyInfo = null;
                foreach (var x in propertiesinfo) {
                    if (x.Name == cg.Campo) {
                        propertyInfo = x;
                        break; //salir del foreach
                    }
                }

                if (propertyInfo != null) {
                    //Acceder al data annotation de la columna
                    object[] arrayDisplayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true);

                    if (arrayDisplayAttribute != null && arrayDisplayAttribute.Length > 0) {
                        displayAttribute = (DisplayAttribute)arrayDisplayAttribute[0];
                    }
                }

                MVCxGridViewColumn gridViewColumn = new MVCxGridViewColumn();
                gridViewColumn.FieldName = cg.Campo;

                //Ver si hay que mostrar total
                if (cg.TotalSuma) {
                    totalesSuma.Add(cg.Campo);
                }

                //Titulo de la columna
                if (!string.IsNullOrWhiteSpace(cg.Titulo)) {
                    gridViewColumn.Caption = cg.Titulo;
                } else {
                    if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name)) {
                        gridViewColumn.Caption = displayAttribute.Name;
                    } else {
                        gridViewColumn.Caption = cg.Campo;
                    }
                }

                //Ancho
                if (defineAnchos) {
                    if (cg.Ancho > 0) {
                        gridViewColumn.Width = Unit.Pixel(cg.Ancho);
                    } else {
                        gridViewColumn.Width = Unit.Pixel(100);
                    }
                } else {
                    if (GeneralUtils.isBoolean(propertyInfo, cg)) {
                        gridViewColumn.Width = Unit.Pixel(75);
                    } else if (GeneralUtils.isDateTime(propertyInfo, cg)) {
                        gridViewColumn.Width = Unit.Pixel(100);
                    }
                }

                //Alineacion
                if (cg.Alineacion != ColumnaGrilla.ALINEACIONES.DEFAULT) {
                    switch (cg.Alineacion) {
                        case ColumnaGrilla.ALINEACIONES.IZQUIERDA:
                            gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                            break;
                        case ColumnaGrilla.ALINEACIONES.DERECHA:
                            gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            break;
                        case ColumnaGrilla.ALINEACIONES.CENTRO:
                            gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            break;
                        case ColumnaGrilla.ALINEACIONES.JUSTIFICADO:
                            gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Justify;
                            break;
                    }
                } else { //Si no se especifica, usar la alineacion por defecto.
                    if (GeneralUtils.isDateTime(propertyInfo, cg)) {
                        gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (GeneralUtils.isImporte(propertyInfo, cg) || GeneralUtils.isInteger(propertyInfo, cg)) {
                        gridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    }
                }

                //mostrar solo en edicion
                if (cg.MostrarSoloEdicion) {
                    gridViewColumn.Visible = false;
                    gridViewColumn.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
                }
                if (cg.MostrarSoloGrilla) {
                    gridViewColumn.Visible = true;
                    gridViewColumn.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                }

                gridViewColumn.EditFormSettings.VisibleIndex = col;
                if (cg.EdicionColSpan > 0) {
                    gridViewColumn.EditFormSettings.ColumnSpan = cg.EdicionColSpan;
                }

                //booleano              1
                if (GeneralUtils.isBoolean(propertyInfo, cg)) {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.CheckBox;

                } else {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.Default;
                }

                if (cg.ReadOnly) {
                    gridViewColumn.ReadOnly = true;
                }

                // si es fecha
                if (GeneralUtils.isDateTime(propertyInfo, cg)) {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.DateEdit;
                    gridViewColumn.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
                    var pcol = gridViewColumn.PropertiesEdit as DateEditProperties;
                    //pcol.NullText = "dd/MM/yyyy";
                    pcol.UseMaskBehavior=true;
                    pcol.EditFormat = EditFormat.Custom;
                }

                if (cg.Validacion == ColumnaGrilla.VALIDACIONES.IMPORTE) {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    var pcol = gridViewColumn.PropertiesEdit as SpinEditProperties;
                    pcol.NumberType = SpinEditNumberType.Float;
                    pcol.DecimalPlaces = 2;
                    pcol.NumberFormat = SpinEditNumberFormat.Custom;
                    pcol.DisplayFormatString = "F";
                    pcol.DisplayFormatInEditMode = true;
                    pcol.SpinButtons.ShowIncrementButtons = false;
                    pcol.SpinButtons.ShowLargeIncrementButtons = false;
                }

                if (cg.Validacion == ColumnaGrilla.VALIDACIONES.ENTERO) {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    var pcol = gridViewColumn.PropertiesEdit as SpinEditProperties;
                    pcol.NumberType = SpinEditNumberType.Integer;
                    pcol.SpinButtons.ShowIncrementButtons = true;
                    pcol.SpinButtons.ShowLargeIncrementButtons = false;
                }

                if (cg.ColumnType == MVCxGridViewColumnType.ComboBox) {
                    gridViewColumn.ColumnType = MVCxGridViewColumnType.ComboBox;
                    
                    var comboBoxProperties = gridViewColumn.PropertiesEdit as ComboBoxProperties;
                    comboBoxProperties.DataSource = cg.ComboBoxPropertiesDataSource;
                    comboBoxProperties.TextField = cg.ComboBoxPropertiesTextField;
                    comboBoxProperties.ValueField = cg.ComboBoxPropertiesValueField;
                    comboBoxProperties.ValueType = cg.ComboBoxPropertiesValueType;
                }

                if (!string.IsNullOrWhiteSpace(cg.Formato)) {
                    gridViewColumn.PropertiesEdit.DisplayFormatString = cg.Formato;
                }

                if (!GeneralUtils.isBoolean(propertyInfo, cg)) {
                    if (cg.Hipervinculo) {
                        gridViewColumn.SetDataItemTemplateContent(container => {
                            Html.DevExpress().HyperLink(hyperlink => {
                                //var visibleIndex = container.VisibleIndex;
                                //var keyValue = container.KeyValue;
                                var texto = DataBinder.Eval(container.DataItem, cg.Campo);
                                string tooltip = null;
                                if (GeneralUtils.isDateTime(propertyInfo, cg)) {
                                    if (texto != null) {
                                        texto = ((DateTime)DataBinder.Eval(container.DataItem, cg.Campo)).ToString("dd/MM/yyyy");
                                    }
                                } else {
                                    if (texto != null && cg.LargoMax > 0 && texto.ToString().Length > cg.LargoMax) {
                                        tooltip = texto.ToString();
                                        texto = texto.ToString().Substring(0, cg.LargoMax) + "...";
                                    }
                                }
                                var pardestino = DataBinder.Eval(container.DataItem, cg.HCampoParametro);
                                if (texto != null) {
                                    hyperlink.Properties.Text = texto.ToString();
                                }
                                hyperlink.NavigateUrl = Url.Action(cg.Haccion, cg.Hcontrolador, new { id = pardestino });
                                if (!string.IsNullOrWhiteSpace(tooltip)) {
                                    hyperlink.ToolTip = tooltip;
                                }
                            }).Render();
                        });
                    } else if (cg.LargoMax > 0) {
                        gridViewColumn.SetDataItemTemplateContent(container => {
                            Html.DevExpress().Label(label => {

                                string tooltip = null;
                                var texto = DataBinder.Eval(container.DataItem, cg.Campo);
                                if (GeneralUtils.isDateTime(propertyInfo, cg)) {
                                    if (texto != null) {
                                        texto = ((DateTime)DataBinder.Eval(container.DataItem, cg.Campo)).ToString("dd/MM/yyyy");
                                    }
                                } else {

                                    if (texto != null && cg.LargoMax > 0 && texto.ToString().Length > cg.LargoMax) {
                                        tooltip = texto.ToString();
                                        texto = texto.ToString().Substring(0, cg.LargoMax) + "...";
                                    }
                                }
                                if (texto != null) {
                                    label.Text = texto.ToString();
                                }
                                if (!string.IsNullOrWhiteSpace(tooltip)) {
                                    label.ToolTip = tooltip;
                                }
                            }).Render();
                        });
                    }
                } //if not boolean

                listGridViewsColumn.Add(gridViewColumn);
            } //foreach
        }

        private void generarBotonesGrupo1() {
            if (ig.BotonesMtoEstandar) {  //si pide agregar los botones de mantenimiento estandar
                listGridViewsBotones = new MVCxGridViewColumn();
                listGridViewsBotones.Caption = "Acciones";
                listGridViewsBotones.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                listGridViewsBotones.Width = Unit.Pixel(90);

                if (!ig.BotonesMtoEstandarAjax) {
                    listGridViewsBotones.SetDataItemTemplateContent(c => {
                        var id = System.Web.UI.DataBinder.Eval(c.DataItem, (String)(ig.KeyFieldName));
                        ViewContext.Writer.WriteLine(Html.BotonImagen("details", ig.Controller, new { id = id }, "boton-consultar-mf", "Consultar").ToHtmlString());
                        ViewContext.Writer.WriteLine(Html.BotonImagen("edit", ig.Controller, new { id = id }, "boton-editar-mf", "Editar").ToHtmlString());
                        ViewContext.Writer.WriteLine(Html.BotonImagen("delete", ig.Controller, new { id = id }, "boton-eliminar-mf", "Eliminar").ToHtmlString());
                    });
                } else {
                    listGridViewsBotones.SetDataItemTemplateContent(c => {
                        var id = System.Web.UI.DataBinder.Eval(c.DataItem, (String)(ig.KeyFieldName));
                        ViewContext.Writer.WriteLine(Html.BotonAjaxImagen("details", "", new { id = id }, "boton-consultar-mf", "Consultar").ToHtmlString());
                        ViewContext.Writer.WriteLine(Html.BotonAjaxImagen("edit", "", new { id = id }, "boton-editar-mf", "Editar").ToHtmlString());
                        ViewContext.Writer.WriteLine(Html.BotonAjaxImagen("delete", "", new { id = id }, "boton-eliminar-mf", "Eliminar").ToHtmlString());
                    });
                }
            } else {
                //No se piden botones automaticos en grupo 1, agregar los que corresponda manualmente
                if (ig.Botones != null && ig.Botones.Count > 0) {
                    listGridViewsBotones = new MVCxGridViewColumn();
                    listGridViewsBotones.Caption = "Acciones";
                    listGridViewsBotones.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    int tot = 30 * ig.Botones.Count; //vienen botones, ver la cantidad
                    //listGridViewsBotones.Width = Unit.Pixel(90);
                    if (tot < 60) {
                        tot = 60;
                    }
                    listGridViewsBotones.Width = Unit.Pixel(tot);

                    listGridViewsBotones.SetDataItemTemplateContent(c => {
                        var id = System.Web.UI.DataBinder.Eval(c.DataItem, (String)(ig.KeyFieldName));
                        foreach (BotonGrilla b in ig.Botones) {
                            if (b is BotonAjaxGrilla) {
                                ViewContext.Writer.WriteLine(Html.BotonAjaxImagen(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip).ToHtmlString());
                            } else {
                                ViewContext.Writer.WriteLine(Html.BotonImagen(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip).ToHtmlString());
                            }
                        }
                    });
                }
            }
        }

        private void generarBotonesGrupo2() {
            if (ig.Botones2 != null && ig.Botones2.Count > 0) {  //grupo 2 son opcionales y pueden ser de tipo imagen o texto
                //Hay grupo de botones secundario
                listGridViewsBotones2 = new MVCxGridViewColumn();
                if (string.IsNullOrWhiteSpace(ig.TextoAcciones2)) {
                    listGridViewsBotones2.Caption = "Acciones";
                } else {
                    listGridViewsBotones2.Caption = ig.TextoAcciones2;
                }
                int tot = 0;
                foreach (var b in ig.Botones2) {
                    if (b.Ancho <= 0) {
                        tot += 30;
                    } else {
                        tot += b.Ancho;
                    }
                }
                if (tot < 90) {
                    tot = 90;
                }
                listGridViewsBotones2.Width = Unit.Pixel(tot); //calcular tamanio total de los botones (30 si vino vacio el ancho)
                listGridViewsBotones2.SetDataItemTemplateContent(c => {
                    var id = DataBinder.Eval(c.DataItem, (String)(ig.KeyFieldName));
                    foreach (BotonGrilla b in ig.Botones2) {
                        if (b is BotonAjaxGrilla) {
                            if (b.Tipo == BotonGrilla.TIPO.IMAGEN) {
                                ViewContext.Writer.WriteLine(Html.BotonAjaxImagen(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip).ToHtmlString());
                            } else {
                                ViewContext.Writer.WriteLine(Html.BotonAjaxTexto(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip, b.Texto).ToHtmlString());
                            }
                        } else {
                            if (b.Tipo == BotonGrilla.TIPO.IMAGEN) {
                                ViewContext.Writer.WriteLine(Html.BotonImagen(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip).ToHtmlString());
                            } else {
                                ViewContext.Writer.WriteLine(Html.BotonTexto(b.Accion, b.Controlador, new { id = id }, b.Clase, b.Tooltip, b.Texto).ToHtmlString());
                            }
                        }

                    }
                });
            }
        }

    }

}





