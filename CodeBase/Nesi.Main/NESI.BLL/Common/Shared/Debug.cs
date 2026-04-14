using System.Data.Entity.Migrations;
using NESI.Data.Entities;

namespace NESI.BLL.Common.Shared
{
	public static class Debug
	{
		#region debug_note
		/// <summary>
		/// copy from app_code\toolbox.cs
		/// </summary>
		/// <param name="note"></param>
		public static void MattNote(object note)
		{
			var db = new NESIMySQL();
			var entity = new NESI.Data.Entities.matt()
			{
				jumble = note.ToString()
			};
			db.matt.AddOrUpdate(entity);
			db.SaveChanges();
		}
		#endregion debug_note
	}
}