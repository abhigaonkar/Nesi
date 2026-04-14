using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.ReleaseSystem
{
	public class RebootTime
	{
		public string HostName { get; set; }
		public int Minutes  { get; set; }
		public int ShutDownTime { get; set; }
	}
}