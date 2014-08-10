using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DLL_Backend;
using AutomotoraWeb.Utils;
using System.Web;
using AutomotoraWeb.Controllers.Sales;

/// <summary>
/// Summary description for DXReportUsuarios
/// </summary>
public class DXReportUsuarios : DevExpress.XtraReports.UI.XtraReport {
    private BottomMarginBand BottomMargin;
    private TopMarginBand TopMargin;
    private DetailBand Detail;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader1;
    private FormattingRule formattingRulePar;
    private FormattingRule formattingRuleImpar;
    private System.Windows.Forms.BindingSource bindingSource1;
    private ReportHeaderBand ReportHeader;
    private PageFooterBand PageFooter;
    private XRLine xrLine2;
    private XRLabel xrLabel51;
    private XRLabel xrLabel52;
    private XRPageInfo xrPageInfo2;
    private XRPageInfo xrPageInfo1;
    private XRLine xrLine1;
    private CalculatedField habilitadoDesc;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel10;
    private XRLabel xrLabel2;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel3;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private CalculatedField EsDBADesc;
    private CalculatedField MultiSucursalDesc;
    private DetailReportBand DetailReport;
    private DetailBand Detail1;
    private XRLabel xrLabel13;
    private XRLabel xrLabel14;
    private GroupHeaderBand GroupHeader2;
    private GroupFooterBand GroupFooter1;
    private XRLine xrLine3;
    private XRLine xrLine4;
    private XRLabel xrLabel15;
    private CalculatedField sumaHabilitados;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public DXReportUsuarios() {
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
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.formattingRulePar = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRuleImpar = new DevExpress.XtraReports.UI.FormattingRule();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.habilitadoDesc = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.EsDBADesc = new DevExpress.XtraReports.UI.CalculatedField();
            this.MultiSucursalDesc = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.sumaHabilitados = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-05F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(650.0001F, 25.08334F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "LISTADO DE USUARIOS";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 11.45833F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel52,
            this.xrLabel51,
            this.xrLine2});
            this.TopMargin.HeightF = 61.45833F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel52
            // 
            this.xrLabel52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NombreSistemaActivo")});
            this.xrLabel52.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(9.99945F, 10.00001F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(308.75F, 23F);
            this.xrLabel52.StylePriority.UseFont = false;
            // 
            // xrLabel51
            // 
            this.xrLabel51.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NombreEmpresaActiva")});
            this.xrLabel51.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(332.2912F, 10.00001F);
            this.xrLabel51.Name = "xrLabel51";
            this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel51.SizeF = new System.Drawing.SizeF(307.708F, 23F);
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.StylePriority.UseTextAlignment = false;
            this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 44.45836F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(650.0002F, 2.083332F);
            // 
            // Detail
            // 
            this.Detail.BorderWidth = 1F;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLine3});
            this.Detail.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.Detail.HeightF = 35.58332F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorderWidth = false;
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // formattingRulePar
            // 
            this.formattingRulePar.Condition = "[DataSource.CurrentRowIndex] % 2 == 0";
            // 
            // 
            // 
            this.formattingRulePar.Formatting.BackColor = System.Drawing.Color.Gainsboro;
            this.formattingRulePar.Formatting.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.formattingRulePar.Name = "formattingRulePar";
            // 
            // formattingRuleImpar
            // 
            this.formattingRuleImpar.Condition = "[DataSource.CurrentRowIndex] % 2  !=  0";
            // 
            // 
            // 
            this.formattingRuleImpar.Formatting.BackColor = System.Drawing.Color.Transparent;
            this.formattingRuleImpar.Formatting.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.formattingRuleImpar.Name = "formattingRuleImpar";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7});
            this.GroupHeader1.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.GroupHeader1.HeightF = 33.00001F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.RepeatEveryPage = true;
            this.GroupHeader1.StylePriority.UseFont = false;
            // 
            // bindingSource1
            // 
            this.bindingSource1.AllowNew = false;
            this.bindingSource1.DataSource = typeof(DLL_Backend.Usuario);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.ReportHeader.HeightF = 35.50002F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.xrLine1});
            this.PageFooter.HeightF = 50F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy HH:mm:ss}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(9.999457F, 22.54169F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(154.1667F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(539.9996F, 22.54169F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.458307F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(650.0002F, 4.083315F);
            // 
            // habilitadoDesc
            // 
            this.habilitadoDesc.Expression = "Iif([Habilitado], \'SI\'  , \'NO\' )";
            this.habilitadoDesc.Name = "habilitadoDesc";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel15,
            this.xrLine4,
            this.xrLabel10});
            this.ReportFooter.HeightF = 54.16668F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Codigo")});
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 31.16668F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(285.8333F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseForeColor = false;
            xrSummary2.FormatString = "Total: {0} Usuarios";
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel10.Summary = xrSummary2;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UserName")});
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 12.58332F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(95.20832F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "xrLabel2";
            // 
            // EsDBADesc
            // 
            this.EsDBADesc.Expression = "Iif([EsDBA], \'SI\'  , \'NO\' )";
            this.EsDBADesc.Name = "EsDBADesc";
            // 
            // MultiSucursalDesc
            // 
            this.MultiSucursalDesc.Expression = "Iif([MultiSucursal], \'SI\'  , \'NO\' )";
            this.MultiSucursalDesc.Name = "MultiSucursalDesc";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "habilitadoDesc")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(120.8333F, 12.58332F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(31.87498F, 23F);
            this.xrLabel3.Text = "xrLabel3";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.Nombre")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(169.3749F, 12.58332F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(104.1667F, 23F);
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MultiSucursalDesc")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(295.8329F, 12.58332F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(37.08374F, 23F);
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EsDBADesc")});
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(375.625F, 12.58332F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(33.33334F, 23F);
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel7.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 9.29168F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(95.20833F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.Text = "Login";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel8.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(105.2083F, 9.29168F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(64.16667F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.Text = "Habilitado";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel9.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(169.375F, 9.29168F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(104.1666F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.Text = "Sucursal";
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel11.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(273.5416F, 9.29168F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(84.37503F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseForeColor = false;
            this.xrLabel11.Text = "Multi Sucursal";
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.xrLabel12.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(357.9166F, 9.29168F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(79.16666F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseForeColor = false;
            this.xrLabel12.Text = "Administrador";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.GroupHeader2,
            this.GroupFooter1});
            this.DetailReport.DataMember = "Perfiles";
            this.DetailReport.DataSource = this.bindingSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            this.DetailReport.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13,
            this.xrLabel14});
            this.Detail1.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.Detail1.HeightF = 19F;
            this.Detail1.Name = "Detail1";
            this.Detail1.StylePriority.UseFont = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Perfiles.Nombre")});
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(219.375F, 0F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(277.0833F, 19F);
            this.xrLabel13.Text = "xrLabel13";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.HeightF = 1.041667F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel14.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(169.375F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.ProcessDuplicates = DevExpress.XtraReports.UI.ValueSuppressType.Suppress;
            this.xrLabel14.SizeF = new System.Drawing.SizeF(50.00003F, 19F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseForeColor = false;
            this.xrLabel14.Text = "Perfiles";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 19.79167F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine3
            // 
            this.xrLine3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine3.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(650F, 12.58333F);
            this.xrLine3.StylePriority.UseBorderDashStyle = false;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            // 
            // sumaHabilitados
            // 
            this.sumaHabilitados.Expression = "Iif([Habilitado], 1  , 0 )";
            this.sumaHabilitados.Name = "sumaHabilitados";
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sumaHabilitados")});
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(357.9166F, 31.16668F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(282.0826F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseForeColor = false;
            xrSummary1.FormatString = "Habilitados: {0} Usuarios";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel15.Summary = xrSummary1;
            // 
            // DXReportUsuarios
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter,
            this.DetailReport});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.habilitadoDesc,
            this.EsDBADesc,
            this.MultiSucursalDesc,
            this.sumaHabilitados});
            this.DataSource = this.bindingSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRulePar,
            this.formattingRuleImpar});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 61, 11);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion


}
