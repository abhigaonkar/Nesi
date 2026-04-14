using System.Configuration;

namespace NESI.Common
{
    public class ReportSection : ConfigurationSection
    {
        [ConfigurationProperty("columns", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(columnsCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]

        public columnsCollection columns
        {
            get
            {
                var columnsCollection =
                    (columnsCollection)base["columns"];

                return columnsCollection;
            }

            set
            {
                columnsCollection columnsCollection = value;
            }

        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        [ConfigurationProperty("createEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string createEndPoint
        {
            get => (string)this["createEndPoint"];
            set => this["createEndPoint"] = value;
        }

        [ConfigurationProperty("editEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string editEndPoint
        {
            get => (string)this["editEndPoint"];
            set => this["editEndPoint"] = value;
        }

        [ConfigurationProperty("deleteEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string deleteEndPoint
        {
            get => (string)this["deleteEndPoint"];
            set => this["deleteEndPoint"] = value;
        }

        [ConfigurationProperty("bulkEditEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string bulkEditEndPoint
        {
            get => (string)this["bulkEditEndPoint"];
            set => this["bulkEditEndPoint"] = value;
        }

        [ConfigurationProperty("searchEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string searchEndPoint
        {
            get => (string)this["searchEndPoint"];
            set => this["searchEndPoint"] = value;
        }


        [ConfigurationProperty("distinctEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string distinctEndPoint
        {
            get => (string)this["distinctEndPoint"];
            set => this["distinctEndPoint"] = value;
        }

        [ConfigurationProperty("exportToExcelEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string exportToExcelEndPoint
        {
            get => (string)this["exportToExcelEndPoint"];
            set => this["exportToExcelEndPoint"] = value;
        }

        [ConfigurationProperty("exportToPdfEndPoint", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string exportToPdfEndPoint
        {
            get => (string)this["exportToPdfEndPoint"];
            set => this["exportToPdfEndPoint"] = value;
        }

        [ConfigurationProperty("editable", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool editable
        {
            get => (bool)this["editable"];
            set => this["editable"] = value;
        }

        [ConfigurationProperty("isPrivate", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool isPrivate
        {
            get => (bool)this["isPrivate"];
            set => this["isPrivate"] = value;
        }

        [ConfigurationProperty("exportable", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool exportable
        {
            get => (bool)this["exportable"];
            set => this["exportable"] = value;
        }

        public ReportSection()
        {
            SchemaSection schema = new SchemaSection();
            columns.Add(schema);

        }

        [ConfigurationProperty("reportFor", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string reportFor
        {
            get => (string)this["reportFor"];
            set => this["reportFor"] = value;
        }

        [ConfigurationProperty("viewInvoiceUrl", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string viewInvoiceUrl
        {
            get => (string)this["viewInvoiceUrl"];
            set => this["viewInvoiceUrl"] = value;
        }

        [ConfigurationProperty("viewInvoiceUrlParams", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string viewInvoiceUrlParams
        {
            get => (string)this["viewInvoiceUrlParams"];
            set => this["viewInvoiceUrlParams"] = value;
        }

        [ConfigurationProperty("hasSummaryRow", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool hasSummaryRow
        {
            get => (bool)this["hasSummaryRow"];
            set => this["hasSummaryRow"] = value;
        }

        [ConfigurationProperty("isSelectable", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool isSelectable
        {
            get => (bool)this["isSelectable"];
            set => this["isSelectable"] = value;
        }
    }
}
