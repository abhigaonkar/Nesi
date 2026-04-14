using System.Collections.Generic;

namespace NESI.BLL.Layout.Menu
{
	public class MenuItem
	{   
        public int Id { get; set; }
        public string MenuId { get; set; }
		public string Label { get; set; }
		public string Icon { get; set; }
		public string Url { get; set; }
        public int Badge { get; set; }
        public string Target { get; set; }
        public string BadgeStyleClass { get; set; }
        public MenuType Type { get; set; }
        public List<MenuItem> Items { get; set; }
	}
	
}