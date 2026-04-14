using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{

    public class SchemaSection : ConfigurationElement
    {
        public SchemaSection(String name, String type, String validation, bool display, bool editable, String header, String width, String summary, int ordinal)
        {
            this.name = name;
            this.type = type;
            this.validation = validation;
            this.display = display;
            this.editable = editable;
            this.header = header;
            this.width = width;
            this.summary = summary;
            this.ordinal = ordinal;
        }

        public SchemaSection()
        {

        }


        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }


        [ConfigurationProperty("type", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }


        [ConfigurationProperty("validation", DefaultValue = "", IsKey = true)]
        public string validation
        {
            get
            {
                return (string)this["validation"];
            }
            set
            {
                this["validation"] = value;
            }
        }


        [ConfigurationProperty("display", DefaultValue = false)]

        public bool display
        {
            get
            {
                return (bool)this["display"];
            }
            set
            {
                this["display"] = value;
            }
        }


        [ConfigurationProperty("isKey", DefaultValue = false)]

        public bool isKey
        {
            get
            {
                return (bool)this["isKey"];
            }
            set
            {
                this["isKey"] = value;
            }
        }


        [ConfigurationProperty("editable", DefaultValue = false)]
        public bool editable
        {
            get
            {
                return (bool)this["editable"];
            }
            set
            {
                this["editable"] = value;
            }
        }


        [ConfigurationProperty("header", DefaultValue = "")]
        public string header
        {
            get
            {
                return (string)this["header"];
            }
            set
            {
                this["header"] = value;
            }
        }


        [ConfigurationProperty("width", DefaultValue = "")]
        public string width
        {
            get
            {
                return (string)this["width"];
            }
            set
            {
                this["width"] = value;
            }
        }


        [ConfigurationProperty("summary", DefaultValue = "")]
        public string summary
        {
            get
            {
                return (string)this["summary"];
            }
            set
            {
                this["summary"] = value;
            }
        }


        [ConfigurationProperty("ordinal", DefaultValue = 0)]
        public int ordinal
        {
            get
            {
                return (int)this["ordinal"];
            }
            set
            {
                this["ordinal"] = value;
            }
        }

        [ConfigurationProperty("lookup", DefaultValue = "")]
        public string lookup
        {
            get
            {
                return (string)this["lookup"];
            }
            set
            {
                this["lookup"] = value;
            }
        }

        [ConfigurationProperty("backcolor", DefaultValue = "")]
        public string backcolor
        {
            get
            {
                return (string)this["backcolor"];
            }
            set
            {
                this["backcolor"] = value;
            }
        }

        [ConfigurationProperty("currency", DefaultValue = "")]
        public string currency
        {
            get
            {
                return (string)this["currency"];
            }
            set
            {
                this["currency"] = value;
            }
        }

        
        [ConfigurationProperty("hyperlink", DefaultValue = false)]
        public bool hyperlink
        {
            get
            {
                return (bool)this["hyperlink"];
            }
            set
            {
                this["hyperlink"] = value;
            }
        }

        [ConfigurationProperty("hyperlinkUrl", DefaultValue = "")]
        public string hyperlinkUrl
        {
            get
            {
                return (string)this["hyperlinkUrl"];
            }
            set
            {
                this["hyperlinkUrl"] = value;
            }
        }
        [ConfigurationProperty("hyperlinkUrlParam", DefaultValue = "")]
        public string hyperlinkUrlParam
        {
            get
            {
                return (string)this["hyperlinkUrlParam"];
            }
            set
            {
                this["hyperlinkUrlParam"] = value;
            }
        }
	    [ConfigurationProperty("mobileHyperlinkUrl", DefaultValue = "")]
	    public string mobileHyperlinkUrl
		{
		    get
		    {
			    return (string)this["mobileHyperlinkUrl"];
		    }
		    set
		    {
			    this["mobileHyperlinkUrl"] = value;
		    }
	    }
		[ConfigurationProperty("rawType", DefaultValue = "")]
        public string rawType
        {
            get
            {
                return (string)this["rawType"];
            }
            set
            {
                this["rawType"] = value;
            }
        }

        [ConfigurationProperty("style", DefaultValue = "")]
        public string style
        {
            get
            {
                return (string)this["style"];
            }
            set
            {
                this["style"] = value;
            }
        }

        [ConfigurationProperty("defaultFilter", DefaultValue = "")]
        public string defaultFilter
        {
            get
            {
                return (string)this["defaultFilter"];
            }
            set
            {
                this["defaultFilter"] = value;
            }
        }

        [ConfigurationProperty("displayFormat", DefaultValue = "")]
        public string displayFormat
        {
            get
            {
                return (string)this["displayFormat"];
            }
            set
            {
                this["displayFormat"] = value;
            }
        }

        [ConfigurationProperty("groupSummary", DefaultValue = "")]
        public string groupSummary
        {
            get
            {
                return (string)this["groupSummary"];
            }
            set
            {
                this["groupSummary"] = value;
            }
        }

        [ConfigurationProperty("groupSummaryOrdinal", DefaultValue = "1")]
        public int groupSummaryOrdinal
        {
            get
            {
                return (int)this["groupSummaryOrdinal"];
            }
            set
            {
                this["groupSummaryOrdinal"] = value;
            }
        }

        [ConfigurationProperty("toolTip", DefaultValue = "")]
        public string toolTip
        {
            get
            {
                return (string)this["toolTip"];
            }
            set
            {
                this["toolTip"] = value;
            }
        }

        [ConfigurationProperty("hideItemFilter", DefaultValue = false)]
        public bool hideItemFilter
        {
            get
            {
                return (bool)this["hideItemFilter"];
            }
            set
            {
                this["hideItemFilter"] = value;
            }
        }
    }
}
