using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using DLL_Backend;

/// <summary>
/// Summary description for _DXSubrepPago
/// </summary>
public class _DXSubrepPago : DevExpress.XtraReports.UI.XtraReport {
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private System.Windows.Forms.BindingSource bindingSource1;
    private DetailReportBand DetailReportEfectivo;
    private DetailBand det_efectivo;
    private XRLabel xrLabel1;
    private GroupHeaderBand gh_efectivo;
    private XRLabel xrLabel2;
    private DetailReportBand DetailReportCheques;
    private DetailBand Detail2;
    private GroupHeaderBand GroupHeader2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel14;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private DetailReportBand DetailReportPagoBanco;
    private DetailBand Detail3;
    private GroupHeaderBand GroupHeader3;
    private XRLabel xrLabel20;
    private XRLabel xrLabel21;
    private XRLabel xrLabel22;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLabel xrLabel27;
    private XRLabel xrLabel26;
    private DetailReportBand DetailReportChequesEmitidos;
    private DetailBand Detail4;
    private GroupHeaderBand GroupHeader4;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel25;
    private XRLabel xrLabel41;
    private XRLabel xrLabel40;
    private XRLabel xrLabel42;
    private XRLabel xrLabel43;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRLabel xrLabel39;
    private ReportFooterBand ReportFooter;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public _DXSubrepPago() {
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.DetailReportEfectivo = new DevExpress.XtraReports.UI.DetailReportBand();
            this.det_efectivo = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.gh_efectivo = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReportCheques = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
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
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReportPagoBanco = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail3 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReportChequesEmitidos = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail4 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 2F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(DLL_Backend.PagoTransaccion);
            // 
            // DetailReportEfectivo
            // 
            this.DetailReportEfectivo.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.det_efectivo,
            this.gh_efectivo});
            this.DetailReportEfectivo.DataMember = "Efectivo";
            this.DetailReportEfectivo.DataSource = this.bindingSource1;
            this.DetailReportEfectivo.Level = 0;
            this.DetailReportEfectivo.Name = "DetailReportEfectivo";
            this.DetailReportEfectivo.ReportPrintOptions.PrintOnEmptyDataSource = false;
            //this.DetailReportEfectivo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReportEfectivo_BeforePrint);
            // 
            // det_efectivo
            // 
            this.det_efectivo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.det_efectivo.HeightF = 18.08332F;
            this.det_efectivo.Name = "det_efectivo";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Efectivo.ImporteTexto")});
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(12.08334F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(130.2084F, 16F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // gh_efectivo
            // 
            this.gh_efectivo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2});
            this.gh_efectivo.HeightF = 23.29167F;
            this.gh_efectivo.Name = "gh_efectivo";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 7.291667F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Efectivo:";
            // 
            // DetailReportCheques
            // 
            this.DetailReportCheques.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2,
            this.GroupHeader2});
            this.DetailReportCheques.DataMember = "Cheques";
            this.DetailReportCheques.DataSource = this.bindingSource1;
            this.DetailReportCheques.Level = 1;
            this.DetailReportCheques.Name = "DetailReportCheques";
            this.DetailReportCheques.ReportPrintOptions.PrintOnEmptyDataSource = false;
            //this.DetailReportCheques.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReportCheques_BeforePrint);
            // 
            // Detail2
            // 
            this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
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
            this.xrLabel4});
            this.Detail2.HeightF = 58.41662F;
            this.Detail2.Name = "Detail2";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 31.99997F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(55.625F, 16F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "Librador:";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 15.99997F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(55.625F, 16F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "Cuenta:";
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 0F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(55.625F, 16F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "Banco:";
            // 
            // xrLabel11
            // 
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.ImporteTexto")});
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(416.6668F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(117.0833F, 16F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.FechaVencimiento", "{0:dd/MM/yyyy}")});
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(350.625F, 0F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(66.04163F, 16F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel10";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.FechaValor", "{0:dd/MM/yyyy}")});
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(276.0417F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(74.58337F, 16F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "xrLabel9";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.Observaciones")});
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(176.0416F, 31.99997F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(357.7085F, 16F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "xrLabel8";
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.NumeroCheque")});
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(176.0416F, 0F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "xrLabel7";
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.Librador")});
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(80.20834F, 31.99997F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(95.83331F, 16F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.Cuenta")});
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(80.20834F, 15.99998F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(95.83331F, 16F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cheques.Banco")});
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(80.20834F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(95.83331F, 16F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel3});
            this.GroupHeader2.HeightF = 42.41667F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel19.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(433.7501F, 26.00002F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseForeColor = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Importe";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel18.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(350.6249F, 26.00002F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(66.04169F, 16F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseForeColor = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "Vto";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel17.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(276.0416F, 26.00002F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(74.58334F, 16F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseForeColor = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Emision";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel16.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(176.0416F, 26.00002F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseForeColor = false;
            this.xrLabel16.Text = "Numero";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel15.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(24.58337F, 26.00002F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseForeColor = false;
            this.xrLabel15.Text = "Banco/Cuenta";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(12.08334F, 10.00001F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Cheques:";
            // 
            // DetailReportPagoBanco
            // 
            this.DetailReportPagoBanco.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail3,
            this.GroupHeader3});
            this.DetailReportPagoBanco.DataMember = "PagosBanco";
            this.DetailReportPagoBanco.DataSource = this.bindingSource1;
            this.DetailReportPagoBanco.Level = 2;
            this.DetailReportPagoBanco.Name = "DetailReportPagoBanco";
            this.DetailReportPagoBanco.ReportPrintOptions.PrintOnEmptyDataSource = false;
            //this.DetailReportPagoBanco.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReportPagoBanco_BeforePrint);
            // 
            // Detail3
            // 
            this.Detail3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel27,
            this.xrLabel26});
            this.Detail3.HeightF = 20.16665F;
            this.Detail3.Name = "Detail3";
            // 
            // xrLabel29
            // 
            this.xrLabel29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PagosBanco.ImporteMov.ImporteTexto")});
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(288.5417F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "xrLabel29";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel28
            // 
            this.xrLabel28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PagosBanco.FechaMov", "{0:dd/MM/yyyy}")});
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(221.875F, 0F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(66.66663F, 16F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "xrLabel28";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel27
            // 
            this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PagosBanco.Cuenta.Numero")});
            this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(121.875F, 0F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.Text = "xrLabel27";
            // 
            // xrLabel26
            // 
            this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PagosBanco.Cuenta.Banco")});
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 0F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(97.29166F, 16F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.Text = "xrLabel26";
            // 
            // GroupHeader3
            // 
            this.GroupHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel20,
            this.xrLabel21,
            this.xrLabel22,
            this.xrLabel23,
            this.xrLabel25});
            this.GroupHeader3.HeightF = 44.33334F;
            this.GroupHeader3.Name = "GroupHeader3";
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 12.29168F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "Pagos Banco:";
            // 
            // xrLabel21
            // 
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel21.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 28.29166F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(97.29167F, 16F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseForeColor = false;
            this.xrLabel21.Text = "Banco";
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel22.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(121.875F, 28.33334F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseForeColor = false;
            this.xrLabel22.Text = "Cuenta";
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel23.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(221.875F, 28.29166F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(66.66663F, 16F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseForeColor = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Fecha";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel25.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(288.5417F, 28.29166F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseForeColor = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "Importe";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // DetailReportChequesEmitidos
            // 
            this.DetailReportChequesEmitidos.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail4,
            this.GroupHeader4,
            this.ReportFooter});
            this.DetailReportChequesEmitidos.DataMember = "ChequesEmitidos";
            this.DetailReportChequesEmitidos.DataSource = this.bindingSource1;
            this.DetailReportChequesEmitidos.Level = 3;
            this.DetailReportChequesEmitidos.Name = "DetailReportChequesEmitidos";
            this.DetailReportChequesEmitidos.ReportPrintOptions.PrintOnEmptyDataSource = false;
            //this.DetailReportChequesEmitidos.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReportChequesEmitidos_BeforePrint);
            // 
            // Detail4
            // 
            this.Detail4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel34,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel24});
            this.Detail4.HeightF = 19.12498F;
            this.Detail4.Name = "Detail4";
            // 
            // xrLabel34
            // 
            this.xrLabel34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.Importe.ImporteTexto")});
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(416.6668F, 0F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(127.4999F, 16F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "xrLabel34";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel33
            // 
            this.xrLabel33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.FechaVencimiento")});
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(350.625F, 0F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(66.04169F, 16F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "xrLabel33";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel32
            // 
            this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.FechaEmision")});
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(289.5834F, 0F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(61.0416F, 16F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "xrLabel32";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel31
            // 
            this.xrLabel31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.Numero")});
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(207.2917F, 0F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(82.2917F, 16F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.Text = "xrLabel31";
            // 
            // xrLabel30
            // 
            this.xrLabel30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.Cuenta.Numero")});
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(128.125F, 0F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(79.16667F, 16F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.Text = "xrLabel30";
            // 
            // xrLabel24
            // 
            this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChequesEmitidos.Cuenta.Banco")});
            this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 0F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.Text = "xrLabel24";
            // 
            // GroupHeader4
            // 
            this.GroupHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel41,
            this.xrLabel40,
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel39});
            this.GroupHeader4.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.GroupHeader4.HeightF = 46.62498F;
            this.GroupHeader4.Name = "GroupHeader4";
            this.GroupHeader4.StylePriority.UseFont = false;
            // 
            // xrLabel41
            // 
            this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel41.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(446.25F, 30.62498F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseForeColor = false;
            this.xrLabel41.StylePriority.UseTextAlignment = false;
            this.xrLabel41.Text = "Importe";
            this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel40
            // 
            this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel40.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(291.6667F, 30.62495F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(61.04163F, 16F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.StylePriority.UseForeColor = false;
            this.xrLabel40.StylePriority.UseTextAlignment = false;
            this.xrLabel40.Text = "Emision";
            this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel42.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(352.7084F, 30.62495F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(66.04163F, 16F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseForeColor = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.Text = "Vto";
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel43
            // 
            this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel43.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(209.375F, 30.62495F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(82.2917F, 16F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseForeColor = false;
            this.xrLabel43.Text = "Numero";
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel37.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(130.2083F, 30.62495F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(79.16667F, 16F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseForeColor = false;
            this.xrLabel37.Text = "Cuenta";
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel38.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(24.58334F, 30.62498F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseForeColor = false;
            this.xrLabel38.Text = "Banco";
            // 
            // xrLabel39
            // 
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 14.625F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(132.2917F, 16F);
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.Text = "Cheques Emitidos:";
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 8.333333F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // _DXSubrepPago
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReportEfectivo,
            this.DetailReportCheques,
            this.DetailReportPagoBanco,
            this.DetailReportChequesEmitidos});
            this.DataSource = this.bindingSource1;
            this.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 2);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    //se hace automaticamente al setear ReportPrintOptions.PrintOnEmptyDataSource = false;
    //private void DetailReportEfectivo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
    //    Object obj = GetCurrentColumnValue("Efectivo");
    //    if (obj == null) {
    //        DetailReportEfectivo.Visible = false;
    //        return;
    //    }
    //    List<Importe> ll = (List<Importe>)obj;
    //    if (ll.Count > 0) {
    //        DetailReportEfectivo.Visible = true;
    //    } else {
    //        DetailReportEfectivo.Visible = false;
    //    }
            
    //}

    //private void DetailReportCheques_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
    //    Object obj = GetCurrentColumnValue("Cheques");
    //    if (obj == null) {
    //        DetailReportCheques.Visible = false;
    //        return;
    //    }
    //    List<Cheque> ll = (List<Cheque>)obj;
    //    if (ll.Count > 0) {
    //        DetailReportCheques.Visible = true;
    //    } else {
    //        DetailReportCheques.Visible = false;
    //    }
    //}

    //private void DetailReportPagoBanco_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
    //    Object obj = GetCurrentColumnValue("PagosBanco");
    //    if (obj == null) {
    //        DetailReportPagoBanco.Visible = false;
    //        return;
    //    }
    //    List<MovBanco> ll = (List<MovBanco>)obj;
    //    if (ll.Count > 0) {
    //        DetailReportPagoBanco.Visible = true;
    //    } else {
    //        DetailReportPagoBanco.Visible = false;
    //    }
    //}

    //private void DetailReportChequesEmitidos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
    //    Object obj = GetCurrentColumnValue("ChequesEmitidos");
    //    if (obj == null) {
    //        DetailReportChequesEmitidos.Visible = false;
    //        return;
    //    }
    //    List<ChequeEmitido> ll = (List<ChequeEmitido>)obj;
    //    if (ll.Count > 0) {
    //        DetailReportChequesEmitidos.Visible = true;
    //    } else {
    //        DetailReportChequesEmitidos.Visible = false;
    //    }
    //}
}
