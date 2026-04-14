using System.Data;
using core;
using nesi.core;
using NESI.BLL.Base;

namespace NESI.BLL.Layout.Banner
{
	public partial class ToDo : BLLBase
	{

		public DTO.ViewModels.CurrentUser.Layout.ToDo[] GetToDo()
		{
			var dt_final = new DataTable();
			dt_final.Columns.Add(new DataColumn("Link"));
			dt_final.Columns.Add(new DataColumn("Description"));
			dt_final.Columns.Add(new DataColumn("Overdue"));
			dt_final.Columns.Add(new DataColumn("PageId"));
			dt_final.Columns.Add(new DataColumn("DescriptionShort"));
			dt_final.Columns.Add(new DataColumn("n2_page"));
			dt_final.Columns.Add(new DataColumn("n2_params"));


			TodoList.getToDoList(dt_final, new NeMember(CurrentUser.Id));
            
			var models = DTO.Mapper.DataTableMapper.Map<DTO.ViewModels.CurrentUser.Layout.ToDo>(dt_final);
			return models.ToArray();
		}
	}


}
