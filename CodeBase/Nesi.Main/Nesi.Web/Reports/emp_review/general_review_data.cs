using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_review;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for general_review_data
	/// </summary>
	public class general_review_data : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private emp_review_general_items emp_review_general_items1;
		private XRLabel xrLabel3;
		private XRLabel xrLabel2;
		private XRLabel xrLabel1;
		private DevExpress.XtraReports.Parameters.Parameter rid;
		private XRShape xrShape1;
		private XRCheckBox xrCheckBox5;
		private XRCheckBox xrCheckBox4;
		private XRCheckBox xrCheckBox3;
		private XRCheckBox xrCheckBox2;
		private XRCheckBox xrCheckBox1;
		private GroupHeaderBand GroupHeader2;
		private XRLine xrLine1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public general_review_data()
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
			var shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
			this.xrCheckBox5 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox4 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox3 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox2 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.emp_review_general_items1 = new emp_review_general_items();
			this.rid = new DevExpress.XtraReports.Parameters.Parameter();
			this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			((System.ComponentModel.ISupportInitialize)(this.emp_review_general_items1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrShape1,
																						this.xrCheckBox5,
																						this.xrCheckBox4,
																						this.xrCheckBox3,
																						this.xrCheckBox2,
																						this.xrCheckBox1,
																						this.xrLabel3});
			this.Detail.HeightF = 46.41663F;
			this.Detail.KeepTogether = true;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.StylePriority.UseBorders = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrShape1
			// 
			this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(662.5F, 5.208333F);
			this.xrShape1.Name = "xrShape1";
			this.xrShape1.Shape = shapeRectangle1;
			this.xrShape1.SizeF = new System.Drawing.SizeF(235.4167F, 37.04164F);
			// 
			// xrCheckBox5
			// 
			this.xrCheckBox5.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox5.LocationFloat = new DevExpress.Utils.PointFloat(619.1666F, 2.083333F);
			this.xrCheckBox5.Name = "xrCheckBox5";
			this.xrCheckBox5.SizeF = new System.Drawing.SizeF(29.16669F, 20.91667F);
			this.xrCheckBox5.StylePriority.UseFont = false;
			this.xrCheckBox5.Text = "5";
			// 
			// xrCheckBox4
			// 
			this.xrCheckBox4.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox4.LocationFloat = new DevExpress.Utils.PointFloat(568.75F, 2.083333F);
			this.xrCheckBox4.Name = "xrCheckBox4";
			this.xrCheckBox4.SizeF = new System.Drawing.SizeF(29.16675F, 20.91667F);
			this.xrCheckBox4.StylePriority.UseFont = false;
			this.xrCheckBox4.Text = "4";
			// 
			// xrCheckBox3
			// 
			this.xrCheckBox3.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox3.LocationFloat = new DevExpress.Utils.PointFloat(514.5832F, 2.083333F);
			this.xrCheckBox3.Name = "xrCheckBox3";
			this.xrCheckBox3.SizeF = new System.Drawing.SizeF(29.16681F, 20.91667F);
			this.xrCheckBox3.StylePriority.UseFont = false;
			this.xrCheckBox3.Text = "3";
			// 
			// xrCheckBox2
			// 
			this.xrCheckBox2.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox2.LocationFloat = new DevExpress.Utils.PointFloat(460.4167F, 2.083333F);
			this.xrCheckBox2.Name = "xrCheckBox2";
			this.xrCheckBox2.SizeF = new System.Drawing.SizeF(29.16672F, 20.91667F);
			this.xrCheckBox2.StylePriority.UseFont = false;
			this.xrCheckBox2.Text = "2";
			// 
			// xrCheckBox1
			// 
			this.xrCheckBox1.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(406.25F, 2.083333F);
			this.xrCheckBox1.Name = "xrCheckBox1";
			this.xrCheckBox1.SizeF = new System.Drawing.SizeF(29.16672F, 20.91667F);
			this.xrCheckBox1.StylePriority.UseFont = false;
			this.xrCheckBox1.Text = "1";
			// 
			// xrLabel3
			// 
			this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.item")});
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(23.54167F, 0F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(345.2083F, 42.24997F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.Text = "xrLabel3";
			// 
			// xrLabel2
			// 
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.description")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 8F);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 27.66665F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(370.8333F, 23F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.Text = "xrLabel2";
			// 
			// xrLabel1
			// 
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.group")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.666646F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(464.5833F, 23F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
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
			this.BottomMargin.HeightF = 4.041735F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// emp_review_general_items1
			// 
			this.emp_review_general_items1.DataSetName = "emp_review_general_items";
			this.emp_review_general_items1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// rid
			// 
			this.rid.Description = "reviewid";
			this.rid.Name = "rid";
			this.rid.Type = typeof(int);
			this.rid.ValueInfo = "0";
			// 
			// GroupHeader2
			// 
			this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrLine1,
																							this.xrLabel1,
																							this.xrLabel2});
			this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
																								new DevExpress.XtraReports.UI.GroupField("group", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			this.GroupHeader2.HeightF = 52.58334F;
			this.GroupHeader2.Name = "GroupHeader2";
			// 
			// xrLine1
			// 
			this.xrLine1.BorderColor = System.Drawing.Color.Gray;
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(895.8334F, 2.083333F);
			this.xrLine1.StylePriority.UseBorderColor = false;
			// 
			// general_review_data
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.GroupHeader2});
			this.DataMember = "DataTable1";
			this.DataSource = this.emp_review_general_items1;
			this.Landscape = true;
			this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 4);
			this.PageHeight = 850;
			this.PageWidth = 1100;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.rid});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.general_review_data_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.emp_review_general_items1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void general_review_data_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var gen = new Nesi.Web.Reports.emp_review.emp_review_general_itemsTableAdapters.DataTable1TableAdapter();
			gen.Fill(emp_review_general_items1.DataTable1,(int)rid.Value);
		
//		dt_emp_offer_cr_dtTableAdapters.emp_offer_crTableAdapter cr_tableadapater = new dt_emp_offer_cr_dtTableAdapters.emp_offer_crTableAdapter();
//		cr_tableadapater.Fill(this.dt_emp_offer_cr_dt1.emp_offer_cr, Convert.ToInt32(this.moid.Value));
			}
		}
	}