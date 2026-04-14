using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeDisciplinary
	/// </summary>
	public class NeDisciplinary
		{
		public NeDisciplinary()
			{
			comments = "";
			note_type = "";
			active = true;
			}

		public bool active { get; set; }
		public int member_note_id { get; set; }
		public int business_unit_id { get; set; }
		public int member_note_added_by_member_id { get; set; }
		public int member_note_member_id { get; set; }
		public string note_type { get; set; }
		public string comments { get; set; }
		public string date { get; set; }
		public int member_id_audit { get; set; }

		public DataTable disciplinary_summary(int _mem_id)
			{
			return Toolbox.doSQL_dt(@"SELECT mn.*, m.Member_fullname as UserNote, mm.Member_fullname as addedby ,f.id file_id, f.name filename FROM MemberNote mn Inner join Member m on mn.Member_ID_Audit = m.Member_Id Inner join Member mm on mn.Membernote_addedby_member_id = mm.Member_Id LEFT JOIN filestore.files f ON mn.MemberNote_ID = f.sub_folder_id and f.page_id = 127 and f.folder_id = 6  WHERE MemberNote_Member_ID =@v0 order by mn.date desc", new object[] { _mem_id });
			}
		public void update() /* Update Disciplinary Info in Employee Tab */
			{
			Toolbox.doSQL_void(@"Update MemberNote
SET Date = @v0,
Comments = @v1,
Active = @v2,
business_unit_id = @v3,
MemberNote_AddedBy_Member_ID =@v4
WHERE MemberNote_ID = @v5",
new object[] {
date, comments, active, business_unit_id, member_note_added_by_member_id, member_note_id});
			}
		public void add(int _mem_id)
			{
			member_note_id = Toolbox.doSQL_return_id(@"Insert Into MemberNote 
(MemberNote_Member_ID, Date, Comments, Member_ID_Audit, Active, business_unit_id, MemberNote_AddedBy_Member_ID) 
Values (@v0,@v1,@v2,@v3,@v4,@v5,@v6)",
new object[] {
_mem_id, date, comments, member_id_audit, active, business_unit_id, member_note_added_by_member_id});
			}
		}
	}