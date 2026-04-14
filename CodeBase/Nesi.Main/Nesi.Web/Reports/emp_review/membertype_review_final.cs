using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_review;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for membertype_review_final
	/// </summary>
	public class membertype_review_final : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private XRLabel xrLabel5;
		private XRLabel xrLabel4;
		private XRLabel xrLabel3;
		private XRLabel xrLabel1;
		private DevExpress.XtraReports.Parameters.Parameter rid;
		private GroupHeaderBand GroupHeader1;
		private XRLabel xrLabel2;
		private emp_review_cr_items emp_review_cr_items1;
		private XRLine xrLine1;
		private DevExpress.XtraReports.Parameters.Parameter is_worksheet;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public membertype_review_final()
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
			var resourceFileName = "membertype_review_final.resx";
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.rid = new DevExpress.XtraReports.Parameters.Parameter();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.emp_review_cr_items1 = new emp_review_cr_items();
			this.is_worksheet = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this.emp_review_cr_items1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrLine1,
																						this.xrLabel4,
																						this.xrLabel3,
																						this.xrLabel1});
			this.Detail.HeightF = 39.75F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLine1
			// 
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 34.375F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(896.875F, 5.375004F);
			// 
			// xrLabel4
			// 
			this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.cr_review_question")});
			this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(22.91667F, 0F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(410.4167F, 23F);
			this.xrLabel4.StylePriority.UseFont = false;
			this.xrLabel4.Text = "xrLabel4";
			// 
			// xrLabel3
			// 
			this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.notes")});
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(443.75F, 0F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(341.6667F, 23F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.Text = "xrLabel3";
			this.xrLabel3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel3_BeforePrint);
			// 
			// xrLabel1
			// 
			this.xrLabel1.Borders = (((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
																			| DevExpress.XtraPrinting.BorderSide.Right)
																			| DevExpress.XtraPrinting.BorderSide.Bottom);
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.score")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 14F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(847.9167F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(52.08331F, 29.66667F);
			this.xrLabel1.StylePriority.UseBorders = false;
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "xrLabel1";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.xrLabel1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel1_BeforePrint);
			// 
			// xrLabel5
			// 
			this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.description")});
			this.xrLabel5.Font = new System.Drawing.Font("Arial", 7F);
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 23F);
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(423.3334F, 17.08333F);
			this.xrLabel5.StylePriority.UseFont = false;
			this.xrLabel5.Text = "xrLabel5";
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
			this.BottomMargin.HeightF = 2.083333F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// rid
			// 
			this.rid.Name = "rid";
			this.rid.Type = typeof(int);
			this.rid.ValueInfo = "0";
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrLabel2,
																							this.xrLabel5});
			this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
																								new DevExpress.XtraReports.UI.GroupField("core_responsibility", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			this.GroupHeader1.HeightF = 40.08334F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// xrLabel2
			// 
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.core_responsibility")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(652.0833F, 23F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.Text = "xrLabel2";
			// 
			// emp_review_cr_items1
			// 
			this.emp_review_cr_items1.DataSetName = "emp_review_cr_items";
			this.emp_review_cr_items1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// is_worksheet
			// 
			this.is_worksheet.Name = "is_worksheet";
			// 
			// membertype_review_final
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.GroupHeader1});
			this.DataMember = "DataTable1";
			this.DataSource = this.emp_review_cr_items1;
			this.Landscape = true;
			this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 2);
			this.PageHeight = 850;
			this.PageWidth = 1100;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.rid,
																							this.is_worksheet});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.membertype_review_final_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.emp_review_cr_items1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void xrLabel3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Notes: " + l.Text;
				}
			}

		private void membertype_review_final_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var cr = new Nesi.Web.Reports.emp_review.emp_review_cr_itemsTableAdapters.DataTable1TableAdapter();
			cr.Fill(emp_review_cr_items1.DataTable1, 0,(int)rid.Value,0);
	
			}

		private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			if (is_worksheet.Value.ToString() == "1")
				{
				var l = (XRLabel)sender;
				if (l.Text == "0")
					{
					l.Text = "";
					}
				}
			}
		}
	}