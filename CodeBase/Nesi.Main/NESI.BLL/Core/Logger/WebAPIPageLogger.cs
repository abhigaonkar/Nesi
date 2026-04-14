using NESI.BLL.Base;

namespace NESI.BLL.Core.Logger
{
	public class WebAPIPageLogger : BLLBase
	{
		public void AddLog(DTO.Models.Core.LogPage model)
		{
			//var log = AutoMapper.Mapper.Map<Data.Entities.log_page>(model);
			//_db.log_page.Add(log);
			//_db.SaveChanges();
			var sql =
				$@"INSERT INTO log.log_N2 (dt,member_id,ip_address,url,request_start,request_end,render_end,elements,cl_process,host) 
									values (@p0,@p1,@p2,@p3,@p5,@p6,@p7,@p8,@p9,@p10)";
			bllToolbox.doSQL_void(sql,model.dt,model.member_id,model.ip_address,model.url,model.query_string,model.request_start,model.request_end,model.render_end,
				model.elements,model.cl_process,model.host);
		}
	}
}