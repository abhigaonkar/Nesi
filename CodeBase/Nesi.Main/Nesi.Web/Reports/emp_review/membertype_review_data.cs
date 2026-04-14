using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_review;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for membertype_review_data
	/// </summary>
	public class membertype_review_data : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private emp_review_cr_items emp_review_cr_items1;
		private XRLabel xrLabel2;
		private GroupHeaderBand GroupHeader1;
		private XRLine xrLine1;
		private XRLabel xrLabel1;
		private XRShape xrShape1;
		private XRCheckBox xrCheckBox4;
		private XRCheckBox xrCheckBox1;
		private XRCheckBox xrCheckBox3;
		private XRCheckBox xrCheckBox5;
		private XRCheckBox xrCheckBox2;
		private XRLabel xrLabel3;
		private DevExpress.XtraReports.Parameters.Parameter rid;
		private DevExpress.XtraReports.Parameters.Parameter mid;
		private DevExpress.XtraReports.Parameters.Parameter mtid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public membertype_review_data()
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
			this.xrCheckBox4 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox3 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox5 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrCheckBox2 = new DevExpress.XtraReports.UI.XRCheckBox();
			this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.emp_review_cr_items1 = new emp_review_cr_items();
			this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
			this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.rid = new DevExpress.XtraReports.Parameters.Parameter();
			this.mid = new DevExpress.XtraReports.Parameters.Parameter();
			this.mtid = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this.emp_review_cr_items1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrCheckBox4,
																						this.xrCheckBox1,
																						this.xrCheckBox3,
																						this.xrCheckBox5,
																						this.xrCheckBox2,
																						this.xrShape1,
																						this.xrLabel2});
			this.Detail.HeightF = 56.25F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrCheckBox4
			// 
			this.xrCheckBox4.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox4.LocationFloat = new DevExpress.Utils.PointFloat(570.6251F, 2.083333F);
			this.xrCheckBox4.Name = "xrCheckBox4";
			this.xrCheckBox4.SizeF = new System.Drawing.SizeF(29.16675F, 20.91667F);
			this.xrCheckBox4.StylePriority.UseFont = false;
			this.xrCheckBox4.Text = "4";
			// 
			// xrCheckBox1
			// 
			this.xrCheckBox1.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(408.1251F, 2.083333F);
			this.xrCheckBox1.Name = "xrCheckBox1";
			this.xrCheckBox1.SizeF = new System.Drawing.SizeF(29.16672F, 20.91667F);
			this.xrCheckBox1.StylePriority.UseFont = false;
			this.xrCheckBox1.Text = "1";
			// 
			// xrCheckBox3
			// 
			this.xrCheckBox3.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox3.LocationFloat = new DevExpress.Utils.PointFloat(516.4583F, 2.083333F);
			this.xrCheckBox3.Name = "xrCheckBox3";
			this.xrCheckBox3.SizeF = new System.Drawing.SizeF(29.16681F, 20.91667F);
			this.xrCheckBox3.StylePriority.UseFont = false;
			this.xrCheckBox3.Text = "3";
			// 
			// xrCheckBox5
			// 
			this.xrCheckBox5.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox5.LocationFloat = new DevExpress.Utils.PointFloat(621.0416F, 2.083333F);
			this.xrCheckBox5.Name = "xrCheckBox5";
			this.xrCheckBox5.SizeF = new System.Drawing.SizeF(29.16669F, 20.91667F);
			this.xrCheckBox5.StylePriority.UseFont = false;
			this.xrCheckBox5.Text = "5";
			// 
			// xrCheckBox2
			// 
			this.xrCheckBox2.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrCheckBox2.LocationFloat = new DevExpress.Utils.PointFloat(462.2917F, 2.083333F);
			this.xrCheckBox2.Name = "xrCheckBox2";
			this.xrCheckBox2.SizeF = new System.Drawing.SizeF(29.16672F, 20.91667F);
			this.xrCheckBox2.StylePriority.UseFont = false;
			this.xrCheckBox2.Text = "2";
			// 
			// xrShape1
			// 
			this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(660.4167F, 4.166667F);
			this.xrShape1.Name = "xrShape1";
			this.xrShape1.Shape = shapeRectangle1;
			this.xrShape1.SizeF = new System.Drawing.SizeF(235.4167F, 37.04164F);
			// 
			// xrLabel2
			// 
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.cr_review_question")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(20.41667F, 0F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(356.6667F, 45.91668F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.Text = "xrLabel2";
			// 
			// xrLabel3
			// 
			this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.description")});
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 7F);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 39.99999F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(293.75F, 13F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.Text = "xrLabel3";
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
			this.BottomMargin.HeightF = 0F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// emp_review_cr_items1
			// 
			this.emp_review_cr_items1.DataSetName = "emp_review_cr_items";
			this.emp_review_cr_items1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrLine1,
																							this.xrLabel1,
																							this.xrLabel3});
			this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
																								new DevExpress.XtraReports.UI.GroupField("core_responsibility", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
			this.GroupHeader1.HeightF = 54.66665F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// xrLine1
			// 
			this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5.208333F);
			this.xrLine1.Name = "xrLine1";
			this.xrLine1.SizeF = new System.Drawing.SizeF(895.8333F, 8.333334F);
			// 
			// xrLabel1
			// 
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "DataTable1.core_responsibility")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 16.99999F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(689.5833F, 23F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
			// 
			// rid
			// 
			this.rid.Name = "rid";
			this.rid.Type = typeof(int);
			this.rid.ValueInfo = "0";
			// 
			// mid
			// 
			this.mid.Name = "mid";
			this.mid.Type = typeof(int);
			this.mid.ValueInfo = "0";
			// 
			// mtid
			// 
			this.mtid.Name = "mtid";
			this.mtid.Type = typeof(int);
			this.mtid.ValueInfo = "0";
			// 
			// membertype_review_data
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.GroupHeader1});
			this.DataMember = "DataTable1";
			this.DataSource = this.emp_review_cr_items1;
			this.Landscape = true;
			this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 0);
			this.PageHeight = 850;
			this.PageWidth = 1100;
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.mid,
																							this.rid,
																							this.mtid});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.membertype_review_data_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.emp_review_cr_items1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void membertype_review_data_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var cr = new Nesi.Web.Reports.emp_review.emp_review_cr_itemsTableAdapters.DataTable1TableAdapter();
			cr.Fill(emp_review_cr_items1.DataTable1, (int)mid.Value, (int)rid.Value, (int)mtid.Value);

//		emp_review_general_itemsTableAdapters.DataTable1TableAdapter gen = new emp_review_general_itemsTableAdapters.DataTable1TableAdapter();
//		gen.Fill(this.emp_review_general_items1.DataTable1, (int)this.rid.Value);
			}
		}
	}