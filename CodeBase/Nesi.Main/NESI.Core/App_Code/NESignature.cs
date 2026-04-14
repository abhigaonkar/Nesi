using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.Common.Models;

namespace nesi.core
{
    public class NESignature
    {
        #region privates
        private int _id;
        private DateTime _created_dt;
        private int _member;
        private string _table;
        private int _table_id;
        private byte[] _graphic;
        private string _printed_name;
        private int _contact_id;
        private string _send_to_contact_ids;
        #endregion privates

        #region publics
        public int id { get { return _id; } set { _id = value; } }

        public DateTime created_dt { get { return _created_dt; } set { _created_dt = value; } }

        public int member { get { return _member; } set { _member = value; } }

        public string table { get { return _table; } set { _table = value; } }

        public int table_id { get { return _table_id; } set { _table_id = value; } }

        public byte[] graphic { get { return _graphic; } set { _graphic = value; } }

        public string printed_name { get { return _printed_name; } set { _printed_name = value; } }

        public int contact_id { get { return _contact_id; } set { _contact_id = value; } }

        public string send_to_contact_ids { get { return _send_to_contact_ids; } set { _send_to_contact_ids = value; } }

        #endregion publics

        public NESignature(int id)
        {
            DataTable dt;
            try { dt = Toolbox.doSQL_dt(@"Select * from signature  where id =@v0", new object[] { id }); }
            catch { throw new Exception("Signature Not Found"); }

            _id = Convert.ToInt32(dt.Rows[0]["id"]);
            _created_dt = Convert.ToDateTime(dt.Rows[0]["created_dt"]);
            _member = Convert.ToInt32(dt.Rows[0]["member"]);
            _table = dt.Rows[0]["table"].ToString();
            _table_id = Convert.ToInt32(dt.Rows[0]["table_id"]);
            _graphic = Convert.FromBase64String(dt.Rows[0]["graphic"].ToString().Split(',')[1]);
            _printed_name = Convert.ToString(dt.Rows[0]["printed_name"]);
            _contact_id = Convert.ToInt32(dt.Rows[0]["contact_id"]);
            _send_to_contact_ids = dt.Rows[0]["send_to_contact_ids"].ToString();
          
        }

        public NESignature()
        {

        }

        public void save(bool IsFinalSignoff=true)
        {
                if (id == 0)
                {
                    try
                    {
                        var sql = @"Insert into signature ( `created_dt`, `member`,  `table`,  `table_id`,  `graphic`,  `printed_name`,  `contact_id`,  `send_to_contact_ids`) values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7)";
                        var paramObjects =
                            new object[]
                            {
                            _created_dt, //0
							_member, //1
							_table, //2
                            _table_id, //3
                            _graphic, //4
							_printed_name, //5
							_contact_id, //6
							_send_to_contact_ids //7							
                            };
                        _id = Toolbox.doSQL_return_id(sql, paramObjects);

                    if (_id > 0 && IsFinalSignoff)
                    {

                        var scriptsql = @"Update woprog set woprog_status=@v0 where woprog_id=@v1";
                        var _paramObjects = new object[]
                       {
                              OpsWOStatus.InitialPrep,
                              _table_id
                       };

					Toolbox.doSQL_void(scriptsql, _paramObjects);
                    }
                }
                    catch
                    {
                        throw new Exception("Couldn't Add Signature.");
                    }
                }
                else
                {
                    try
                    {
                        var sql = @"Update signature set 
			                        created_dt=@v0,
			                        member=@v1,
			                        table=@v2,
			                        table_id=@v3,
			                        graphic=@v4,
			                        printed_name=@v5,
			                        contact_id=@v6,
			                        send_to_contact_ids=@v7";
                        var paramObjects = new object[]
                        {
                            _created_dt, //0
							_member,//1
							_table,//2
							_table_id, //3
							_graphic,//4
							_printed_name,//5
							_contact_id,//6
							_send_to_contact_ids//7							
						};
					Toolbox.doSQL_void(sql, paramObjects);
                      
                    }
                    catch
                    {
                        throw new Exception("Couldn't Update Signature");
                    }
                }
        }

        
    }
}
