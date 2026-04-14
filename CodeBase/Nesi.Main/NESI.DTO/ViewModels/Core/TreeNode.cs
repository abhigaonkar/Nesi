using System.Collections.Generic;

namespace NESI.DTO.ViewModels.Core
{
	public class TreeNode
	{
		
		public string Label { get; set; }
		public string Data { get; set; }
		public string ExpandedIcon { get; set; }
		public string CollapsedIcon { get; set; }
		public string Icon { get; set; }
		//public string Type { get; set; }
		//public TreeNode Parent { get; set; }
		//public bool PartialSelected { get; set; }
		//public bool Leaf { get; set; }
		//public bool Draggable { get; set; }
		//public bool Droppable { get; set; }
		//public bool Selectable { get; set; }
		public List<TreeNode> Children { get; set; }
	}
}