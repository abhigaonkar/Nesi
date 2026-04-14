using DevExpress.XtraReports.UI;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for subcontract_employment
	/// </summary>
	public class subcontract_employment : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin; 
		private ReportHeaderBand ReportHeader;
		private XRPictureBox xrPictureBox1;
		private XRLabel lblheader;
		private XRLabel xrLabel3;
		private XRLabel lblbetween;
		private XRRichText xrRichText2;
		private XRRichText tr_contract_details;
		private XRRichText xrRichText1;
		private XRLabel lbl_subcontractor_address;
		private XRLabel xrLabel5;
		private XRLabel xrLabel4;
		private XRLabel xrLabel2;
		private XRLabel xrLabel1;
		private XRLabel lbl_herinafter;
		private XRLabel lblnecompany;
		private XRRichText xrRichText3;
		private ReportFooterBand ReportFooter;
		private XRRichText xrRichText4;
		private XRPageInfo xrPageInfo1;
		private XRLine xrLine4;
		private XRLine xrLine3;
		private XRLine xrLine2;
		private XRLine xrLine1;
		private XRLabel xrLabel9;
		private XRLabel xrLabel8;
		private XRLabel xrLabel7;
		private XRLabel xrLabel6;
        private XRPictureBox xrPictureBox2;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public subcontract_employment()
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
            string resourceFileName = "subcontract_employment.resx";
            System.Resources.ResourceManager resources = global::Resources.subcontract_employment.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
            this.tr_contract_details = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.lblheader = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lbl_subcontractor_address = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_herinafter = new DevExpress.XtraReports.UI.XRLabel();
            this.lblnecompany = new DevExpress.XtraReports.UI.XRLabel();
            this.lblbetween = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText4 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr_contract_details)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText2,
            this.tr_contract_details,
            this.xrRichText1,
            this.xrRichText3});
            this.Detail.HeightF = 392.0834F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrRichText2
            // 
            this.xrRichText2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xrRichText2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 212.9167F);
            this.xrRichText2.Name = "xrRichText2";
            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
            this.xrRichText2.SizeF = new System.Drawing.SizeF(650F, 89.58334F);
            this.xrRichText2.StylePriority.UseBackColor = false;
            this.xrRichText2.StylePriority.UseFont = false;
            // 
            // tr_contract_details
            // 
            this.tr_contract_details.Font = new System.Drawing.Font("Arial", 9F);
            this.tr_contract_details.LocationFloat = new DevExpress.Utils.PointFloat(0F, 190F);
            this.tr_contract_details.Name = "tr_contract_details";
            this.tr_contract_details.SerializableRtfString = resources.GetString("tr_contract_details.SerializableRtfString");
            this.tr_contract_details.SizeF = new System.Drawing.SizeF(650F, 22.91666F);
            this.tr_contract_details.StylePriority.UseFont = false;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(650F, 190F);
            this.xrRichText1.StylePriority.UseFont = false;
            // 
            // xrRichText3
            // 
            this.xrRichText3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xrRichText3.Font = new System.Drawing.Font("Arial", 9F);
            this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 302.5F);
            this.xrRichText3.Name = "xrRichText3";
            this.xrRichText3.SerializableRtfString = resources.GetString("xrRichText3.SerializableRtfString");
            this.xrRichText3.SizeF = new System.Drawing.SizeF(650F, 89.58334F);
            this.xrRichText3.StylePriority.UseBackColor = false;
            this.xrRichText3.StylePriority.UseFont = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblheader,
            this.xrPictureBox1});
            this.TopMargin.HeightF = 120.8333F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblheader
            // 
            this.lblheader.Font = new System.Drawing.Font("Arial", 8F);
            this.lblheader.LocationFloat = new DevExpress.Utils.PointFloat(198.5419F, 35.41667F);
            this.lblheader.Name = "lblheader";
            this.lblheader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblheader.SizeF = new System.Drawing.SizeF(451.4581F, 23F);
            this.lblheader.StylePriority.UseFont = false;
            this.lblheader.StylePriority.UseTextAlignment = false;
            this.lblheader.Text = "lblheader";
            this.lblheader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/nesi-logo-blue-invert.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(152.0834F, 106.25F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPictureBox2});
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(246.875F, 20.79169F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_subcontractor_address,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel1,
            this.lbl_herinafter,
            this.lblnecompany,
            this.lblbetween,
            this.xrLabel3});
            this.ReportHeader.HeightF = 256.25F;
            this.ReportHeader.KeepTogether = true;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lbl_subcontractor_address
            // 
            this.lbl_subcontractor_address.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbl_subcontractor_address.LocationFloat = new DevExpress.Utils.PointFloat(175F, 221.4167F);
            this.lbl_subcontractor_address.Name = "lbl_subcontractor_address";
            this.lbl_subcontractor_address.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_subcontractor_address.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.lbl_subcontractor_address.StylePriority.UseFont = false;
            this.lbl_subcontractor_address.StylePriority.UseTextAlignment = false;
            this.lbl_subcontractor_address.Text = "[SUBCONTRACTOR ADDRESS AND PHONE]";
            this.lbl_subcontractor_address.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(175F, 202.5833F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "OF";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(175F, 183.75F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "(HEREINAFTER, \"THE CONTRACTOR\")";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(175F, 164.9166F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "lblnecompany";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(175F, 135.4167F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "AND";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lbl_herinafter
            // 
            this.lbl_herinafter.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbl_herinafter.LocationFloat = new DevExpress.Utils.PointFloat(175F, 106.3334F);
            this.lbl_herinafter.Name = "lbl_herinafter";
            this.lbl_herinafter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_herinafter.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.lbl_herinafter.StylePriority.UseFont = false;
            this.lbl_herinafter.StylePriority.UseTextAlignment = false;
            this.lbl_herinafter.Text = "(HEREINAFTER, \"The Company\")";
            this.lbl_herinafter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblnecompany
            // 
            this.lblnecompany.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblnecompany.LocationFloat = new DevExpress.Utils.PointFloat(175F, 87.5F);
            this.lblnecompany.Name = "lblnecompany";
            this.lblnecompany.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblnecompany.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.lblnecompany.StylePriority.UseFont = false;
            this.lblnecompany.StylePriority.UseTextAlignment = false;
            this.lblnecompany.Text = "lblnecompany";
            this.lblnecompany.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblbetween
            // 
            this.lblbetween.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblbetween.LocationFloat = new DevExpress.Utils.PointFloat(0F, 85.41666F);
            this.lblbetween.Name = "lblbetween";
            this.lblbetween.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblbetween.SizeF = new System.Drawing.SizeF(90.625F, 18.83334F);
            this.lblbetween.StylePriority.UseFont = false;
            this.lblbetween.Text = "BETWEEN:";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Gray;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 14F);
            this.xrLabel3.ForeColor = System.Drawing.Color.White;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 38.625F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Contract For Services";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine4,
            this.xrLine3,
            this.xrLine2,
            this.xrLine1,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrRichText4});
            this.ReportFooter.HeightF = 206.25F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(339.3751F, 132.375F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(269.375F, 9.375F);
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 132.375F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(269.375F, 9.375F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 86.45834F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(269.375F, 9.375F);
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(339.375F, 86.45834F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(269.375F, 9.375F);
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(332.7083F, 141.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Date";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(332.7084F, 95.83334F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Date";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 141.75F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "lblnecompany";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 95.83334F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(307.2917F, 18.83334F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "lblnecompany";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrRichText4
            // 
            this.xrRichText4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xrRichText4.Font = new System.Drawing.Font("Arial", 9F);
            this.xrRichText4.KeepTogether = true;
            this.xrRichText4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrRichText4.Name = "xrRichText4";
            this.xrRichText4.SerializableRtfString = resources.GetString("xrRichText4.SerializableRtfString");
            this.xrRichText4.SizeF = new System.Drawing.SizeF(650F, 62.5F);
            this.xrRichText4.StylePriority.UseBackColor = false;
            this.xrRichText4.StylePriority.UseFont = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.ImageUrl = "~\\images\\Logos\\nesi-logo.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(577.2917F, 10.00001F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(62.70831F, 42.70833F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // subcontract_employment
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 121, 100);
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tr_contract_details)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion
		}
	}