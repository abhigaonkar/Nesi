using System;
using System.Collections.Generic;
using System.Data;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerRequestAdminGrid;
namespace NESI.BLL.Pages.CustomerRequestAdmin
{
    public class CustomerRequestAdminGrid : BLLGridBase<dtoT>
    {
        private readonly string _entity;

        public CustomerRequestAdminGrid() { }

        public CustomerRequestAdminGrid(Employee user) : base(user) { }

        public CustomerRequestAdminGrid(Employee user, BodyParams param, string entity)
          : base(user, param, new object[] { user.Id, user.BusinessUnitId })
        {
            _entity = entity;

            bool authEditAll = CurrentUser.AuthenticatedForPrivilege(217);
            bool isBranchManagerHr = CurrentUser.AuthenticatedForPrivilege(33);

            if (param.queryparam?.Length > 0 && param.queryparam[0].value?.ToString() != "1") ;

            // Conditionally include industrial_type_id only for cus_end_market_segment
            string columns = "id, name, is_active";
            if (_entity == "cus_end_market_segment")
            {
                columns += ", industrial_type_id";
            }

            string sqlTemplate = $@"
                                    SELECT {columns}
                                    FROM {_entity}
                                    WHERE 1=1 
                                    ORDER BY name";

            if (authEditAll)
                this.query = sqlTemplate;
            else if (isBranchManagerHr)
                this.query = $"{sqlTemplate}";
            else
                this.query = $"{sqlTemplate}";

            // if you later need row-level security, add the same WHERE clauses used in ApplicantHomeGrid
        }

        public string Delete(int id)
        {
            bllToolbox.doSQL_void($"UPDATE {_entity} SET is_active = 0 WHERE id = @v0", id);
            this.ClearCache(new object[] { UserId });
            return "Record deactivated successfully.";
        }
        // inside CustomerRequestAdminGrid.cs
        public DataTable GetIndustrialTypes()
        {
            const string sql = @"SELECT id, name, is_active
                         FROM   cus_industrial_type
                         WHERE  is_active = 1
                         ORDER  BY name";
            return bllToolbox.doSQL_dt(sql);
        }
        public string Update(string field, string value, int id)
        {
            var sql = "";
            switch (field)
            {
                case "name":
                    sql = $"UPDATE {_entity} SET name=@p0 WHERE id=@p1";
                    break;
                case "is_active":
                    sql = $"UPDATE {_entity} SET is_active=@p0 WHERE id=@p1";
                    break;
                case "industrial_type_id":  // ADD THIS CASE
                    sql = $"UPDATE {_entity} SET industrial_type_id=@p0 WHERE id=@p1";
                    break;
                default:
                    return "Error: Invalid field";
            }
            bllToolbox.doSQL_void(sql, value, id);
            this.ClearCache(new object[] { UserId });
            return "Record updated successfully.";
        }

        public string Create(string name, bool isActive, int? industrialTypeId = null)
        {

            // Simple validation without duplicate check for now
            if (string.IsNullOrWhiteSpace(name))
                return "Error: Name is required.";

            if (name.Length > 200)
                return "Error: Name cannot exceed 200 characters.";
            if (industrialTypeId.HasValue && _entity == "cus_end_market_segment")
            {
                bllToolbox.doSQL_void($"INSERT INTO {_entity} (name, is_active,created_by,industrial_type_id) VALUES (@v0, @v1,@v2,@v3)", name, isActive, UserId, Convert.ToInt32(industrialTypeId));
            }
            else
            {
                bllToolbox.doSQL_void($"INSERT INTO {_entity} (name, is_active,created_by) VALUES (@v0, @v1,@v2)", name, isActive, UserId);
            }
            this.ClearCache(new object[] { UserId });
            return "Record created successfully.";

        }

    }

}