using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.WOShoppingCart;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for woshoppingcart
	/// </summary>
	public class woshoppingcart : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin; 
		private XRLabel xrLabel1;
		private WOShoppingCartData woShoppingCartData1;
		private Nesi.Web.Reports.WOShoppingCart.WOShoppingCartDataTableAdapters.woshoppingcartdataTableAdapter woshoppingcartdataTableAdapter1;
		private DevExpress.XtraReports.Parameters.Parameter woid			= new DevExpress.XtraReports.Parameters.Parameter();
		private DevExpress.XtraReports.Parameters.Parameter unfulfilled		= new DevExpress.XtraReports.Parameters.Parameter();
		private XRPictureBox xrPictureBox1;
		private XRBarCode xrBarCode1;
		private XRLabel xrLabel11;
		private XRLabel lblAddress;
		private XRLabel xrLabel2;
		private XRLabel xrLabel3;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRLabel xrLabel6;
		private CalculatedField qty_to_pull;
		private XRLabel xrLabel14;
		private XRLine xrLine1;
		private XRShape xrShape1;
		private XRPageInfo xrPageInfo1;
		private CalculatedField masterid_barcode;
		private XRPictureBox xrPictureBox2;
		private XRPictureBox xrPictureBox3;
		private PageHeaderBand PageHeader;
		private XRBarCode xrBarCode3;
		private XRLabel xrLabel15;
		private ReportHeaderBand ReportHeader;
		private XRPanel xrPanel1;
		private XRLabel xrLabel16;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8;
		private XRLabel xrLabel9;
		private XRLabel xrLabel10;
		private XRLabel xrLabel12;
		private XRLabel xrLabel13;
		private XRPanel xrPanel2;
		private XRLabel xrLabel17;
		private XRLabel xrLabel18;
		private XRLabel xrLabel19;
		private XRLabel xrLabel20;
		private XRLabel xrLabel21;
		private XRLabel xrLabel22;
		private XRLabel xrLabel23;
		private XRLabel lblDescription;
		private XRBarCode xrBarCode2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public woshoppingcart(int woprogid)
			{
			InitializeComponent();
			//
			//  
			//
			var _tools = new Toolbox();
			xrBarCode1.Symbology.CalcCheckSum = false;
			var wo = new NeWOProg(Convert.ToInt32(woprogid));
			var add = new NEAddress(wo.woprog_Address_ID);
			

			var addfull = add.Addr1 + "," + add.Addr2 + "," + add.Addr3 + "," + add.City + "," + add.Prov + "," + NeCountry.get_name(add.Country);
			lblAddress.Text = wo.CustomerName + Environment.NewLine + add.Addr1 + ", ";
			if (add.Addr2 != "")
				{
				lblAddress.Text += add.Addr2 + ", " + Environment.NewLine;
				}
			if (add.Addr3 != "")
				{
				lblAddress.Text += add.Addr3 + ", ";
				}
			lblAddress.Text += add.City + "," + Environment.NewLine + add.Prov + "," + NeCountry.get_name(add.Country);
        
			xrLabel11.Text = wo.OrderNumber;
			xrLabel15.Text = xrLabel11.Text;

			lblDescription.Text = _tools.value_from(wo.Description) + Environment.NewLine + "PM: " + wo.strProjectManager + Environment.NewLine + "Start Date: " + wo.woprog_Expected_StartDate.ToShortDateString();

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
			var resources = global::Resources.woshoppingcart.ResourceManager;
			var code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
			var shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
			var code39Generator1 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
			var code39ExtendedGenerator1 = new DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
			this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
			this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.lblAddress = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
			this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
			this.woShoppingCartData1 = new WOShoppingCartData();
			this.woshoppingcartdataTableAdapter1 = new Nesi.Web.Reports.WOShoppingCart.WOShoppingCartDataTableAdapters.woshoppingcartdataTableAdapter();
			this.woid = new DevExpress.XtraReports.Parameters.Parameter();
			this.qty_to_pull = new DevExpress.XtraReports.UI.CalculatedField();
			this.masterid_barcode = new DevExpress.XtraReports.UI.CalculatedField();
			this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
			this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
			this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrBarCode3 = new DevExpress.XtraReports.UI.XRBarCode();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.lblDescription = new DevExpress.XtraReports.UI.XRLabel();
			this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
			this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
			((System.ComponentModel.ISupportInitialize)(this.woShoppingCartData1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrBarCode2,
																						this.xrShape1,
																						this.xrLabel14,
																						this.xrLabel6,
																						this.xrLabel5,
																						this.xrLabel4,
																						this.xrLabel3,
																						this.xrLabel2,
																						this.xrLabel1,
																						this.xrLine1});
			this.Detail.Font = new System.Drawing.Font("Arial", 9F);
			this.Detail.HeightF = 54.50003F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.StylePriority.UseFont = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrBarCode2
			// 
			this.xrBarCode2.AutoModule = true;
			this.xrBarCode2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.masterid_barcode")});
			this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrBarCode2.Name = "xrBarCode2";
			this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
			this.xrBarCode2.ShowText = false;
			this.xrBarCode2.SizeF = new System.Drawing.SizeF(121.0833F, 44.50003F);
			code128Generator1.CharacterSet = DevExpress.XtraPrinting.BarCode.Code128Charset.CharsetAuto;
			this.xrBarCode2.Symbology = code128Generator1;
			this.xrBarCode2.Text = "xrBarCode2";
			// 
			// xrShape1
			// 
			this.xrShape1.ForeColor = System.Drawing.Color.DimGray;
			this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(638.2916F, 0F);
			this.xrShape1.Name = "xrShape1";
			shapeRectangle1.Fillet = 2;
			this.xrShape1.Shape = shapeRectangle1;
			this.xrShape1.SizeF = new System.Drawing.SizeF(47.70837F, 45.99997F);
			// 
			// xrLabel14
			// 
			this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.qty_to_pull")});
			this.xrLabel14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(559.8333F, 2.000046F);
			this.xrLabel14.Name = "xrLabel14";
			this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel14.SizeF = new System.Drawing.SizeF(78.12497F, 22.99999F);
			this.xrLabel14.StylePriority.UseFont = false;
			this.xrLabel14.StylePriority.UseTextAlignment = false;
			this.xrLabel14.Text = "xrLabel14";
			this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel6
			// 
			this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.Location")});
			this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(178.8333F, 2.000046F);
			this.xrLabel6.Name = "xrLabel6";
			this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
			this.xrLabel6.Text = "xrLabel6";
			// 
			// xrLabel5
			// 
			this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.QtyinStock")});
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(278.8333F, 2.000046F);
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(63.29156F, 22.99998F);
			this.xrLabel5.StylePriority.UseTextAlignment = false;
			this.xrLabel5.Text = "xrLabel5";
			this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel4
			// 
			this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.wo_detail_current_qty_committed")});
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(469.2083F, 2.000046F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(78.125F, 23F);
			this.xrLabel4.StylePriority.UseTextAlignment = false;
			this.xrLabel4.Text = "xrLabel4";
			this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel3
			// 
			this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.wo_detail_current_qty_ordered")});
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(372.5833F, 2.000046F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(72.91666F, 23F);
			this.xrLabel3.StylePriority.UseTextAlignment = false;
			this.xrLabel3.Text = "xrLabel3";
			this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel2
			// 
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.wo_detail_current_master_id")});
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(121.0833F, 2.000046F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(57.74998F, 23F);
			this.xrLabel2.Text = "xrLabel2";
			// 
			// xrLabel1
			// 
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "woshoppingcartdata.description")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 8F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(121.0833F, 25.00006F);
			this.xrLabel1.Multiline = true;
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(516.875F, 20.99991F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
			// 
			// xrLine1
			// 
			this.xrLine1.ForeColor = System.Drawing.Color.Gainsboro;
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52.50003F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(685.9999F, 2F);
			this.xrLine1.StylePriority.UseForeColor = false;
			// 
			// TopMargin
			// 
			this.TopMargin.Font = new System.Drawing.Font("Arial", 11F);
			this.TopMargin.HeightF = 45F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.StylePriority.UseFont = false;
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// lblAddress
			// 
			this.lblAddress.LocationFloat = new DevExpress.Utils.PointFloat(384.6666F, 133.5F);
			this.lblAddress.Multiline = true;
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblAddress.SizeF = new System.Drawing.SizeF(307.3334F, 71.95831F);
			this.lblAddress.StylePriority.UseTextAlignment = false;
			this.lblAddress.Text = "lblAddress";
			this.lblAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel11
			// 
			this.xrLabel11.Font = new System.Drawing.Font("Arial", 18F);
			this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(426.3334F, 76.04166F);
			this.xrLabel11.Name = "xrLabel11";
			this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel11.SizeF = new System.Drawing.SizeF(265.6667F, 45.99998F);
			this.xrLabel11.StylePriority.UseFont = false;
			this.xrLabel11.StylePriority.UseTextAlignment = false;
			this.xrLabel11.Text = "xrLabel11";
			this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrBarCode1
			// 
			this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.xrBarCode1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(340.75F, 0F);
			this.xrBarCode1.Module = 1F;
			this.xrBarCode1.Name = "xrBarCode1";
			this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
			this.xrBarCode1.SizeF = new System.Drawing.SizeF(355.25F, 62.5F);
			this.xrBarCode1.StylePriority.UseFont = false;
			this.xrBarCode1.StylePriority.UseTextAlignment = false;
			code39Generator1.WideNarrowRatio = 3F;
			this.xrBarCode1.Symbology = code39Generator1;
			this.xrBarCode1.Text = "xrBarCode1";
			this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
			this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1.000007F, 0F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new System.Drawing.SizeF(171.875F, 85.41667F);
			this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
			// 
			// BottomMargin
			// 
			this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrPictureBox3,
																							this.xrPictureBox2,
																							this.xrPageInfo1});
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPictureBox3
			// 
			this.xrPictureBox3.ImageUrl = "~\\images\\IconsButtons\\32px-Crystal_Clear_action_ShoppingCart.png";
			this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(602.7499F, 0F);
			this.xrPictureBox3.Name = "xrPictureBox3";
			this.xrPictureBox3.SizeF = new System.Drawing.SizeF(47.29175F, 43.79168F);
			// 
			// xrPictureBox2
			// 
			this.xrPictureBox2.ImageUrl = "~\\images\\IconsButtons\\32px-Crystal_Clear_action_ShoppingCart.png";
			this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrPictureBox2.Name = "xrPictureBox2";
			this.xrPictureBox2.SizeF = new System.Drawing.SizeF(57.29167F, 43.79168F);
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(287.75F, 20.79169F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
			this.xrPageInfo1.StylePriority.UseTextAlignment = false;
			this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// woShoppingCartData1
			// 
			this.woShoppingCartData1.DataSetName = "WOShoppingCartData";
			this.woShoppingCartData1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// woshoppingcartdataTableAdapter1
			// 
			this.woshoppingcartdataTableAdapter1.ClearBeforeFill = true;
			// 
			// woid
			// 
			this.woid.Name = "woid";
			// 
			// qty_to_pull
			// 
			this.qty_to_pull.DataMember = "woshoppingcartdata";
			this.qty_to_pull.Expression = "[wo_detail_current_qty_ordered] - [wo_detail_current_qty_committed]";
			this.qty_to_pull.Name = "qty_to_pull";
			// 
			// masterid_barcode
			// 
			this.masterid_barcode.DataMember = "woshoppingcartdata";
			this.masterid_barcode.Expression = "Concat(\'001-\',[wo_detail_current_master_id] )";
			this.masterid_barcode.FieldType = DevExpress.XtraReports.UI.FieldType.String;
			this.masterid_barcode.Name = "masterid_barcode";
			// 
			// PageHeader
			// 
			this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrPanel2,
																							this.xrLabel15,
																							this.xrBarCode3});
			this.PageHeader.HeightF = 77.08334F;
			this.PageHeader.Name = "PageHeader";
			this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
			// 
			// xrPanel2
			// 
			this.xrPanel2.BackColor = System.Drawing.Color.DarkGray;
			this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrLabel17,
																						this.xrLabel18,
																						this.xrLabel19,
																						this.xrLabel20,
																						this.xrLabel21,
																						this.xrLabel22,
																						this.xrLabel23});
			this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(1.000007F, 45.99997F);
			this.xrPanel2.Name = "xrPanel2";
			this.xrPanel2.SizeF = new System.Drawing.SizeF(692F, 24.125F);
			this.xrPanel2.StylePriority.UseBackColor = false;
			// 
			// xrLabel17
			// 
			this.xrLabel17.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel17.ForeColor = System.Drawing.Color.White;
			this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(638.2917F, 4.666646F);
			this.xrLabel17.Name = "xrLabel17";
			this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel17.SizeF = new System.Drawing.SizeF(47.70831F, 16.75F);
			this.xrLabel17.StylePriority.UseFont = false;
			this.xrLabel17.StylePriority.UseForeColor = false;
			this.xrLabel17.StylePriority.UseTextAlignment = false;
			this.xrLabel17.Text = "Pulled";
			this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrLabel18
			// 
			this.xrLabel18.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel18.ForeColor = System.Drawing.Color.White;
			this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(119.4583F, 4.666646F);
			this.xrLabel18.Name = "xrLabel18";
			this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel18.SizeF = new System.Drawing.SizeF(59.375F, 16.75F);
			this.xrLabel18.StylePriority.UseFont = false;
			this.xrLabel18.StylePriority.UseForeColor = false;
			this.xrLabel18.Text = "Master ID";
			// 
			// xrLabel19
			// 
			this.xrLabel19.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel19.ForeColor = System.Drawing.Color.White;
			this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(178.8333F, 4.666645F);
			this.xrLabel19.Name = "xrLabel19";
			this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel19.SizeF = new System.Drawing.SizeF(86.83334F, 16.75F);
			this.xrLabel19.StylePriority.UseFont = false;
			this.xrLabel19.StylePriority.UseForeColor = false;
			this.xrLabel19.Text = "Location";
			// 
			// xrLabel20
			// 
			this.xrLabel20.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel20.ForeColor = System.Drawing.Color.White;
			this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(277.8333F, 4.333369F);
			this.xrLabel20.Name = "xrLabel20";
			this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel20.SizeF = new System.Drawing.SizeF(63.29156F, 16.75F);
			this.xrLabel20.StylePriority.UseFont = false;
			this.xrLabel20.StylePriority.UseForeColor = false;
			this.xrLabel20.StylePriority.UseTextAlignment = false;
			this.xrLabel20.Text = "Stock";
			this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel21
			// 
			this.xrLabel21.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel21.ForeColor = System.Drawing.Color.White;
			this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(372.5833F, 4.666646F);
			this.xrLabel21.Name = "xrLabel21";
			this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel21.SizeF = new System.Drawing.SizeF(72.91666F, 16.75F);
			this.xrLabel21.StylePriority.UseFont = false;
			this.xrLabel21.StylePriority.UseForeColor = false;
			this.xrLabel21.StylePriority.UseTextAlignment = false;
			this.xrLabel21.Text = "Required";
			this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel22
			// 
			this.xrLabel22.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel22.ForeColor = System.Drawing.Color.White;
			this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(469.2084F, 4.666646F);
			this.xrLabel22.Name = "xrLabel22";
			this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel22.SizeF = new System.Drawing.SizeF(78.125F, 16.75F);
			this.xrLabel22.StylePriority.UseFont = false;
			this.xrLabel22.StylePriority.UseForeColor = false;
			this.xrLabel22.StylePriority.UseTextAlignment = false;
			this.xrLabel22.Text = "Committed";
			this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel23
			// 
			this.xrLabel23.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel23.ForeColor = System.Drawing.Color.White;
			this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(559.8333F, 4.666646F);
			this.xrLabel23.Name = "xrLabel23";
			this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel23.SizeF = new System.Drawing.SizeF(78.125F, 16.75F);
			this.xrLabel23.StylePriority.UseFont = false;
			this.xrLabel23.StylePriority.UseForeColor = false;
			this.xrLabel23.StylePriority.UseTextAlignment = false;
			this.xrLabel23.Text = "to Pull";
			this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrLabel15
			// 
			this.xrLabel15.Font = new System.Drawing.Font("Arial", 18F);
			this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(1.000007F, 0F);
			this.xrLabel15.Name = "xrLabel15";
			this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel15.SizeF = new System.Drawing.SizeF(265.6667F, 45.99998F);
			this.xrLabel15.StylePriority.UseFont = false;
			this.xrLabel15.StylePriority.UseTextAlignment = false;
			this.xrLabel15.Text = "xrLabel11";
			this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrBarCode3
			// 
			this.xrBarCode3.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.xrBarCode3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.xrBarCode3.LocationFloat = new DevExpress.Utils.PointFloat(336.75F, 0F);
			this.xrBarCode3.Module = 1F;
			this.xrBarCode3.Name = "xrBarCode3";
			this.xrBarCode3.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
			this.xrBarCode3.ShowText = false;
			this.xrBarCode3.SizeF = new System.Drawing.SizeF(355.25F, 45.99998F);
			this.xrBarCode3.StylePriority.UseFont = false;
			this.xrBarCode3.StylePriority.UseTextAlignment = false;
			code39ExtendedGenerator1.WideNarrowRatio = 3F;
			this.xrBarCode3.Symbology = code39ExtendedGenerator1;
			this.xrBarCode3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.lblDescription,
																							this.xrPanel1,
																							this.xrPictureBox1,
																							this.xrBarCode1,
																							this.lblAddress,
																							this.xrLabel11});
			this.ReportHeader.HeightF = 244.7917F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// lblDescription
			// 
			this.lblDescription.LocationFloat = new DevExpress.Utils.PointFloat(1.000007F, 133.5F);
			this.lblDescription.Multiline = true;
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblDescription.SizeF = new System.Drawing.SizeF(307.3334F, 71.95831F);
			this.lblDescription.StylePriority.UseTextAlignment = false;
			this.lblDescription.Text = "lblAddress";
			this.lblDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPanel1
			// 
			this.xrPanel1.BackColor = System.Drawing.Color.DarkGray;
			this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrLabel16,
																						this.xrLabel7,
																						this.xrLabel8,
																						this.xrLabel9,
																						this.xrLabel10,
																						this.xrLabel12,
																						this.xrLabel13});
			this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(3.958321F, 210.6667F);
			this.xrPanel1.Name = "xrPanel1";
			this.xrPanel1.SizeF = new System.Drawing.SizeF(688.0417F, 24.125F);
			this.xrPanel1.StylePriority.UseBackColor = false;
			// 
			// xrLabel16
			// 
			this.xrLabel16.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel16.ForeColor = System.Drawing.Color.White;
			this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(634.3334F, 4.666583F);
			this.xrLabel16.Name = "xrLabel16";
			this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel16.SizeF = new System.Drawing.SizeF(47.70831F, 16.75F);
			this.xrLabel16.StylePriority.UseFont = false;
			this.xrLabel16.StylePriority.UseForeColor = false;
			this.xrLabel16.StylePriority.UseTextAlignment = false;
			this.xrLabel16.Text = "Pulled";
			this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrLabel7
			// 
			this.xrLabel7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel7.ForeColor = System.Drawing.Color.White;
			this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(117.125F, 4.666583F);
			this.xrLabel7.Name = "xrLabel7";
			this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel7.SizeF = new System.Drawing.SizeF(57.75002F, 16.75F);
			this.xrLabel7.StylePriority.UseFont = false;
			this.xrLabel7.StylePriority.UseForeColor = false;
			this.xrLabel7.Text = "Master ID";
			// 
			// xrLabel8
			// 
			this.xrLabel8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel8.ForeColor = System.Drawing.Color.White;
			this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(174.875F, 4.666583F);
			this.xrLabel8.Name = "xrLabel8";
			this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 16.75F);
			this.xrLabel8.StylePriority.UseFont = false;
			this.xrLabel8.StylePriority.UseForeColor = false;
			this.xrLabel8.Text = "Location";
			// 
			// xrLabel9
			// 
			this.xrLabel9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel9.ForeColor = System.Drawing.Color.White;
			this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(274.875F, 4.666583F);
			this.xrLabel9.Name = "xrLabel9";
			this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel9.SizeF = new System.Drawing.SizeF(63.29156F, 16.75F);
			this.xrLabel9.StylePriority.UseFont = false;
			this.xrLabel9.StylePriority.UseForeColor = false;
			this.xrLabel9.StylePriority.UseTextAlignment = false;
			this.xrLabel9.Text = "Stock";
			this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel10
			// 
			this.xrLabel10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel10.ForeColor = System.Drawing.Color.White;
			this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(368.625F, 4.666646F);
			this.xrLabel10.Name = "xrLabel10";
			this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel10.SizeF = new System.Drawing.SizeF(72.91666F, 16.75F);
			this.xrLabel10.StylePriority.UseFont = false;
			this.xrLabel10.StylePriority.UseForeColor = false;
			this.xrLabel10.StylePriority.UseTextAlignment = false;
			this.xrLabel10.Text = "Required";
			this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel12
			// 
			this.xrLabel12.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel12.ForeColor = System.Drawing.Color.White;
			this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(465.2501F, 4.666583F);
			this.xrLabel12.Name = "xrLabel12";
			this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel12.SizeF = new System.Drawing.SizeF(78.125F, 16.75F);
			this.xrLabel12.StylePriority.UseFont = false;
			this.xrLabel12.StylePriority.UseForeColor = false;
			this.xrLabel12.StylePriority.UseTextAlignment = false;
			this.xrLabel12.Text = "Committed";
			this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel13
			// 
			this.xrLabel13.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
			this.xrLabel13.ForeColor = System.Drawing.Color.White;
			this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(555.8749F, 4.666583F);
			this.xrLabel13.Name = "xrLabel13";
			this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel13.SizeF = new System.Drawing.SizeF(78.125F, 16.75F);
			this.xrLabel13.StylePriority.UseFont = false;
			this.xrLabel13.StylePriority.UseForeColor = false;
			this.xrLabel13.StylePriority.UseTextAlignment = false;
			this.xrLabel13.Text = "to Pull";
			this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// woshoppingcart
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.PageHeader,
																		this.ReportHeader});
			this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
																								this.qty_to_pull,
																								this.masterid_barcode});
			this.DataAdapter = this.woshoppingcartdataTableAdapter1;
			this.DataMember = "woshoppingcartdata";
			this.DataSource = this.woShoppingCartData1;
			this.Font = new System.Drawing.Font("Arial", 11F);
			this.Margins = new System.Drawing.Printing.Margins(73, 81, 45, 100);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.woid, this.unfulfilled});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.woshoppingcart_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.woShoppingCartData1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void woshoppingcart_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			//  WOSODataTableAdapters.wo_detail_currentTableAdapter wosotableadapter = new WOSODataTableAdapters.wo_detail_currentTableAdapter();
			//   wosotableadapter.Fill(this.wosoData1.wo_detail_current, Convert.ToInt32(this.woid.Value));
			var wosctableadapter = new Nesi.Web.Reports.WOShoppingCart.WOShoppingCartDataTableAdapters.woshoppingcartdataTableAdapter();
			wosctableadapter.Fill(woShoppingCartData1.woshoppingcartdata, Convert.ToInt32(woid.Value), Convert.ToInt16(unfulfilled.Value));
			xrBarCode1.Text = "002-" + woid.Value;
			xrBarCode3.Text = xrBarCode1.Text;
			}
		}
	}