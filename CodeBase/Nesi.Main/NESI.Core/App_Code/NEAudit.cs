using System;
using System.Data;

namespace nesi.core
	{
	public class NeAudit
		{

		public int audititem_id { get; set; }
		public string audititem_name { get; set; }
		public int audititem_active { get; set; }
		public int audititem_checkbox { get; set; }

		public NeAudit() { }
		public NeAudit(int _id)
			{
			load(_id);
			}
		private void load(int _id)
			{
			audititem_id = _id;
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM audititem  WHERE audititem_id = @v0", new object[] { _id });
			foreach(DataRow dr in dt.Rows)
				{
				audititem_name = dr["audititem_name"].ToString();
				audititem_checkbox = (int) dr["audititem_checkbox"];
				}
			}
		}
	public class NeAudit_History
		{
		public int audithistory_type                         { get; set; }
		public int audithistory_headerid                     { get; set; }
		public int business_unit_id             { get; set; }
		public int audithistory_header_memberid              { get; set; }
		public DateTime audithistory_header_audit_date       { get; set; }
		public DateTime audithistory_header_entered_date     { get; set; }
		public NeAuditItem_RatingDesc[] audithistory_details { get; set; }
		public double audithistory_header_score              { get; set; }
		public string audithistory_header_companyname        { get; set; }

		public NeAudit_History(){}
		public NeAudit_History(int _id)
			{
			audithistory_header_score = 0.0;
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM audithistory_header  WHERE audithistory_headerid = @v0", new object[] { _id });
			foreach(DataRow dr in dt.Rows)
				{
				audithistory_headerid            = Convert.ToInt16(dr["audithistory_headerID"]);
				business_unit_id				 = (int) dr["business_unit_id"];
				audithistory_header_memberid     = Convert.ToInt16(dr["audithistory_header_memberid"]);
				audithistory_header_audit_date   = Convert.ToDateTime(dr["audithistory_header_dateofaudit"]);
				audithistory_header_entered_date = dr["audithistory_header_dateofentry"] == DBNull.Value
					? DateTime.Today 
					: Convert.ToDateTime(dr["audithistory_header_dateofentry"]);
				audithistory_header_score        = dr["audithistory_header_score"] ==DBNull.Value
					? 0
					: Convert.ToDouble(dr["audithistory_header_score"]);
				audithistory_header_companyname  = new NeBusinessUnit(business_unit_id).name;
				audithistory_type                = Convert.ToInt32(dr["audithistory_type"]);
				}
			}
		public void save()
			{
			audithistory_headerid = Toolbox.doSQL_return_id(@"
INSERT INTO audithistory_header 
	(
	business_unit_id,
	audithistory_header_memberid,
	audithistory_header_dateofaudit,
	audithistory_header_dateofentry,
	audithistory_header_score
	) 
VALUES
	(@v0,@v1,@v2,NOW(),@v3)",
	new object[] {
				business_unit_id, 
				audithistory_header_memberid, 
				audithistory_header_audit_date.ToString("YYYY-MM-DD"), 
				audithistory_header_score});
			}
		public void savescore()
			{
			Toolbox.doSQL_void(@"UPDATE audithistory_header
SET audithistory_header_score = @v0 
WHERE audithistory_headerid = @v1",
new object[] {
audithistory_header_score, audithistory_headerid});
			} 
		}
	public class NeAuditItem_RatingDesc
		{
		public int audit_item_rating_desc_id { get; set; }
		public string audit_item_rating_desc_name { get; set; }
		public int audit_item_rating_desc_audit_item_id { get; set; }
		public int audit_item_rating_desc_value { get; set; }
		public int audit_item_details_header_id { get; set; }
		public bool audit_item_result { get; set; }

		public NeAuditItem_RatingDesc(){}

		public NeAuditItem_RatingDesc(int _audit_item_rating_desc_id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM audititem_ratingdesc  where audititem_ratingdesc_id = @v0", new object[] { _audit_item_rating_desc_id });
			foreach(DataRow dr in dt.Rows)
				{
				audit_item_rating_desc_id            = Convert.ToInt32(dr["audititem_ratingdesc_id"]);
				audit_item_rating_desc_name          = dr["audititem_ratingdesc_name"].ToString();
				audit_item_rating_desc_audit_item_id = Convert.ToInt32(dr["audititem_ratingdesc_audititem_id"]);
				audit_item_rating_desc_value         = Convert.ToInt32(dr["audititem_ratingdesc_value"]);
				}
			}
		public void Save()
			{
			audit_item_rating_desc_id = Toolbox.doSQL_return_id(@"
INSERT INTO audititem_ratingdesc 
	(
	audititem_ratingdesc_Name, 
	audititem_ratingdesc_auditItem_id, 
	audititem_value
	) 
VALUES 
	(@v0,@v1,@v2)",
	new object[] {
				audit_item_rating_desc_name, 
				audit_item_rating_desc_audit_item_id, 
				audit_item_rating_desc_value
			});
			}
		}
	}