using System;
using System.Data;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.WOSignOff;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for WOSOSheet
	/// </summary>
	public class WOSOSheet : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private WOSOData wosoData1;
		private Nesi.Web.Reports.WOSignOff.WOSODataTableAdapters.wo_detail_currentTableAdapter wo_detail_currentTableAdapter1;
		private ReportHeaderBand ReportHeader;
		private GroupHeaderBand GroupHeader1;
		private PageFooterBand PageFooter;
		private XRTable xrTableWODetails;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell2;
		private XRTableCell xrTableCell4;
		private DevExpress.XtraReports.Parameters.Parameter woid;
		private XRPanel xrPanel1;
		private XRPictureBox xrPictureBox2;
		private XRLabel xrLabelWONum;
		private XRLabel xrLabelCustomerName;
		private XRLabel xrLabelAddress;
		private XRLabel xrLabelCity;
		private XRLabel xrLabelPhone;
		private XRControlStyle xrControlStyle1;
		private XRPageInfo xrPageInfoPageNumber;
		private XRPageInfo xrPageInfo1;
		private XRLabel xrLabelFax;
		private XRLabel xrLabelCompphone;
		private XRLabel xrLabelCompFax;
		private XRRichText xrRichTextDescription;
		private XRLabel xrLabel2;
		private XRLabel xrLabel3;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private ReportFooterBand ReportFooter;
		private XRLabel xrLabel12;
		private XRLine xrLine6;
		private XRLine xrLine7;
		private XRLine xrLine5;
		private XRLine xrLine3;
		private XRLine xrLine4;
		private XRLabel xrLabel11;
		private XRRichText xrRichText1;
		private XRLine xrLine13;
		private XRLine xrLine14;
		private XRLine xrLine15;
		private XRLine xrLine12;
		private XRLine xrLine9;
		private XRLine xrLine10;
		private XRLine xrLine11;
		private XRRichText xrRichTextPO;
		private XRLabel xrLabel7;
		private XRLabel xrLabel6;
		private XRLine xrLine8;
		private XRPanel xrPanel2;
		private XRCheckBox xrCheckBox3;
		private XRCheckBox xrCheckBox2;
		private XRCheckBox xrCheckBox1;
		private XRLabel xrLabel1;
		private XRCheckBox xrCheckBoxJSC;
		private XRLabel xrLabel10;
		private XRLabel xrLabel9;
		private XRLine xrLine2;
		private XRLabel xrLabel8;
		private XRLine xrLine1;
		private XRLine xrLine16;
		private XRLine xrLine17;
		private XRLine xrLine18;
		private XRLine xrLine19;
		private XRLine xrLine20;
		private XRLabel xrlbl_workdone;
		private XRLabel xrLabel15;
		private XRLabel xrLabel14;
		private XRLabel xrLabel13;
        private XRPictureBox xrPictureBox1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public WOSOSheet()
			{
			InitializeComponent();
			//
			//  
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
            string resourceFileName = "WOSOSheet.resx";
            System.Resources.ResourceManager resources = global::Resources.WOSOSheet.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTableWODetails = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.wosoData1 = new WOSOData();
            this.wo_detail_currentTableAdapter1 = new Nesi.Web.Reports.WOSignOff.WOSODataTableAdapters.wo_detail_currentTableAdapter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichTextDescription = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabelCompFax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelCompphone = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelFax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelPhone = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelCity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelCustomerName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelWONum = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfoPageNumber = new DevExpress.XtraReports.UI.XRPageInfo();
            this.woid = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrlbl_workdone = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine20 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine19 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine18 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine17 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine16 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLine13 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine14 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine15 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine12 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine11 = new DevExpress.XtraReports.UI.XRLine();
            this.xrRichTextPO = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrCheckBox3 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox2 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCheckBoxJSC = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableWODetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wosoData1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichTextDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichTextPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableWODetails});
            this.Detail.HeightF = 26.7862F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableWODetails
            // 
            this.xrTableWODetails.EvenStyleName = "xrControlStyle1";
            this.xrTableWODetails.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableWODetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTableWODetails.Name = "xrTableWODetails";
            this.xrTableWODetails.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTableWODetails.SizeF = new System.Drawing.SizeF(639.9999F, 25F);
            this.xrTableWODetails.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell1,
            this.xrTableCell2});
            this.xrTableRow1.EvenStyleName = "xrControlStyle1";
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "master_id")});
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 0.33630938310108649D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "description")});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 2.3095244507852124D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "qty_committed")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.354166166113701D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 40.625F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 58.95818F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // wosoData1
            // 
            this.wosoData1.DataSetName = "WOSOData";
            this.wosoData1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // wo_detail_currentTableAdapter1
            // 
            this.wo_detail_currentTableAdapter1.ClearBeforeFill = true;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPanel1});
            this.ReportHeader.HeightF = 253.125F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(540F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrRichTextDescription,
            this.xrLabelCompFax,
            this.xrLabelCompphone,
            this.xrLabelFax,
            this.xrLabelPhone,
            this.xrLabelCity,
            this.xrLabelAddress,
            this.xrLabelCustomerName,
            this.xrLabelWONum,
            this.xrPictureBox2});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 48.95833F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(640F, 204.1667F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 164.0367F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(45.83333F, 18.29002F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Fax: ";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(428.6664F, 127.4567F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(45.83333F, 18.29002F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Fax: ";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(428.6664F, 109.1667F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(45.83333F, 18.29002F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Phone: ";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 145.7468F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(45.83333F, 18.29002F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Phone: ";
            // 
            // xrRichTextDescription
            // 
            this.xrRichTextDescription.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichTextDescription.LocationFloat = new DevExpress.Utils.PointFloat(0F, 30.29165F);
            this.xrRichTextDescription.Name = "xrRichTextDescription";
            this.xrRichTextDescription.SerializableRtfString = resources.GetString("xrRichTextDescription.SerializableRtfString");
            this.xrRichTextDescription.SizeF = new System.Drawing.SizeF(398.9583F, 55.87334F);
            this.xrRichTextDescription.StylePriority.UseFont = false;
            // 
            // xrLabelCompFax
            // 
            this.xrLabelCompFax.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelCompFax.LocationFloat = new DevExpress.Utils.PointFloat(474.4997F, 127.4567F);
            this.xrLabelCompFax.Name = "xrLabelCompFax";
            this.xrLabelCompFax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelCompFax.SizeF = new System.Drawing.SizeF(165.5002F, 18.28999F);
            this.xrLabelCompFax.StylePriority.UseFont = false;
            this.xrLabelCompFax.Text = "xrLabelCompFax";
            // 
            // xrLabelCompphone
            // 
            this.xrLabelCompphone.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelCompphone.LocationFloat = new DevExpress.Utils.PointFloat(474.4997F, 109.1667F);
            this.xrLabelCompphone.Name = "xrLabelCompphone";
            this.xrLabelCompphone.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelCompphone.SizeF = new System.Drawing.SizeF(165.5002F, 18.28999F);
            this.xrLabelCompphone.StylePriority.UseFont = false;
            this.xrLabelCompphone.Text = "xrLabelCompphone";
            // 
            // xrLabelFax
            // 
            this.xrLabelFax.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelFax.LocationFloat = new DevExpress.Utils.PointFloat(45.83329F, 164.0367F);
            this.xrLabelFax.Name = "xrLabelFax";
            this.xrLabelFax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelFax.SizeF = new System.Drawing.SizeF(188.5417F, 18.28998F);
            this.xrLabelFax.StylePriority.UseFont = false;
            this.xrLabelFax.Text = "xrLabelFax";
            // 
            // xrLabelPhone
            // 
            this.xrLabelPhone.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelPhone.LocationFloat = new DevExpress.Utils.PointFloat(45.83329F, 145.7468F);
            this.xrLabelPhone.Name = "xrLabelPhone";
            this.xrLabelPhone.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelPhone.SizeF = new System.Drawing.SizeF(188.5417F, 18.28999F);
            this.xrLabelPhone.StylePriority.UseFont = false;
            this.xrLabelPhone.Text = "xrLabelPhone";
            // 
            // xrLabelCity
            // 
            this.xrLabelCity.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelCity.LocationFloat = new DevExpress.Utils.PointFloat(0F, 127.4568F);
            this.xrLabelCity.Name = "xrLabelCity";
            this.xrLabelCity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelCity.SizeF = new System.Drawing.SizeF(278.125F, 18.29F);
            this.xrLabelCity.StylePriority.UseFont = false;
            this.xrLabelCity.Text = "xrLabelCity";
            // 
            // xrLabelAddress
            // 
            this.xrLabelAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelAddress.LocationFloat = new DevExpress.Utils.PointFloat(0F, 109.165F);
            this.xrLabelAddress.Name = "xrLabelAddress";
            this.xrLabelAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelAddress.SizeF = new System.Drawing.SizeF(278.125F, 18.29173F);
            this.xrLabelAddress.StylePriority.UseFont = false;
            this.xrLabelAddress.StylePriority.UseTextAlignment = false;
            this.xrLabelAddress.Text = "xrLabelAddress";
            this.xrLabelAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabelCustomerName
            // 
            this.xrLabelCustomerName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelCustomerName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 86.16499F);
            this.xrLabelCustomerName.Name = "xrLabelCustomerName";
            this.xrLabelCustomerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelCustomerName.SizeF = new System.Drawing.SizeF(278.125F, 23F);
            this.xrLabelCustomerName.StylePriority.UseFont = false;
            this.xrLabelCustomerName.StylePriority.UseTextAlignment = false;
            this.xrLabelCustomerName.Text = "xrLabelCustomerName";
            this.xrLabelCustomerName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabelWONum
            // 
            this.xrLabelWONum.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelWONum.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelWONum.Name = "xrLabelWONum";
            this.xrLabelWONum.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelWONum.SizeF = new System.Drawing.SizeF(234.375F, 30.29166F);
            this.xrLabelWONum.StylePriority.UseFont = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox2.Image")));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(428.6664F, 0F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(211.3335F, 109.1667F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13});
            this.GroupHeader1.HeightF = 16F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel15
            // 
            this.xrLabel15.BackColor = System.Drawing.Color.DarkGray;
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel15.ForeColor = System.Drawing.Color.White;
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(564.4445F, 0F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(75.55536F, 16F);
            this.xrLabel15.StylePriority.UseBackColor = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseForeColor = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Qty";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel14
            // 
            this.xrLabel14.BackColor = System.Drawing.Color.DarkGray;
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel14.ForeColor = System.Drawing.Color.White;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(71.746F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(492.6985F, 16F);
            this.xrLabel14.StylePriority.UseBackColor = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseForeColor = false;
            this.xrLabel14.Text = "Description";
            // 
            // xrLabel13
            // 
            this.xrLabel13.BackColor = System.Drawing.Color.DarkGray;
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel13.ForeColor = System.Drawing.Color.White;
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(71.74598F, 16F);
            this.xrLabel13.StylePriority.UseBackColor = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseForeColor = false;
            this.xrLabel13.Text = "Part No";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrPageInfoPageNumber});
            this.PageFooter.HeightF = 30.00005F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfoPageNumber
            // 
            this.xrPageInfoPageNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfoPageNumber.LocationFloat = new DevExpress.Utils.PointFloat(306.25F, 0F);
            this.xrPageInfoPageNumber.Name = "xrPageInfoPageNumber";
            this.xrPageInfoPageNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfoPageNumber.SizeF = new System.Drawing.SizeF(30.20834F, 23F);
            this.xrPageInfoPageNumber.StylePriority.UseFont = false;
            // 
            // woid
            // 
            this.woid.Name = "woid";
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlbl_workdone,
            this.xrLine20,
            this.xrLine19,
            this.xrLine18,
            this.xrLine17,
            this.xrLine16,
            this.xrLabel12,
            this.xrLine6,
            this.xrLine7,
            this.xrLine5,
            this.xrLine3,
            this.xrLine4,
            this.xrLabel11,
            this.xrRichText1,
            this.xrLine13,
            this.xrLine14,
            this.xrLine15,
            this.xrLine12,
            this.xrLine9,
            this.xrLine10,
            this.xrLine11,
            this.xrRichTextPO,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLine8,
            this.xrPanel2,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLine2,
            this.xrLabel8,
            this.xrLine1});
            this.ReportFooter.HeightF = 471.9652F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrlbl_workdone
            // 
            this.xrlbl_workdone.CanShrink = true;
            this.xrlbl_workdone.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlbl_workdone.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrlbl_workdone.Multiline = true;
            this.xrlbl_workdone.Name = "xrlbl_workdone";
            this.xrlbl_workdone.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlbl_workdone.SizeF = new System.Drawing.SizeF(639.9999F, 4.666633F);
            this.xrlbl_workdone.StylePriority.UseFont = false;
            this.xrlbl_workdone.Text = "Work Completed";
            // 
            // xrLine20
            // 
            this.xrLine20.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 47.79814F);
            this.xrLine20.Name = "xrLine20";
            this.xrLine20.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine19
            // 
            this.xrLine19.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 60.38143F);
            this.xrLine19.Name = "xrLine19";
            this.xrLine19.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine18
            // 
            this.xrLine18.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 72.96484F);
            this.xrLine18.Name = "xrLine18";
            this.xrLine18.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine17
            // 
            this.xrLine17.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 85.5482F);
            this.xrLine17.Name = "xrLine17";
            this.xrLine17.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine16
            // 
            this.xrLine16.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 98.13158F);
            this.xrLine16.Name = "xrLine16";
            this.xrLine16.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 25.00673F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(130.8333F, 14.66665F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "Other Items Used in Job";
            // 
            // xrLine6
            // 
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 110.7149F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine7
            // 
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 123.2983F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 465.4007F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(80F, 4.250015F);
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(375F, 432.2985F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(150F, 4.250015F);
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(375F, 465.4007F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(150F, 4.250015F);
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(525F, 457.2985F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(37.5F, 14.66669F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "Date";
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(12.50003F, 357.2983F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(630F, 40.70834F);
            this.xrRichText1.StylePriority.UseFont = false;
            // 
            // xrLine13
            // 
            this.xrLine13.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 186.215F);
            this.xrLine13.Name = "xrLine13";
            this.xrLine13.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine14
            // 
            this.xrLine14.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 211.3817F);
            this.xrLine14.Name = "xrLine14";
            this.xrLine14.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine15
            // 
            this.xrLine15.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 223.965F);
            this.xrLine15.Name = "xrLine15";
            this.xrLine15.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine12
            // 
            this.xrLine12.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 198.7984F);
            this.xrLine12.Name = "xrLine12";
            this.xrLine12.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine9
            // 
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 148.465F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine10
            // 
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 161.0484F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrLine11
            // 
            this.xrLine11.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 173.6316F);
            this.xrLine11.Name = "xrLine11";
            this.xrLine11.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrRichTextPO
            // 
            this.xrRichTextPO.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichTextPO.LocationFloat = new DevExpress.Utils.PointFloat(80.62502F, 332.2984F);
            this.xrRichTextPO.Name = "xrRichTextPO";
            this.xrRichTextPO.SerializableRtfString = resources.GetString("xrRichTextPO.SerializableRtfString");
            this.xrRichTextPO.SizeF = new System.Drawing.SizeF(131.875F, 23.00003F);
            this.xrRichTextPO.StylePriority.UseFont = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(12.50003F, 419.7984F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(112.5F, 14.66663F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Employee Signature";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 332.2984F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(80.62496F, 23.00002F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Customer PO:";
            // 
            // xrLine8
            // 
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 135.8816F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(640F, 12.58335F);
            // 
            // xrPanel2
            // 
            this.xrPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox3,
            this.xrCheckBox2,
            this.xrCheckBox1,
            this.xrLabel1,
            this.xrCheckBoxJSC});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(1.051847E-05F, 257.2984F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(639.9999F, 68.75F);
            this.xrPanel2.StylePriority.UseBackColor = false;
            // 
            // xrCheckBox3
            // 
            this.xrCheckBox3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox3.LocationFloat = new DevExpress.Utils.PointFloat(234.375F, 45.74998F);
            this.xrCheckBox3.Name = "xrCheckBox3";
            this.xrCheckBox3.SizeF = new System.Drawing.SizeF(287.9166F, 23.00002F);
            this.xrCheckBox3.StylePriority.UseFont = false;
            this.xrCheckBox3.Text = "No Outstanding Issues Remain";
            // 
            // xrCheckBox2
            // 
            this.xrCheckBox2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox2.LocationFloat = new DevExpress.Utils.PointFloat(234.375F, 18.99999F);
            this.xrCheckBox2.Name = "xrCheckBox2";
            this.xrCheckBox2.SizeF = new System.Drawing.SizeF(287.9166F, 23.00002F);
            this.xrCheckBox2.StylePriority.UseFont = false;
            this.xrCheckBox2.Text = "Technicians were Professional and Courteous";
            // 
            // xrCheckBox1
            // 
            this.xrCheckBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 42.00001F);
            this.xrCheckBox1.Name = "xrCheckBox1";
            this.xrCheckBox1.SizeF = new System.Drawing.SizeF(180.625F, 23.00002F);
            this.xrCheckBox1.StylePriority.UseFont = false;
            this.xrCheckBox1.Text = "Kept Informed of Progress";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(260.4167F, 19F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Check the items below if true";
            // 
            // xrCheckBoxJSC
            // 
            this.xrCheckBoxJSC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCheckBoxJSC.LocationFloat = new DevExpress.Utils.PointFloat(0F, 18.99999F);
            this.xrCheckBoxJSC.Name = "xrCheckBoxJSC";
            this.xrCheckBoxJSC.SizeF = new System.Drawing.SizeF(180.625F, 23.00002F);
            this.xrCheckBoxJSC.StylePriority.UseFont = false;
            this.xrCheckBoxJSC.Text = "Job Site Left Clean and Orderly";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(275F, 457.2985F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(95.83334F, 14.66667F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Customer Name";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(275F, 419.7984F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(95.83334F, 14.66667F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Employee Name";
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(125F, 465.4007F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(150F, 4.250015F);
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(12.50003F, 457.2985F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(112.5F, 14.66663F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Customer Signature";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(125F, 432.2985F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(150F, 4.250015F);
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(589.083F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(50.91681F, 23F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // WOSOSheet
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.PageFooter,
            this.ReportFooter});
            this.DataAdapter = this.wo_detail_currentTableAdapter1;
            this.DataSource = this.wosoData1;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 41, 59);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.woid});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "14.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.woso_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTableWODetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wosoData1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichTextDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichTextPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion
		private void woso_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var wosotableadapter = new Nesi.Web.Reports.WOSignOff.WOSODataTableAdapters.wo_detail_currentTableAdapter();
			wosotableadapter.Fill(wosoData1.wo_detail_current,Convert.ToInt32(woid.Value));

			var _tools = new Toolbox();
			var woproginfo = _tools.getSQL_datatable(@"SELECT a.woprog_bvwo, URLDECODE(a.woprog_customername) AS customername, URLDECODE(a.woprog_description) AS description, a.woprog_custpo, b.Address_Addr1, CONCAT(b.Address_City, ' ', b.Address_Prov, ' ', b.Address_Postal) AS city, CONCAT('(',b.Address_PhoneArea,')',b.Address_PhoneFirst,'-',b.Address_PhoneLast) AS phone, CONCAT('(',b.Address_FaxArea,')',b.Address_FaxFirst,'-',b.Address_FaxLast) AS fax, CONCAT('(',c.PhoneArea,')',c.PhoneFirst,'-',c.PhoneLast) AS compphone, CONCAT('(',c.FaxArea,')',c.FaxFirst,'-',c.FaxLast) AS compfax, c.logo_file FROM woprog AS a LEFT JOIN address AS b on (a.woprog_address_id = b.address_id) LEFT JOIN business_unit AS c on (a.business_unit_id = c.id) WHERE a.woprog_id = @v0 ", new object[] {  woid.Value.ToString() } );

			foreach (DataRow row in woproginfo.Rows)
				{
				xrLabelWONum.Text = row["woprog_bvwo"].ToString();
				xrRichTextDescription.Text = row["description"].ToString();
				xrLabelCustomerName.Text = row["customername"].ToString();
				xrLabelAddress.Text = _tools.value_from(row["Address_Addr1"].ToString());
				xrLabelCity.Text = row["city"].ToString();
				xrLabelPhone.Text = row["phone"].ToString();
				xrLabelFax.Text =   row["fax"].ToString();
				xrLabelCompphone.Text = row["compphone"].ToString();
				xrLabelCompFax.Text =   row["compfax"].ToString();
				    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + row["logo_file"];
				    xrPictureBox2.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + row["logo_file"];
                try
					{
					xrRichTextPO.Text = row["woprog_custpo"].ToString();
					}
				catch { }
				}

			}
		}
	}