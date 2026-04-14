using NESI.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    [DataContract]
    [Serializable]
    public class Schema : ISchema
    {




        [DataMember]
        public string name { get; set; }


        [DataMember]
        public string type { get; set; }


        [DataMember]
        public string validation { get; set; }


        [DataMember]

        public bool display { get; set; }

        [DataMember]
        public bool isKey { get; set; }

        [DataMember]
        public bool editable { get; set; }

        [DataMember]
        public string header { get; set; }

        [DataMember]
        public string width { get; set; }

        [DataMember]
        public string summary { get; set; }

        [DataMember]
        public int ordinal { get; set; }
        
        [DataMember]
        public Lookup lookup { get; set; }

        [DataMember]
        public string backcolor { get; set; }

        [DataMember]
        public string currency { get; set; }

        [DataMember]
        public bool hyperlink { get; set; }

        [DataMember]
        public string hyperlinkUrl { get; set; }

	    [DataMember]
	    public string mobileHyperlinkUrl { get; set; }

		[DataMember]
        public string hyperlinkUrlParam { get; set; }

        [DataMember]
        public string rawType { get; set; }

        [DataMember]
        public string style { get; set; }

        [DataMember]
        public string defaultFilter { get; set; }

        [DataMember]
        public string displayFormat { get; set; }

        [DataMember]
        public string groupSummary { get; set; }

        [DataMember]
        public int groupSummaryOrdinal { get; set; }

        [DataMember]
        public string toolTip { get; set; }

        [DataMember]
        public bool hideItemFilter { get; set; }
    }
}
