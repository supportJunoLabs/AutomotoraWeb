using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DLL_Backend;

/// <summary>
/// Summary description for DXReportConsultaFinanciacion
/// </summary>
public class DXReportConsultaFinanciacion : DevExpress.XtraReports.UI.XtraReport {
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private System.Windows.Forms.BindingSource bindingSource1;
    private XRLine xrLine1;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel52;
    private XRLabel xrLabel51;
    private XRLine xrLine2;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel14;
    private DetailReportBand DetailReport;
    private DetailBand Detail1;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel15;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLabel xrLabel27;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRLabel xrLabel16;
    private XRLabel xrLabel38;
    private XRLabel xrLabel37;
    private XRLabel xrLabel36;
    private XRLabel xrLabel35;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private DetailReportBand DetailReport1;
    private DetailBand Detail2;
    private XRLabel xrLabel39;
    private XRLabel xrLabel40;
    private XRLabel xrLabel41;
    private XRLabel xrLabel43;
    private XRLabel xrLabel42;
    private XRLabel xrLabel44;
    private DetailReportBand DetailReport2;
    private DetailBand Detail3;
    private XRLabel xrLabel48;
    private XRLabel xrLabel47;
    private XRLabel xrLabel46;
    private XRLabel xrLabel45;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public DXReportConsultaFinanciacion() {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport1 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport2 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail3 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2});
            this.Detail.HeightF = 181.3752F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 33.00002F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(81.25028F, 23F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.Text = "Cliente";
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 55.99998F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(81.25027F, 23F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "Fecha";
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 78.99996F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(81.25028F, 23F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.Text = "Precio";
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 102F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(81.25028F, 23F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.Text = "Financiacion";
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(373.9583F, 10.00001F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(69.79166F, 23F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Vehiculo";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 10.00001F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(81.25028F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "Operacion";
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.MontoFinanciado.ImporteTexto")});
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(91.24991F, 102F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(122.2917F, 23F);
            this.xrLabel13.Text = "xrLabel13";
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Importe.ImporteTexto")});
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(91.25001F, 78.99996F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(122.2917F, 23F);
            this.xrLabel12.Text = "xrLabel12";
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Observaciones")});
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(373.9583F, 125F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(254.1667F, 23F);
            this.xrLabel11.Text = "xrLabel11";
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Fecha", "{0:dd/MM/yyyy}")});
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(91.25001F, 55.99998F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(122.2917F, 23F);
            this.xrLabel10.Text = "xrLabel10";
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Observaciones")});
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 149.9583F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(618.125F, 23F);
            this.xrLabel9.Text = "xrLabel9";
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Matricula")});
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 102F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel8.Text = "xrLabel8";
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Anio")});
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 78.99996F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel7.Text = "xrLabel7";
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Modelo")});
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 55.99998F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Marca")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 32.99999F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Vehiculo.Ficha")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 10.00001F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cliente.Nombre")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(91.24991F, 32.99999F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(282.7084F, 23F);
            this.xrLabel3.Text = "xrLabel3";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Codigo")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(91.25001F, 10.00001F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(122.2917F, 23F);
            this.xrLabel2.Text = "xrLabel2";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel52,
            this.xrLabel51,
            this.xrLine1});
            this.TopMargin.HeightF = 60.41667F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel52
            // 
            this.xrLabel52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NombreSistemaActivo")});
            this.xrLabel52.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(291.0416F, 23F);
            this.xrLabel52.StylePriority.UseFont = false;
            // 
            // xrLabel51
            // 
            this.xrLabel51.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NombreEmpresaActiva")});
            this.xrLabel51.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(322.2917F, 10.00001F);
            this.xrLabel51.Name = "xrLabel51";
            this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel51.SizeF = new System.Drawing.SizeF(317.7083F, 23F);
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.StylePriority.UseTextAlignment = false;
            this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 47.49997F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(650.0002F, 2.083332F);
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 10.41667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(539.9998F, 28.08339F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.ReportHeader.HeightF = 36.45833F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-05F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(650.0001F, 22.99999F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "CONSULTA  FINANCIACION";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy HH:mm:ss}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 28.08339F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(154.1667F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(650.0002F, 4.083315F);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(DLL_Backend.Venta);
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2,
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.PageFooter.HeightF = 58.33333F;
            this.PageFooter.Name = "PageFooter";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.GroupHeader1});
            this.DetailReport.DataMember = "Financiacion.CuotasVigentes";
            this.DetailReport.DataSource = this.bindingSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel27,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel16});
            this.Detail1.HeightF = 23F;
            this.Detail1.Name = "Detail1";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel35,
            this.xrLabel34,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel15});
            this.GroupHeader1.HeightF = 59.37502F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(203.5416F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "FINANCIACION VIGENTE";
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.Numero")});
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(28.125F, 23F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.Text = "xrLabel16";
            // 
            // xrLabel22
            // 
            this.xrLabel22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.NumeroCuotaSetTexto")});
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(38.12501F, 0F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(29.16667F, 23F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = "xrLabel22";
            // 
            // xrLabel23
            // 
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.Importe.ImporteTexto")});
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(67.29167F, 0F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(96.87477F, 23F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "xrLabel23";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel24
            // 
            this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.Vencimiento", "{0:dd/MM/yy}")});
            this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(164.1664F, 0F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "xrLabel24";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel25
            // 
            this.xrLabel25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.ImporteCobrado.ImporteTexto")});
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(244.7918F, 0F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(76.04166F, 23F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "xrLabel25";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel26
            // 
            this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.FechaCobro", "{0:dd/MM/yy}")});
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(320.8334F, 0F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(78.125F, 23F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "xrLabel26";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel27
            // 
            this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.DescripcionEstado")});
            this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(398.9584F, 0F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(87.5F, 23F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.Text = "xrLabel27";
            // 
            // xrLabel28
            // 
            this.xrLabel28.CanGrow = false;
            this.xrLabel28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.Financista.Nombre")});
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(486.4583F, 0F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(73.33313F, 23F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.Text = "xrLabel28";
            // 
            // xrLabel29
            // 
            this.xrLabel29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.CuotasVigentes.CodigoExterno")});
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(566.6666F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(80.20828F, 23F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.Text = "xrLabel29";
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel30.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(9.999736F, 36.37502F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(28.12526F, 23F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseForeColor = false;
            this.xrLabel30.Text = "N°";
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel31.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(39.16642F, 36.37502F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(28.12526F, 23F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseForeColor = false;
            this.xrLabel31.Text = "N°";
            // 
            // xrLabel32
            // 
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel32.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(91.24991F, 32.99999F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(72.91653F, 23F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseForeColor = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Importe";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel33
            // 
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel33.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(164.1664F, 32.99999F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseForeColor = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "Vto";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel34.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(247.9169F, 32.99999F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(72.91653F, 23F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseForeColor = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "Pago";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel35.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(320.8334F, 32.99999F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseForeColor = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "Fecha";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel36.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(401.4587F, 32.99999F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseForeColor = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "Estado";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel37.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(566.6666F, 32.99999F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseForeColor = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "Cod Externo";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel38.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(482.084F, 32.99999F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(80.62532F, 23F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseForeColor = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = "Financista";
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetailReport1
            // 
            this.DetailReport1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2,
            this.DetailReport2});
            this.DetailReport1.DataMember = "Financiacion.HistoricoFinanciacion";
            this.DetailReport1.DataSource = this.bindingSource1;
            this.DetailReport1.Level = 1;
            this.DetailReport1.Name = "DetailReport1";
            // 
            // Detail2
            // 
            this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel41,
            this.xrLabel43,
            this.xrLabel42,
            this.xrLabel44});
            this.Detail2.HeightF = 73.95834F;
            this.Detail2.Name = "Detail2";
            // 
            // xrLabel39
            // 
            this.xrLabel39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Fecha", "{0:dd/MM/yyyy}")});
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.Text = "xrLabel39";
            // 
            // xrLabel40
            // 
            this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Observaciones")});
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(113.5417F, 10.00001F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(526.4581F, 23F);
            this.xrLabel40.Text = "xrLabel40";
            // 
            // DetailReport2
            // 
            this.DetailReport2.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail3});
            this.DetailReport2.DataMember = "Financiacion.HistoricoFinanciacion.Cuotas";
            this.DetailReport2.DataSource = this.bindingSource1;
            this.DetailReport2.Level = 0;
            this.DetailReport2.Name = "DetailReport2";
            // 
            // Detail3
            // 
            this.Detail3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel48,
            this.xrLabel47,
            this.xrLabel46,
            this.xrLabel45});
            this.Detail3.HeightF = 23F;
            this.Detail3.Name = "Detail3";
            // 
            // xrLabel41
            // 
            this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel41.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 44.37497F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(28.12526F, 23F);
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseForeColor = false;
            this.xrLabel41.Text = "N°";
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel42.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(39.16667F, 44.37497F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(28.12526F, 23F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseForeColor = false;
            this.xrLabel42.Text = "N°";
            // 
            // xrLabel43
            // 
            this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel43.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(91.25017F, 44.37497F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(72.91653F, 23F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseForeColor = false;
            this.xrLabel43.StylePriority.UseTextAlignment = false;
            this.xrLabel43.Text = "Importe";
            this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel44
            // 
            this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel44.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(167.292F, 44.37497F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.StylePriority.UseForeColor = false;
            this.xrLabel44.StylePriority.UseTextAlignment = false;
            this.xrLabel44.Text = "Vto";
            this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel45
            // 
            this.xrLabel45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Cuotas.Numero")});
            this.xrLabel45.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(28.125F, 23F);
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.Text = "xrLabel45";
            // 
            // xrLabel46
            // 
            this.xrLabel46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Cuotas.NumeroCuotaSet")});
            this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(39.16667F, 0F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(28.12527F, 23F);
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.Text = "xrLabel46";
            // 
            // xrLabel47
            // 
            this.xrLabel47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Cuotas.Importe.ImporteTexto")});
            this.xrLabel47.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(67.29195F, 0F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.StylePriority.UseTextAlignment = false;
            this.xrLabel47.Text = "xrLabel47";
            this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel48
            // 
            this.xrLabel48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Financiacion.HistoricoFinanciacion.Cuotas.Vencimiento", "{0:dd/MM/yy}")});
            this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(167.292F, 0F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel48.StylePriority.UseFont = false;
            this.xrLabel48.StylePriority.UseTextAlignment = false;
            this.xrLabel48.Text = "xrLabel48";
            this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // DXReportConsultaFinanciacion
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.DetailReport,
            this.DetailReport1});
            this.DataSource = this.bindingSource1;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 60, 10);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion


}
