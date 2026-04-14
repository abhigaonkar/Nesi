using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public interface IReportExporter
    {

        Task<HttpResponseMessage> ExporttoExcel(string fileName);

        Task<HttpResponseMessage> ExporttoPdf(string fileName);

        Task<HttpResponseMessage> ExporttoPdf(string fileName, List<string> columnList);

        Task<HttpResponseMessage> ExporttoExcel(string fileName, List<string> columnList);
    }
}
