using DevExpress.XtraReports.UI;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for Map
	/// </summary>
	public class Map : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private XRPictureBox xrPictureBox2;
		private DevExpress.XtraReports.Parameters.Parameter printmap;
		private DevExpress.XtraReports.Parameters.Parameter address;
	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Map()
			{
			InitializeComponent();
		
		
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
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.printmap = new DevExpress.XtraReports.Parameters.Parameter();
			this.address = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrPictureBox2});
			this.Detail.HeightF = 550F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.StylePriority.UseTextAlignment = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrPictureBox2
			// 
			this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrPictureBox2.Name = "xrPictureBox2";
			this.xrPictureBox2.SizeF = new System.Drawing.SizeF(675F, 550F);
			this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.CenterImage;
			this.xrPictureBox2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox2_BeforePrint);
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 75F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// printmap
			// 
			this.printmap.Name = "printmap";
			// 
			// address
			// 
			this.address.Name = "address";
			// 
			// Map
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin});
			this.Margins = new System.Drawing.Printing.Margins(50, 50, 75, 100);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.printmap,
																							this.address});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Map_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
		
			}

		private void Map_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			if (printmap.Value.ToString()=="True")
				{
				xrPictureBox2.ImageUrl = @"http://maps.google.com/maps/api/staticmap?center=" + address.Value + @"&zoom=14&size=685x550&maptype=roadmap;amp&markers=color:red%7Ccolor:red%7Clabel:C%7C" + address.Value + @"&sensor=false";
				}
			else
				{
				xrPictureBox2.Visible = false;
				}
			}
		}
	}