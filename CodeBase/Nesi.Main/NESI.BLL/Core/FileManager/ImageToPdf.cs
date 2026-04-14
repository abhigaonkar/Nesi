using System;
using System.Diagnostics;
using System.Drawing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace NESI.BLL.Core.FileManager
{
	public static class ImageToPdf
	{

		public static void GeneratePDF(string filename, string[] images)
		{
			PdfDocument document = new PdfDocument();

			foreach (var imageLoc in images)
			{

				// Create an empty page or load existing
				PdfPage page = document.AddPage();

				// Get an XGraphics object for drawing
				XGraphics gfx = XGraphics.FromPdfPage(page);
				XImage image = XImage.FromFile(imageLoc);
				var endpoint = new Point(image.PixelWidth, image.PixelHeight);
				var scaleX = endpoint.X / (page.Width - 10);
				var scaleY = endpoint.Y / (page.Height - 10);

				if (image.PixelWidth > page.Width - 10 || image.PixelHeight > page.Height - 10)
				{
					if (scaleX > scaleY)
					{
						endpoint.X = Convert.ToInt32(page.Width) - 5;
						endpoint.Y = Convert.ToInt32(endpoint.Y / scaleX) - 5;
					}
					else
					{
						endpoint.Y = Convert.ToInt32(page.Height) - 5;
						endpoint.X = Convert.ToInt32(endpoint.X / scaleY) - 5;
					}
				}
				gfx.DrawImage(image, 5, 5, endpoint.X, endpoint.Y);
			}

			// Save and start View
			document.Save(filename);
		}

		//public static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
		//{
		//	XImage image = XImage.FromFile(jpegSamplePath);
		//	gfx.DrawImage(image, x, y, width, height);
		//}

	}
}