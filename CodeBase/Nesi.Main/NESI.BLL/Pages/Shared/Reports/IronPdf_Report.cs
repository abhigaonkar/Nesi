using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPdf;

namespace NESI.BLL.Pages.Shared.Reports
{
   public class IronPdf_Report
    {
        public static string FileFullPath { get; set; }

        public IronPdf_Report()
        {
            IronPdf.License.LicenseKey = ConfigurationManager.AppSettings["IronPdf.LicenseKey"].ToString();
        }

        public static HtmlToPdf SetHtmlToPdf()
        {
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            // new instance of HtmlToPdf
            // Render an HTML document or snippet as a string
            htmlToPdf.PrintOptions.MarginTop = 2.0;
            htmlToPdf.PrintOptions.MarginRight = 2.0;
            htmlToPdf.PrintOptions.MarginLeft = 2.0;
            htmlToPdf.PrintOptions.MarginBottom = 2.0;  // setting Margin of PDF
           
            return htmlToPdf;
        }

        public static bool SavePdf(HtmlToPdf htmlToPdf,string teBaseFolder, string FileName,string html,bool IsReview=false)
        {
            try
            {
                var pdf = htmlToPdf.RenderHtmlAsPdf(html);
                var originalfilefullpath = Path.Combine(teBaseFolder + @"\signature_files\", FileName);
                var filefullpath = "";
                if (!File.Exists(originalfilefullpath) || IsReview)
                {
                    // save resulting pdf into file
                    pdf.SaveAs(originalfilefullpath);
                    if (File.Exists(originalfilefullpath))
                    {
                        FileFullPath = originalfilefullpath;
                        return true;
                    }
                }
                else
                {
                    string filepath = Path.Combine(teBaseFolder + @"\signature_files_backup\");
                    if (!Directory.Exists(Path.Combine(teBaseFolder + @"\signature_files_backup\")))
                    {               
                        Directory.CreateDirectory(filepath);
                    }

                        bool done = false;
                        int sequence = 1;
						var fi = new FileInfo(FileName);

                        string baseName = Path.GetFileNameWithoutExtension(fi.FullName);
                        do
                        {
                            // change here ***
                            string fname = baseName;
                            if (sequence >=1)
                                fname = fname + "_" + sequence;
                            // end of change ***

                            filefullpath = Path.Combine(filepath, fname+fi.Extension);
                            if (File.Exists(filefullpath))
                                ++sequence;
                            else
                                done = true;
                        } while (!done);

                    File.Copy(originalfilefullpath, filefullpath);
                    pdf.SaveAs(originalfilefullpath);
                    if (File.Exists(filefullpath))
                    {
                        return true;
                    }
                }

                return false;

            }
            catch(Exception ex)
            {
                return false;
            }
        }


    }
}
