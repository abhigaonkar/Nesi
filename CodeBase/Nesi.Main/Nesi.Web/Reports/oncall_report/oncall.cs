using System;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Data;

using System.Drawing.Printing;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for oncall
	/// </summary>
	public class oncall : DevExpress.XtraReports.UI.XtraReport
		{
		private DevExpress.XtraReports.UI.DetailBand Detail; 
		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private XRPictureBox xrPictureBox1;
		private PageFooterBand PageFooter;
		private XRPageInfo xrPageInfo1;
		private PageHeaderBand PageHeader;
		private ReportHeaderBand ReportHeader;
		private XRLabel xrLabel2;
		public DataTable dt;
		public oncall()
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
            string resourceFileName = "oncall.resx";
            System.Resources.ResourceManager resources = global::Resources.oncall.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Dpi = 96F;
            this.Detail.HeightF = 15F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 96F;
            this.TopMargin.HeightF = 19F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 96F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Dpi = 96F;
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(219F, 123F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
            this.PageFooter.Dpi = 96F;
            this.PageFooter.HeightF = 58F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 96F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(433F, 10F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(96F, 22.08F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Dpi = 96F;
            this.PageHeader.HeightF = 0F;
            this.PageHeader.Name = "PageHeader";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPictureBox1});
            this.ReportHeader.Dpi = 96F;
            this.ReportHeader.HeightF = 134F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 96F;
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 22F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(270F, 73.91998F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(424F, 49.08F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "After Hours On Call Schedule";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // oncall
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader,
            this.ReportHeader});
            this.Dpi = 96F;
            this.HorizontalContentSplitting = DevExpress.XtraPrinting.HorizontalContentSplitting.Smart;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(48, 48, 19, 0);
            this.PageHeight = 816;
            this.PageWidth = 1056;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.Pixels;
            this.Version = "14.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.oncall_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void oncall_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			this.PageHeader.Controls.Add(CreateXRTable_header());
			this.Detail.Controls.Add(CreateXRTable());
			}


		public XRTable CreateXRTable_header()
			{
			var cellsInRow = dt.Columns.Count;

			var rowHeight = 37f;
			var table = new XRTable();
			table.Font = cellsInRow < 5 ? new System.Drawing.Font("Arial", 11) : new System.Drawing.Font("Arial", 9);
			table.Font = cellsInRow > 8 ? new System.Drawing.Font("Arial", 7) : table.Font;
			table.Borders = DevExpress.XtraPrinting.BorderSide.All;
			table.BorderColor = System.Drawing.Color.LightGray;
			table.Padding = 5;

			table.BeginInit();
			var row_header = new XRTableRow();
			row_header.HeightF = rowHeight;
			for (var j = 0; j < cellsInRow; j++)
				{
				var cell = new XRTableCell();
				row_header.Cells.Add(cell);
				cell.Text = dt.Columns[j].ColumnName;
				cell.ForeColor = System.Drawing.Color.White;
				cell.BackColor = System.Drawing.Color.DarkBlue;
				cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
				row_header.Padding = 10;


				}
			var _date = DateTime.Today.AddDays(1);
			var _color = Color.LightGray;
			table.BeforePrint += new PrintEventHandler(table_BeforePrint);
			table.Rows.Add(row_header);
			table.AdjustSize();
			table.EndInit();
			return table;
			}


		public XRTable CreateXRTable()
			{
		


			var cellsInRow = dt.Columns.Count;
		
			var rowHeight = 30f;

			var table = new XRTable();
			table.Font = cellsInRow < 5 ? new System.Drawing.Font("Arial", 11) : new System.Drawing.Font("Arial", 9);
			table.Font = cellsInRow > 8 ? new System.Drawing.Font("Arial",7) : table.Font;
			table.Borders = DevExpress.XtraPrinting.BorderSide.All;
			table.BorderColor = System.Drawing.Color.LightGray;
			table.Padding = 5;

			table.BeginInit();
			var row_header = new XRTableRow();
			row_header.HeightF = rowHeight;
			for (var j = 0; j < cellsInRow; j++)
				{
				var cell = new XRTableCell();
				row_header.Cells.Add(cell);
				cell.Text = dt.Columns[j].ColumnName;
				cell.ForeColor = System.Drawing.Color.White;
				cell.BackColor = System.Drawing.Color.DarkBlue;
				cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
				row_header.Padding = 10;
			
			
				}
			var _date = DateTime.Today.AddDays(1);
			var _color = Color.LightGray;
			//	table.Rows.Add(row_header);
		

			for (var i = 0; i < dt.Rows.Count; i++)
				{
				var row = new XRTableRow();
				row.Padding = 10;
				row.HeightF = rowHeight;
		
				row.CanShrink = true;

				if (_color == Color.White)
					{
					_color = Color.WhiteSmoke;
					}
				else
					{
					_color = Color.White;
					}
				row.BackColor = _color;
				for (var j = 0; j < cellsInRow; j++)
					{
					var cell = new XRTableCell();
				
				

					cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;		
				
					//	cell.KeepTogether = true;

					if (dt.Columns[j].ColumnName=="Date")
						{
					
						if (!Convert.ToDateTime(dt.Rows[i][j]).ToString().Equals(_date.ToString()))
							{
						
							//cell.RowSpan = 2;
					
							_date = Convert.ToDateTime(dt.Rows[i][j]);
						
							cell.Text = dt.Rows[i][j].ToString();
							row.Cells.Add(cell);
							//		cell.WidthF = this.PrintingSystem.Graph.MeasureString(cell.Text).Width;
						
							}
						else
							{
							row.Cells.Add(cell);
							//		cell.WidthF = this.PrintingSystem.Graph.MeasureString(cell.Text).Width;
							}
					
						}
					else if (dt.Columns[j].ColumnName == "Role")
						{
					
						cell.Text = dt.Rows[i][j].ToString();
						row.Cells.Add(cell);
						//	cell.WidthF = this.PrintingSystem.Graph.MeasureString(cell.Text).Width;
						}
					else
						{
					
						cell.Text = dt.Rows[i][j].ToString();
						row.Cells.Add(cell);
						//	cell.WidthF = this.PrintingSystem.Graph.MeasureString(cell.Text).Width;
						}
//				cell.Styles.Style.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
					}
				table.Rows.Add(row);
				}

			table.BeforePrint += new PrintEventHandler(table_BeforePrint);
		
			table.AdjustSize();
			table.EndInit();
			return table;
			}

		void table_BeforePrint(object sender, PrintEventArgs e)
			{
			var table = ((XRTable)sender);
			table.LocationF = new DevExpress.Utils.PointFloat(0F, 0F);
			table.WidthF = this.PageWidth - this.Margins.Left - this.Margins.Right;
			}

		}
	}