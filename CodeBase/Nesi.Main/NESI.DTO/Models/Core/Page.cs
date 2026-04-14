using AutoMapper.Attributes;
using NESI.Data.Entities;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(page))]
	[MapsFrom(typeof(page))]
	public class Page
	{
	    public int page_id { get; set; }
	    public int page_parent_id { get; set; }
	    public string page_name { get; set; }
	    public string page_desc { get; set; }
	    public string page_scriptpath { get; set; }
	    public int? page_order { get; set; }
	    public string page_mainmenu_img { get; set; }
	    public string page_mainmenu_img_h { get; set; }
	    public int page_enabled { get; set; }
	    public int page_custenabled { get; set; }
	    public int page_vendorenabled { get; set; }
	    public string page_class { get; set; }
	    public bool? page_mobile_ready { get; set; }
	    public string page_mobile_url { get; set; }
	    public string page_mobile_icon { get; set; }
	    public string page_mobile_action { get; set; }
	    public bool page_desktop_ready { get; set; }
	    public string menu_router { get; set; }
	    public int? menu_type { get; set; }
	    public string menu_icon_class { get; set; }
	    public string menu_name { get; set; }
	    public string menu_badge_style { get; set; }
	    public int? menu_parent_id { get; set; }
	    public string menu_description { get; set; }
	    public int? menu_order { get; set; }
	    public bool? menu_enabled { get; set; }
	    public bool? menu_customer_enabled { get; set; }
	    public bool? menu_vendor_enabled { get; set; }
	    public bool? menu_mobile_enabled { get; set; }
	    public string menu_mobile_router { get; set; }
	    public int? menu_mobile_router_type { get; set; }
	    public string menu_target { get; set; }
    }
}