using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NeGLGroup
    {
		public int id { get; set; }
		public string number { get; set; }
		public string desc { get; set; }
		public string type { get; set; }
		public int tax_entity_id { get; set; }
		public int total { get; set; }
        public string line_advance { get; set; }
        protected void load(int _id)
        {

 var dt = Toolbox.doSQL_dt(@"SELECT * FROM gl_group_te WHERE id = @v0 ", new object[] {  _id } );
            foreach (DataRow dr in dt.Rows)
            {
                id = (int)dr["id"];
                number = dr["number"].ToString();
                desc = dr["desc"].ToString();
                type = dr["type"].ToString();
                tax_entity_id = (int)dr["tax_entity_id"];
                total = Convert.ToInt32(dr["total"]);
                line_advance = dr["line_advance"].ToString();
            }
        }
        public NeGLGroup(int tax_entity_id, string number)
        {
            var _get_id = Toolbox.doSQL_int(@"SELECT id FROM gl_group_te WHERE tax_entity_id = @v0  and number=@v1 ", new object[] {  tax_entity_id, number } );
            load(_get_id);

        }

        public NeGLGroup(int id)
        {
            load(id);
        }


        public void save(bool save_to_bv)
        {

            if (id == 0)
            {
                using (var conn = Toolbox.connect())
                {
                    var comm = new MySqlCommand(@"
                    Insert into
	                gl_group_te 
                 (number ,
                `desc` ,
                type ,
                tax_entity_id,
                total,
                line_advance) 
values
(
 ?number,
 ?desc,
 ?type,
 ?tax_entity_id,
?total,
?line_advance)", conn);
                comm.Parameters.AddWithValue("?number", number);
                    comm.Parameters.AddWithValue("?desc", desc);
                    comm.Parameters.AddWithValue("?type", type);
                comm.Parameters.AddWithValue("?tax_entity_id", tax_entity_id);
                    comm.Parameters.AddWithValue("?total", total);
                    comm.Parameters.AddWithValue("?line_advance", line_advance);
                    comm.ExecuteNonQuery();


                //if (save_to_bv)
                //    {
                //    // now add it to BV if its not already there
                //    string dsn = new NeTaxEntity(tax_entity_id).DSN;
                //    if (new Toolbox().getSQL_int(@"Select count(gl_group) from gl_groups  where gl_group=?", dsn,
                //            new object[] {number}) == 0)
                //        {
				//
                //        new Toolbox().getSQL_int(
                //            @"Insert into gl_groups (gl_group, default_desc,alias,search_arg,acct_type,total_desc,total,line_advance)  values(?,?,?,?,?, ?,?,?)",
                //            dsn, new object[] {number, desc, desc, desc.ToUpper(), type, desc, total, line_advance});
                //        }
                //    }

                }
        }
            else
            {
                using (var conn = Toolbox.connect())
                {
                    var comm = new MySqlCommand(@"
UPDATE 
	gl_group_te 
SET 
	 number = ?number,
               `desc` = ?desc,
                type = ?type,
                tax_entity_id = ?tax_entity_id,
                total = ?total,
                line_advance = ?line_advance
                WHERE 
	id = ?id", conn);
                    comm.Parameters.AddWithValue("?number", number);
                    comm.Parameters.AddWithValue("?desc", desc);
                    comm.Parameters.AddWithValue("?type", type);
                    comm.Parameters.AddWithValue("?tax_entity_id", tax_entity_id);
                    comm.Parameters.AddWithValue("?total", total);
                    comm.Parameters.AddWithValue("?line_advance", line_advance);

                    comm.ExecuteNonQuery();
                }
            }
        }

	

		public NeGLGroup()
			{
            id = 0;
			//
			//  
			//
			}
		}
	}