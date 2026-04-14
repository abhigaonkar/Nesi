
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class FVRGrid : BLLGridBase<DTO.ViewModels.Page.Reports.FVRGrid>
    {
        public FVRGrid()
        {
        }

        public FVRGrid(Employee user) : base(user)
        {
        }

        public FVRGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            var id = user.Id;
            var bv = user.BusinessUnitId;
            string querystring = @"
SELECT 
	a.id id,
	IF(b.active = 1, 'True', 'False') active,
	b.dt_insert date,
	a.type type,
	d.ddl_name business_unit, 
	k.member_fullname cut_by,
	c.member_fullname employee, 
	CONCAT(g.name,'.',g.ext) filename,
	IF(IFNULL(e.confirmed, 0) = 1, 'True', 'False') confirmed,
	IF(IFNULL(b.upload_required, 0) = 1, 'True', 'False') upload_required,
	CONCAT(h.name,'.',h.ext) uploaded_filename,
	i.status hr_status,
	j.name fvr_status
FROM 
	member_fvr_dtl b 
LEFT JOIN	member c
	ON b.member_id = c.member_id
LEFT JOIN member_fvr_hdr a 
	ON a.id = b.member_fvr_hdr_id  
LEFT join business_unit d 
	ON c.business_unit_id = d.id 
LEFT JOIN member_fvr_history e 
	ON b.id = e.member_fvr_dtl_id 
LEFT JOIN member_fvr_tab f
	ON b.tab_index = f.id and a.type = f.type
LEFT JOIN filestore.files g 
	ON b.file_id = g.id
LEFT JOIN filestore.files h 
	ON b.uploaded_file_id = h.id
LEFT JOIN member_hrstatus i
	on c.member_hrstatus_id = i.id
LEFT JOIN member_fvr_status j
	ON b.status_id = j.id
LEFT JOIN member k
	ON a.req_member_id = k.member_id 
WHERE 
	c.member_status = 'Active' and
find_in_set(d.id,'{bu_ids}') AND
	(is_supervisor(c.member_id,";


            querystring = querystring + id + @") or (" + bv + @"=11 ))
GROUP BY 
	b.id 
ORDER BY
	a.ts DESC,
	d.id,
	b.member_id,
	b.tab_index";


            this.query = querystring;
        }
    }
}
