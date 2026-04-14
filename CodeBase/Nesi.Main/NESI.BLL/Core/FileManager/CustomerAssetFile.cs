using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Core.FileManager
{
    public class CustomerAssetFile : NeFileBase
    {
        private readonly int _caId;
        private readonly int _taxEntityId;

        private string query = @"
                                SELECT bu.tax_entity_id AS tax_entity_id FROM customer_asset ca
                                INNER JOIN customer c ON ca.customer_id = c.customer_id
                                INNER JOIN business_unit bu ON bu.id = c.business_unit_id
                                WHERE ca.id=";
        public override string BaseFolder => base.FileServer + $@"\TE\TE{this._taxEntityId}\customer_asset_files\";

        public override string BasePath => Path.Combine(BaseFolder, $"CA{this._caId}");

        public CustomerAssetFile(int caId, int taxEntityId)
        {
            this._caId = caId;
            this._taxEntityId = taxEntityId;

            this.CreateFolderIfNotExists();
        }

        private void CreateFolderIfNotExists()
        {
            if (this._taxEntityId > 0)
                {
                    CreateFolder();
                }
        }

       
        private string GetBusinessUnitFromDataStore()
        {
            DataTable dbResult = new DataTable();
            string TEId = string.Empty;

            if (this._caId <= 0)
            {
                return TEId;
            }
            
            using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
            {
                query = $"{query}{this._caId}";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dbResult);
                    }
                }
            }

            if(dbResult != null && dbResult.Rows.Count > 0)
            {
                TEId = Convert.ToString(dbResult.Rows[0]["tax_entity_id"]);
            }
            return TEId;
        }
        public override void CreateFolder()
        {
            //Verify_folders_exist(BasePath, "Accounts Receivables", "Part Specs", "Correspondance", "Equipment", "PLC and HMI", "Schematics", "Pictures", "Safety");
            try
            {
                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
