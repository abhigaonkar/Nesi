using System;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.PurchaseOrder;

namespace nesi.core.print
{
    /// <summary>
    /// Summary description for popheaderdisp
    /// </summary>
    public class popheaderdisp : XtraReport
    {
        private DetailBand Detail;
        private TopMarginBand TopMargin;
        private BottomMarginBand BottomMargin;
        private poheaderdisplay poheaderdisplay1;
        private ReportHeaderBand ReportHeader;
        private ReportFooterBand ReportFooter;
        private XRLabel xrLabel1;
        private DevExpress.XtraReports.Parameters.Parameter poprogid;

        private XRLabel xrLabel11;
        private XRLabel xrLabel21;
        private XRLabel xrLabel28;
        private XRSubreport xrSubreportPODetails;
        private XRLabel xrLabel26;
        private XRLabel xrLabel12;
        private XRLabel xrLabel13;
        private XRLabel xrLabel14;
        private XRLabel xrLabel2;
        private XRLabel xrLabel4;
        private XRLabel xrLabel5;
        private XRPageInfo xrPageInfo1;
        private PageFooterBand PageFooter;
        private XRLabel xrLabel6;
        private XRLabel xrLabel8;
        private XRLabel xrLabel9;
        private XRLabel labelPM;
        private XRLabel textShipAddress;
        private PageHeaderBand PageHeader;
        private XRPanel xrPanel1;
        private XRLabel xrLabel3;
        private XRLabel xrLabel7;
        private XRLabel xrLabel15;
        private XRLabel xrLabel25;
        private XRLabel xrLabel17;
        private XRLabel xrLabel18;
        private XRLabel xrLabel19;
        private XRLabel xrLabel20;
        private XRLabel lblTerms;
        private XRLabel xrLabel22;
        private XRLabel xrLabelNotes;
        private XRPictureBox xrPictureBox1;
        private XRBarCode xrBarCode1;
        private XRBarCode xrBarCode2;
        private XRLabel xrLabel23;
        private XRLabel xrLabel16;
        private XRLabel lbl_currency;
        private XRLabel textInvoiceAddress;
        private XRLabel xrLabel24;
        private XRLabel xrLabel10;
        private XRLabel xrTermsConditions;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public popheaderdisp()
        {
            InitializeComponent();
            AddTermsConditionsFooter();
            //
            //  
            //
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator code39ExtendedGenerator1 = new DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator();
            DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator code39ExtendedGenerator2 = new DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrSubreportPODetails = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.poheaderdisplay1 = new Nesi.Web.Reports.PurchaseOrder.poheaderdisplay();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lbl_currency = new DevExpress.XtraReports.UI.XRLabel();
            this.textInvoiceAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabelNotes = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTerms = new DevExpress.XtraReports.UI.XRLabel();
            this.textShipAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.labelPM = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.poprogid = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTermsConditions = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.poheaderdisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreportPODetails});
            this.Detail.HeightF = 26F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrSubreportPODetails
            // 
            this.xrSubreportPODetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrSubreportPODetails.Name = "xrSubreportPODetails";
            this.xrSubreportPODetails.SizeF = new System.Drawing.SizeF(742F, 22.99999F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 35F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 6F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // poheaderdisplay1
            // 
            this.poheaderdisplay1.DataSetName = "poheaderdisplay";
            this.poheaderdisplay1.EnforceConstraints = false;
            this.poheaderdisplay1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.lbl_currency,
            this.textInvoiceAddress,
            this.xrLabel24,
            this.xrBarCode2,
            this.xrLabelNotes,
            this.xrLabel22,
            this.lblTerms,
            this.textShipAddress,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel21,
            this.xrPanel1});
            this.ReportHeader.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportHeader.HeightF = 377.1667F;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.StylePriority.UseFont = false;
            // 
            // lbl_currency
            // 
            this.lbl_currency.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_currency.LocationFloat = new DevExpress.Utils.PointFloat(401.2075F, 354.1667F);
            this.lbl_currency.Name = "lbl_currency";
            this.lbl_currency.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_currency.SizeF = new System.Drawing.SizeF(330.2086F, 22.99998F);
            this.lbl_currency.StylePriority.UseFont = false;
            this.lbl_currency.StylePriority.UseTextAlignment = false;
            this.lbl_currency.Text = "lbl_currency";
            this.lbl_currency.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // textInvoiceAddress
            // 
            this.textInvoiceAddress.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textInvoiceAddress.LocationFloat = new DevExpress.Utils.PointFloat(402.4993F, 301.5F);
            this.textInvoiceAddress.Multiline = true;
            this.textInvoiceAddress.Name = "textInvoiceAddress";
            this.textInvoiceAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.textInvoiceAddress.SizeF = new System.Drawing.SizeF(330.2089F, 42.20831F);
            this.textInvoiceAddress.StylePriority.UseFont = false;
            this.textInvoiceAddress.StylePriority.UseTextAlignment = false;
            this.textInvoiceAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(402.4993F, 265.625F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(330.2086F, 14F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Send Invoice To:";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrBarCode2
            // 
            this.xrBarCode2.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrBarCode2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?poprogid")});
            this.xrBarCode2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(399.9578F, 140.375F);
            this.xrBarCode2.Module = 1F;
            this.xrBarCode2.Name = "xrBarCode2";
            this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode2.ShowText = false;
            this.xrBarCode2.SizeF = new System.Drawing.SizeF(340.7499F, 24.79166F);
            this.xrBarCode2.StylePriority.UseFont = false;
            this.xrBarCode2.StylePriority.UseTextAlignment = false;
            code39ExtendedGenerator1.WideNarrowRatio = 3F;
            this.xrBarCode2.Symbology = code39ExtendedGenerator1;
            this.xrBarCode2.Text = "004-4632";
            this.xrBarCode2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            this.xrBarCode2.TextFormatString = "004-{0}";
            // 
            // xrLabelNotes
            // 
            this.xrLabelNotes.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelNotes.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 282.75F);
            this.xrLabelNotes.Multiline = true;
            this.xrLabelNotes.Name = "xrLabelNotes";
            this.xrLabelNotes.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelNotes.SizeF = new System.Drawing.SizeF(381.7917F, 82.70825F);
            this.xrLabelNotes.StylePriority.UseFont = false;
            this.xrLabelNotes.StylePriority.UseTextAlignment = false;
            this.xrLabelNotes.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(402.499F, 170.375F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(330.2086F, 14F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Ship To:";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTerms
            // 
            this.lblTerms.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerms.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 246.2084F);
            this.lblTerms.Name = "lblTerms";
            this.lblTerms.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTerms.SizeF = new System.Drawing.SizeF(330.2086F, 22.99998F);
            this.lblTerms.StylePriority.UseFont = false;
            this.lblTerms.StylePriority.UseTextAlignment = false;
            this.lblTerms.Text = "lblTerms";
            this.lblTerms.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // textShipAddress
            // 
            this.textShipAddress.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textShipAddress.LocationFloat = new DevExpress.Utils.PointFloat(402.499F, 187.5F);
            this.textShipAddress.Multiline = true;
            this.textShipAddress.Name = "textShipAddress";
            this.textShipAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.textShipAddress.SizeF = new System.Drawing.SizeF(330.2089F, 60.95834F);
            this.textShipAddress.StylePriority.UseFont = false;
            this.textShipAddress.StylePriority.UseTextAlignment = false;
            this.textShipAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.VendPhoneBlock]")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 218.75F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(119.7083F, 15.70834F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.VendCityBlock]")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 204.75F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(200F, 14F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrLabel2
            // 
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.VendAddressBlock]")});
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 190.7501F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(200F, 14F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrLabel21
            // 
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Vendor_Name]")});
            this.xrLabel21.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(0F, 170.375F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(299.0834F, 20.375F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.Text = "xrLabel21";
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel23,
            this.xrLabel16,
            this.xrPictureBox1,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel25,
            this.xrLabel15,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel28,
            this.xrLabel8,
            this.xrLabel9,
            this.labelPM,
            this.xrLabel3,
            this.xrLabel26,
            this.xrLabel1,
            this.xrLabel11,
            this.xrLabel12,
            this.xrLabel14,
            this.xrLabel13});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 12.5F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(741.7083F, 120.5833F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrLabel23
            // 
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.poprog_date_required]")});
            this.xrLabel23.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(631.7081F, 103.25F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(100.0001F, 12.58335F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel23.TextFormatString = "{0:d-MMM-yy}";
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 104.2499F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(93.66711F, 11.20836F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Required Date:";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(2.125009F, 2.861023E-06F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(225F, 117.4584F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrPictureBox1.StylePriority.UseBackColor = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(238.9582F, 72.00003F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(69.29166F, 13.99998F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Fax No::";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(238.9584F, 58.00002F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(69.29164F, 14F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Phone No:";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[poprog_order_placed_date]")});
            this.xrLabel25.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(631.7081F, 90.6666F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(100.0001F, 12.58335F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "xrLabel25";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel25.TextFormatString = "{0:d-MMM-yy}";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 78.08325F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(93.66711F, 12.58336F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Shipping Method:";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 90.6666F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(69.79163F, 12.58334F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Order Date:";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 5.722046E-06F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(198.0001F, 27.87497F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Purchase Order";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel28
            // 
            this.xrLabel28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.Total_PO_Number]")});
            this.xrLabel28.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 29.87498F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(198F, 19.29162F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 51.49989F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(80.20831F, 14.00001F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Vendor No:";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(533.7083F, 65.49991F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(69.79163F, 12.58334F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Purchaser:";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // labelPM
            // 
            this.labelPM.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPM.LocationFloat = new DevExpress.Utils.PointFloat(631.7081F, 65.49991F);
            this.labelPM.Name = "labelPM";
            this.labelPM.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.labelPM.SizeF = new System.Drawing.SizeF(100F, 12.58334F);
            this.labelPM.StylePriority.UseFont = false;
            this.labelPM.StylePriority.UseTextAlignment = false;
            this.labelPM.Text = "labelPM";
            this.labelPM.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vendor_number]")});
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(631.7081F, 51.49989F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(99.99997F, 14F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "xrLabel3";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel26
            // 
            this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[shipping_method]")});
            this.xrLabel26.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(631.7081F, 78.08325F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(100F, 12.58336F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "xrLabel26";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel1
            // 
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name]")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(238.9582F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(276.375F, 17.87497F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address]")});
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(238.9582F, 27.87498F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(276.375F, 15.12504F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.CompanyCityBlock]")});
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(238.9584F, 43.00003F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(276.3749F, 14F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "xrLabel12";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.CompanyFaxBlock]")});
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(308.2498F, 72.00003F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(137.5F, 14.99999F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel14";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[vw_poheaderinfo.CompanyPhoneBlock]")});
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(308.2498F, 58.00012F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(137.5F, 13.99989F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "xrLabel13";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // poprogid
            // 
            this.poprogid.Name = "poprogid";
            this.poprogid.Type = typeof(int);
            this.poprogid.ValueInfo = "0";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(309.2498F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
            this.PageFooter.HeightF = 26F;
            this.PageFooter.Name = "PageFooter";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1,
            this.xrLabel18,
            this.xrLabel17});
            this.PageHeader.HeightF = 62.5F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?poprogid")});
            this.xrBarCode1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 10.00001F);
            this.xrBarCode1.Module = 1F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(340.7499F, 39.66659F);
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            code39ExtendedGenerator2.WideNarrowRatio = 3F;
            this.xrBarCode1.Symbology = code39ExtendedGenerator2;
            this.xrBarCode1.Text = "004-1234";
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            this.xrBarCode1.TextFormatString = "004-{0}";
            // 
            // xrLabel18
            // 
            this.xrLabel18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Vendor_Name]")});
            this.xrLabel18.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(356.3749F, 29.2916F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(387.6251F, 20.375F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "xrLabel21";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTermsConditions});
            this.ReportFooter.Dpi = 96F;
            this.ReportFooter.HeightF = 1292.517F;
            this.ReportFooter.Name = "ReportFooter";
            //
            // xrTermsConditions
            // 
            this.xrTermsConditions.Dpi = 96F;
            this.xrTermsConditions.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTermsConditions.KeepTogether = true;
            this.xrTermsConditions.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 86.49371F);
            this.xrTermsConditions.Multiline = true;
            this.xrTermsConditions.Name = "xrTermsConditions";
            this.xrTermsConditions.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTermsConditions.SizeF = new System.Drawing.SizeF(554.5212F, 18.07983F);
            this.xrTermsConditions.StylePriority.UseFont = false;
            this.xrTermsConditions.StylePriority.UseTextAlignment = false;
            this.xrTermsConditions.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[poprog_bvpo]")});
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(546F, 10.00001F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(198F, 19.29162F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "xrLabel28";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name]")});
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 11.25F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(402.4993F, 283.625F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(328.9167F, 17.87497F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel1";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // popheaderdisp
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageFooter,
            this.PageHeader});
            this.DataMember = "DataTable1";
            this.DataSource = this.poheaderdisplay1;
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(49, 57, 35, 6);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.poprogid});
            this.Version = "19.2";
            this.Watermark.ShowBehind = false;
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.poheader_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.poheaderdisplay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private void poheader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            bool print_preview = false;
            var poprocheckid = poprogid.Value.ToString();
            var tempo = new NePOProg(Convert.ToInt32(poprocheckid));

            if ((tempo.poprog_status == 1) ||
                (tempo.poprog_status == 2) ||
                (tempo.poprog_status == 5) ||
                (tempo.poprog_status == 8) ||
                (tempo.poprog_status == 9))
            {
                print_preview = true;

            }


            var wm = this.Watermark;
            if (print_preview)
            {
                wm.Text = "NOT APPROVED FOR PURCHASE";

            }
            else if ((tempo.poprog_status == 4) ||
                     (tempo.poprog_status == 6) ||
                     (tempo.poprog_status == 7) ||
                     (tempo.poprog_status == 10) ||
                     (tempo.poprog_status == 11) ||
                     (tempo.poprog_status == 12)
            )
            {
                wm.Text = "REPRINT";
            }
            else
            {
                wm.Text = "";
            }


            var tools = new Toolbox();
            //	xrBarCode1.Symbology.CalcCheckSum = false;
            //	xrBarCode2.Symbology.CalcCheckSum = false;
            try
            {
                var vendid = tools.getSQL_string(@"SELECT poprog_vendor_id FROM poprog_header  WHERE poprog_id =@v0", new object[] { poprocheckid });
                var vedntermid = tools.getSQL_string(@"SELECT vendor_term_id FROM vendor  WHERE vendor_id = @v0", new object[] { vendid });
                if (vedntermid != "0")
                {
                    var a = tools.getSQL_string(@"SELECT poprog_poprog_payment_method_id FROM poprog_header  WHERE poprog_id=@v0", new object[] { poprocheckid });
                    var terms = tools.getSQL_string(@"Select ifnull((SELECT term_desc FROM term  WHERE term_id =@v0),'')", new object[] { a });
                    if (terms.Length > 2)
                    {
                        lblTerms.Text = "Terms: " + terms;
                        lblTerms.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    }
                    else
                    {
                        lblTerms.Text = "";
                    }
                }
                else
                {
                    lblTerms.Text = "";
                }
            }
            catch { lblTerms.Text = ""; }

            try
            {
                var address = NePOProg.GetShippingAddress((int)poprogid.Value);
                //tools.getSQL_string(@"SELECT URLDECODE(poprog_manual_shipaddress) FROM poprog_header  WHERE poprog_id=@v0", new object[] { poprocheckid });

                if (address.Trim() == "")
                {
                    textShipAddress.Text = "Same as above";
                }
                else
                {
                    textShipAddress.Text = address.Replace("<br />", "\n");
                    textShipAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    textShipAddress.Multiline = true;
                }
                //memoAddress.Text = address;

                var invoiceAddr = new NeBusinessUnit(tempo.business_unit_id);
                textInvoiceAddress.Text = "c/o Spark Power Corp" + "\n" + invoiceAddr.po_billing_address;
                textInvoiceAddress.Multiline = true;
                textInvoiceAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

                lbl_currency.Text = "Currency: " + tools.getSQL_string(@"Select ifnull((Select currency from currency inner join poprog_header on poprog_header.poprog_country_code_currency=currency.id  where poprog_id=@v0  limit 1),'Unknown')", new object[] { poprocheckid });
            }
            catch { }
            try
            {
                //xrLabelDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                var popheadertableadapter = new Nesi.Web.Reports.PurchaseOrder.poheaderdisplayTableAdapters.vw_poheaderinfoTableAdapter();
                popheadertableadapter.Fill(poheaderdisplay1.vw_poheaderinfo, Convert.ToInt32(poprogid.Value));

                var report = new podetailsdisp();
                report.Parameters[0].Value = Convert.ToInt32(poprogid.Value);
                report.Parameters[1].Value = print_preview;

                xrSubreportPODetails.ReportSource = report;

                xrLabelNotes.Text = tools.getSQL_string(@"SELECT URLDECODE(poprog_order_description) FROM poprog_header  WHERE poprog_id = @v0", new object[] { poprogid.Value.ToString() });
                if (xrLabelNotes.Text != "")
                {
                    xrLabelNotes.Text = "*** " + xrLabelNotes.Text;
                }



            }
            catch (Exception ee)
            {
                tools.catch_error(ee);
                throw;
            }
            try
            {



                var companyid = tempo.business_unit_id.ToString();
                var pmname = tools.getSQL_string(@"SELECT Get_Name(@v0)", new object[] { tempo.poprog_cutby_member_id });
                labelPM.Text = pmname;
                if ((tempo.poprog_status == 1) && (!print_preview))
                {
                    xrLabel6.Text = "Request for Quotation";
                    xrLabel21.Visible = false;
                    xrLabel2.Visible = false;
                    xrLabel4.Visible = false;
                    xrLabel5.Visible = false;

                    //         SetTextWaterMarkPriceRequest(sender as XtraReport);
                    xrLabel28.Visible = false;
                }
                xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + new NeBusinessUnit(companyid).logo_file;
                var poreqreq = tools.getSQL_string(@"SELECT poprog_ack_req FROM poprog_header  WHERE poprog_id = @v0", new object[] { poprogid.Value.ToString() });
                var poshipreq = tools.getSQL_string(@"SELECT poprog_ship_note_req FROM poprog_header  WHERE poprog_id = @v0", new object[] { poprogid.Value.ToString() });
                if (poreqreq == "1" && tempo.poprog_status > 1)
                {
                    xrLabelNotes.Text += " - Please Send a notice of PO received.";
                }
                if (poshipreq == "1" && tempo.poprog_status > 1)
                {
                    xrLabelNotes.Text += " - Please Send a notice when items shipped.";

                }
                //	xrBarCode1.Text = "004-" + tempo.poprog_id;
                //	xrBarCode2.Text = xrBarCode1.Text;
                /*if (tempo.poprog_status >= 2)
            {
                xrLabel16.Text = "***PO REPRINT ***";
                xrLabel6.Text = "***PO REPRINT ***";
               
            }
            else
            {
              
                

            }*/

            }
            catch
            {
            }
            xrTermsConditions.Text = Resources.quotemaster.terms_conditions;
        }
        private void SetTextWaterMarkPriceRequest(XtraReport report)
        {
            report.Watermark.Text = "PRICE REQUEST";
            report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 40);
            report.Watermark.ForeColor = Color.Black;
            report.Watermark.TextTransparency = 200;
            report.Watermark.ShowBehind = false;
            //report.Watermark.PageRange = "1";

        }
        private void AddTermsConditionsFooter()
        {
            // Initialize the XRLabel for terms_conditions
            xrTermsConditions = new XRLabel
            {
                Name = "xrTermsConditions",
                Text = string.Empty, // Placeholder, will be set dynamically
                BoundsF = new RectangleF(0, 0, 700, 30), // Adjust size and position as needed
                Font = new Font("Arial", 10, FontStyle.Regular),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Add the label to the PageFooter or ReportFooter
            if (PageFooter == null)
            {
                PageFooter = new PageFooterBand
                {
                    HeightF = 50 // Adjust the height as needed
                };
                Bands.Add(PageFooter);
            }

            PageFooter.Controls.Add(xrTermsConditions);
        }

    }
}