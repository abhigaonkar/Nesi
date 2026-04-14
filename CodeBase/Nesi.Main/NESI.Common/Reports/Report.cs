using NESI.Common.Interface;
using NESI.Common.Serialization;
using System;
using System.Runtime.Serialization;

namespace NESI.Common
{
    [DataContract]
    [Serializable]
    public class Report : IReport
    {
        [DataMember]
        [ReflectionMapIgnore]
        public Schema[] columns { get; set; }
        
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string createEndPoint { get; set; }

        [DataMember]
        public string editEndPoint { get; set; }

        [DataMember]
        public string deleteEndPoint { get; set; }

        [DataMember]
        public bool editable { get; set; }

        [DataMember]
        public bool exportable { get; set; }

        [DataMember]
        public string bulkEditEndPoint { get; set; }


        [DataMember]
        public string searchEndPoint { get; set; }

        [DataMember]
        public bool isPrivate { get; set; }

        [DataMember]
        public string distinctEndPoint { get; set; }

        [DataMember]
        public string exportToExcelEndPoint { get; set; }


        [DataMember]
        public string exportToPdfEndPoint { get; set; }

        [DataMember]
        public string reportFor { get; set; }

        [DataMember]
        public string viewInvoiceUrl { get; set; }

        [DataMember]
        public string viewInvoiceUrlParams { get; set; }

        [DataMember]
        public bool hasSummaryRow { get; set; }

        [DataMember]
        public bool isSelectable { get; set; }
    }
}
