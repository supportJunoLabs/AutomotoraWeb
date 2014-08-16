using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DLL_Backend;
using System.Collections.Generic;

/// <summary>
/// Summary description for DXReciboVentaAnulacion
/// </summary>
public class DXReciboVentaAnulacion : DevExpress.XtraReports.UI.XtraReport {
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private System.Windows.Forms.BindingSource bindingSource1;
    private XRLabel xrLabel52;
    private XRLabel xrLabel51;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel4;
    private XRLabel xrLabel5;
    private XRLine xrLine1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel1;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLabel xrLabel27;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRLabel xr_entrega2;
    private XRLabel xr_entrega1;
    private XRLabel xrLabel20;
    private XRLabel xrLabel21;
    private PageFooterBand PageFooter;
    private GroupFooterBand gf_senia;
    private XRSubreport sr_senia;
    private GroupFooterBand gf_pago;
    private XRSubreport sr_pago;
    private GroupFooterBand gf_acv;
    private XRLabel xrLabel35;
    private XRSubreport sr_pagos_acv;
    private GroupFooterBand gf_cuotas;
    private XRSubreport sr_cuotas;
    private GroupFooterBand gf_vales;
    private XRSubreport sr_vales;
    private GroupFooterBand gf_permuta;
    private XRSubreport sr_permuta;
    private GroupFooterBand GroupFooter1;
    private XRLabel xrLabel37;
    private XRLabel xrLabel36;
    private XRLabel xrLabel39;
    private XRLabel xrLabel38;
    private XRLabel xrLabel40;
    private GroupFooterBand gf_devolucion;
    private XRLabel xrLabel42;
    private XRLabel xrLabel41;
    private XRSubreport sr_devolucion;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public DXReciboVentaAnulacion() {
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
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xr_entrega2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xr_entrega1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.gf_senia = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_senia = new DevExpress.XtraReports.UI.XRSubreport();
            this.gf_pago = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_pago = new DevExpress.XtraReports.UI.XRSubreport();
            this.gf_acv = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_pagos_acv = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.gf_cuotas = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_cuotas = new DevExpress.XtraReports.UI.XRSubreport();
            this.gf_vales = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_vales = new DevExpress.XtraReports.UI.XRSubreport();
            this.gf_permuta = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_permuta = new DevExpress.XtraReports.UI.XRSubreport();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.gf_devolucion = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sr_devolucion = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel38,
            this.xrLabel21,
            this.xrLabel20,
            this.xr_entrega2,
            this.xr_entrega1,
            this.xrLabel34,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel27,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9});
            this.Detail.HeightF = 313.5417F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel40
            // 
            this.xrLabel40.CanGrow = false;
            this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Observaciones")});
            this.xrLabel40.ForeColor = System.Drawing.Color.Gray;
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(9.998576F, 251.6666F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(620.0011F, 22.99998F);
            this.xrLabel40.StylePriority.UseForeColor = false;
            // 
            // xrLabel39
            // 
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(459.1664F, 32.99999F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.xrLabel39.Text = "del vehículo:";
            // 
            // xrLabel38
            // 
            this.xrLabel38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Fecha", "{0:dd/MM/yyyy}")});
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(364.1665F, 32.99999F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(86.45828F, 23F);
            this.xrLabel38.Text = "xrLabel38";
            // 
            // xrLabel21
            // 
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 280.5417F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(619.9997F, 23.00003F);
            this.xrLabel21.Text = "El pago de la venta original cancelada se recibió según el siguiente detalle:";
            // 
            // xrLabel20
            // 
            this.xrLabel20.CanGrow = false;
            this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Observaciones")});
            this.xrLabel20.ForeColor = System.Drawing.Color.Gray;
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(9.998719F, 228.6666F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(620.0011F, 22.99998F);
            this.xrLabel20.StylePriority.UseForeColor = false;
            // 
            // xr_entrega2
            // 
            this.xr_entrega2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.FechaEntrega", "{0:dd/MM/yyyy}")});
            this.xr_entrega2.LocationFloat = new DevExpress.Utils.PointFloat(321.4583F, 205.6666F);
            this.xr_entrega2.Name = "xr_entrega2";
            this.xr_entrega2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xr_entrega2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xr_entrega2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xr_entrega2_BeforePrint);
            // 
            // xr_entrega1
            // 
            this.xr_entrega1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 205.6666F);
            this.xr_entrega1.Name = "xr_entrega1";
            this.xr_entrega1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xr_entrega1.SizeF = new System.Drawing.SizeF(311.4583F, 23F);
            this.xr_entrega1.Text = "con entrega registrada del vehiculo al cliente de fecha";
            this.xr_entrega1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xr_entrega1_BeforePrint);
            // 
            // xrLabel34
            // 
            this.xrLabel34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Chasis")});
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(413.3331F, 136.6666F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(216.6667F, 23F);
            // 
            // xrLabel33
            // 
            this.xrLabel33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Motor")});
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(413.3331F, 113.6666F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(216.6667F, 22.99999F);
            // 
            // xrLabel32
            // 
            this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Padron")});
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(413.3331F, 90.66664F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(216.6667F, 23F);
            // 
            // xrLabel31
            // 
            this.xrLabel31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Color")});
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(413.3331F, 67.66666F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(216.6667F, 23F);
            // 
            // xrLabel30
            // 
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(36.04166F, 67.66666F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel30.Text = "Ficha";
            // 
            // xrLabel29
            // 
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(36.04166F, 90.66664F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel29.Text = "Matricula";
            // 
            // xrLabel28
            // 
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(36.04166F, 113.6666F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel28.Text = "Marca";
            // 
            // xrLabel27
            // 
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(36.04164F, 136.6666F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel27.Text = "Modelo";
            // 
            // xrLabel26
            // 
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(36.04164F, 159.6666F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel26.Text = "Año";
            // 
            // xrLabel25
            // 
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(342.4998F, 67.66666F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel25.Text = "Color";
            // 
            // xrLabel24
            // 
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(342.4997F, 90.66664F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel24.Text = "Padron";
            // 
            // xrLabel23
            // 
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(341.6667F, 113.6666F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel23.Text = "Motor";
            // 
            // xrLabel22
            // 
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(341.6667F, 136.6666F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel22.Text = "Chasis";
            // 
            // xrLabel19
            // 
            this.xrLabel19.CanGrow = false;
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Observaciones")});
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(36.04164F, 182.6666F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(593.9581F, 23F);
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Anio")});
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(106.875F, 159.6666F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(214.5833F, 23F);
            // 
            // xrLabel17
            // 
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Modelo")});
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(106.875F, 136.6666F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(214.5833F, 22.99998F);
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Marca")});
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(106.875F, 113.6666F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(214.5833F, 22.99999F);
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Matricula")});
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(106.875F, 90.66664F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(214.5833F, 23F);
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Vehiculo.Ficha")});
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(106.875F, 67.66666F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(214.5833F, 23F);
            // 
            // xrLabel13
            // 
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(308.75F, 32.99999F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(55.41647F, 23F);
            this.xrLabel13.Text = "de fecha";
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Importe.ImporteTexto")});
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(149.5834F, 32.99999F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(142.7083F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "xrLabel12";
            // 
            // xrLabel11
            // 
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 32.99999F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(126.0417F, 23F);
            this.xrLabel11.Text = "por el equivalente  a";
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Cliente.Nombre")});
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(149.5834F, 10.00001F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(480.4163F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(139.5834F, 23F);
            this.xrLabel9.Text = "Cancelación de venta a ";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel52,
            this.xrLabel51});
            this.TopMargin.HeightF = 41.66667F;
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
            this.xrLabel52.SizeF = new System.Drawing.SizeF(298.75F, 23F);
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
            this.xrLabel51.SizeF = new System.Drawing.SizeF(307.708F, 23F);
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.StylePriority.UseTextAlignment = false;
            this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 1.041667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel1,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel4,
            this.xrLabel5});
            this.ReportHeader.HeightF = 59.37503F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel8
            // 
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(459.1665F, 33.00002F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Nro Op";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Codigo")});
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(529.9998F, 33.00002F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel7
            // 
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(9.998726F, 10.00001F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel7.Text = "Fecha";
            // 
            // xrLabel6
            // 
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(237.9167F, 10.00001F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel6.Text = "Sucursal";
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(459.1664F, 10.00001F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(70.83334F, 23F);
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Nro Trans";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NroRecibo")});
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(529.9998F, 10.00001F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Fecha", "{0:dd/MM/yyyy}")});
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(80.83207F, 10.00001F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.Nombre")});
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(308.75F, 10.00001F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(650.0002F, 4.083315F);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(540.0005F, 21.83336F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy HH:mm:ss}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 21.83336F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(154.1667F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.xrLine1});
            this.PageFooter.HeightF = 58.33333F;
            this.PageFooter.Name = "PageFooter";
            // 
            // gf_senia
            // 
            this.gf_senia.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_senia});
            this.gf_senia.HeightF = 44.79167F;
            this.gf_senia.Name = "gf_senia";
            this.gf_senia.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_senia_BeforePrint);
            // 
            // sr_senia
            // 
            this.sr_senia.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.sr_senia.Name = "sr_senia";
            this.sr_senia.ReportSource = new _DXSubrepVentaSenia();
            this.sr_senia.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.sr_senia.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_senia_BeforePrint);
            // 
            // gf_pago
            // 
            this.gf_pago.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_pago});
            this.gf_pago.HeightF = 45.83333F;
            this.gf_pago.Level = 1;
            this.gf_pago.Name = "gf_pago";
            this.gf_pago.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_pago_BeforePrint);
            // 
            // sr_pago
            // 
            this.sr_pago.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.sr_pago.Name = "sr_pago";
            this.sr_pago.ReportSource = new _DXSubrepPago();
            this.sr_pago.SizeF = new System.Drawing.SizeF(650.0002F, 23F);
            this.sr_pago.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_pago_BeforePrint);
            // 
            // gf_acv
            // 
            this.gf_acv.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_pagos_acv,
            this.xrLabel35});
            this.gf_acv.HeightF = 60.41667F;
            this.gf_acv.Level = 2;
            this.gf_acv.Name = "gf_acv";
            this.gf_acv.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_acv_BeforePrint);
            // 
            // sr_pagos_acv
            // 
            this.sr_pagos_acv.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.99995F);
            this.sr_pagos_acv.Name = "sr_pagos_acv";
            this.sr_pagos_acv.ReportSource = new _DXSubrepPago();
            this.sr_pagos_acv.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.sr_pagos_acv.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_pagos_acv_BeforePrint);
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(239.5833F, 23F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.Text = "Pagos Registrados a Cuenta";
            // 
            // gf_cuotas
            // 
            this.gf_cuotas.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_cuotas});
            this.gf_cuotas.HeightF = 41.66667F;
            this.gf_cuotas.Level = 3;
            this.gf_cuotas.Name = "gf_cuotas";
            this.gf_cuotas.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_cuotas_BeforePrint);
            // 
            // sr_cuotas
            // 
            this.sr_cuotas.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.sr_cuotas.Name = "sr_cuotas";
            this.sr_cuotas.ReportSource = new _DXCuotasVenta();
            this.sr_cuotas.SizeF = new System.Drawing.SizeF(650.0001F, 23F);
            this.sr_cuotas.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_cuotas_BeforePrint);
            // 
            // gf_vales
            // 
            this.gf_vales.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_vales});
            this.gf_vales.HeightF = 41.66667F;
            this.gf_vales.Level = 4;
            this.gf_vales.Name = "gf_vales";
            this.gf_vales.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_vales_BeforePrint);
            // 
            // sr_vales
            // 
            this.sr_vales.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.sr_vales.Name = "sr_vales";
            this.sr_vales.ReportSource = new _DXValesVenta();
            this.sr_vales.SizeF = new System.Drawing.SizeF(650.0002F, 23F);
            this.sr_vales.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_vales_BeforePrint);
            // 
            // gf_permuta
            // 
            this.gf_permuta.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_permuta});
            this.gf_permuta.HeightF = 43.75F;
            this.gf_permuta.Level = 5;
            this.gf_permuta.Name = "gf_permuta";
            this.gf_permuta.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_permuta_BeforePrint);
            // 
            // sr_permuta
            // 
            this.sr_permuta.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.sr_permuta.Name = "sr_permuta";
            this.sr_permuta.ReportSource = new _DXSubrepPermutaVenta();
            this.sr_permuta.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.sr_permuta.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_permuta_BeforePrint);
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel37,
            this.xrLabel36});
            this.GroupFooter1.Level = 7;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLabel37
            // 
            this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NombreEmpresaActiva")});
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(382.2917F, 67.00001F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(247.7082F, 23F);
            this.xrLabel37.StylePriority.UseBorders = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "xrLabel37";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Venta.Cliente.Nombre")});
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(36.04167F, 67.00001F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(285.4166F, 23F);
            this.xrLabel36.StylePriority.UseBorders = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // gf_devolucion
            // 
            this.gf_devolucion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sr_devolucion,
            this.xrLabel42,
            this.xrLabel41});
            this.gf_devolucion.HeightF = 111.4583F;
            this.gf_devolucion.Level = 6;
            this.gf_devolucion.Name = "gf_devolucion";
            this.gf_devolucion.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gf_devolucion_BeforePrint);
            // 
            // sr_devolucion
            // 
            this.sr_devolucion.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 78.45834F);
            this.sr_devolucion.Name = "sr_devolucion";
            this.sr_devolucion.ReportSource = new _DXSubrepPago();
            this.sr_devolucion.SizeF = new System.Drawing.SizeF(617.4997F, 23F);
            this.sr_devolucion.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_devolucion_BeforePrint);
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(239.5833F, 23F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.Text = "DEVOLUCION:";
            // 
            // xrLabel41
            // 
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 22.99995F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(619.9997F, 36.5417F);
            this.xrLabel41.Text = "Se anulan los documentos de deuda mencionados y se reintegra al cliente efectivo " +
    "y cheques recibidos según el siguiente detalle:";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(DLL_Backend.TRVentaAnulacion);
            // 
            // DXReciboVentaAnulacion
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.gf_senia,
            this.gf_pago,
            this.gf_acv,
            this.gf_cuotas,
            this.gf_vales,
            this.gf_permuta,
            this.GroupFooter1,
            this.gf_devolucion});
            this.DataSource = this.bindingSource1;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 42, 1);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xr_entrega1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Entregado) {
            xr_entrega1.Visible = true;
        } else {
            xr_entrega1.Visible = false;
        }
    }

    private void xr_entrega2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Entregado) {
            xr_entrega2.Visible = true;
        } else {
            xr_entrega2.Visible = false;
        }
    }

    private void sr_senia_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Senia != null) {
            List<Senia> ll = new List<Senia>();
            ll.Add(v.Senia);
            sr_senia.ReportSource.DataSource = ll;
        }
    }

    private void gf_senia_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Senia == null || v.Senia.Codigo <= 0) {
            gf_senia.Visible = false;
        }
    }

    private void sr_pago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        PagoTransaccion p = v.Pago;
        if (p != null && p.CantMovs > 0) {
            List<PagoTransaccion> ll = new List<PagoTransaccion>();
            ll.Add(p);
            sr_pago.ReportSource.DataSource = ll;
        }
    }


    private void gf_pago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        PagoTransaccion p = v.Pago;
        if (p == null || p.CantMovs <= 0) {
            gf_pago.Visible = false;
        }
    }


    private void sr_pagos_acv_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        PagoTransaccion p1 = v.pagosTransaccionesPreventa();
        if (p1 != null && p1.CantMovs > 0) {
            List<PagoTransaccion> ll = new List<PagoTransaccion>();
            ll.Add(p1);
            sr_pagos_acv.ReportSource.DataSource = ll;
        }
    }

    private void gf_acv_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        PagoTransaccion p1 = v.pagosTransaccionesPreventa();
        if (p1 == null || p1.CantMovs <= 0) {
            gf_acv.Visible = false;

        }
    }

    private void sr_cuotas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        Financiacion f = v.Financiacion;
        if (f != null && f.CantCuotas > 0) {
            List<Financiacion> ll = new List<Financiacion>();
            ll.Add(f);
            sr_cuotas.ReportSource.DataSource = ll;
        }
    }

    private void gf_cuotas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        Financiacion f = v.Financiacion;
        if (f == null || f.CantCuotas <= 0) {
            gf_cuotas.Visible = false;
        }
    }

    private void sr_vales_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if(v.ValesOriginales==null || v.ValesOriginales.Count==0){
            return;
        }
        sr_vales.ReportSource.DataSource = v.ValesOriginales;
    }

    private void gf_vales_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if(v.ValesOriginales==null || v.ValesOriginales.Count==0){
            gf_vales.Visible = false;
        }
    }


    private void sr_permuta_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Permuta == null) {
            return;
        }
        List<Vehiculo> ll = new List<Vehiculo>();
        ll.Add(v.Permuta);
        sr_permuta.ReportSource.DataSource = ll;
    }

    private void gf_permuta_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        Venta v = (Venta)GetCurrentColumnValue("Venta");
        if (v.Permuta == null) {
            gf_permuta.Visible = false;
        }
    }


    private void sr_devolucion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        PagoTransaccion p = (PagoTransaccion)GetCurrentColumnValue("Pago");
        if (p != null && p.CantMovs > 0) {
            List<PagoTransaccion> ll = new List<PagoTransaccion>();
            ll.Add(p);
            sr_devolucion.ReportSource.DataSource = ll;
        }
    }

    private void gf_devolucion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        PagoTransaccion p = (PagoTransaccion)GetCurrentColumnValue("Pago");
        if (p != null && p.CantMovs > 0) {
            gf_devolucion.Visible = true;
        } else {
            gf_devolucion.Visible = false;
        }
    }

}
