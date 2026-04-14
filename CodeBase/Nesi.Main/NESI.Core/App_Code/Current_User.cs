using System;
using System.Reflection;
using System.Web;
using log4net;
using Microsoft.Ajax.Utilities;
using NESI.Common.Exceptions;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for Current_User
	/// </summary>
	public class Current_User
		{
		    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		    public string logo_icon
		        {
		        get
		            {
		            return HttpContext.Current != null && HttpContext.Current.Session != null &&
		                   HttpContext.Current.Session["global_logo_icon"] != null
		                ? HttpContext.Current.Session["global_logo_icon"].ToString()
		                : "";
		            }
		        set { HttpContext.Current.Session["global_logo_icon"] = value; }
		        }

		    public string logo_icon_full
		        {
		        get
		            {
		            return HttpContext.Current != null && HttpContext.Current.Session != null &&
		                   HttpContext.Current.Session["global_logo_icon_full"] != null
		                ? HttpContext.Current.Session["global_logo_icon_full"].ToString()
		                : "";
		            }
		        set { HttpContext.Current.Session["global_logo_icon_full"] = value; }
		        }

        public string visible_tax_entities
			{
			get
				{
				return HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["global_visible_tax_entities"] != null ? HttpContext.Current.Session["global_visible_tax_entities"].ToString() : "";
				}
			set
				{
				HttpContext.Current.Session["global_visible_tax_entities"] = value;
				}
			}

		
		public string visible_business_units
        {
			get
				{
				    if (HttpContext.Current != null && HttpContext.Current.Session != null &&
				        HttpContext.Current.Session["global_visible_business_units"] != null)
				    {
				        return HttpContext.Current.Session["global_visible_business_units"].ToString();
				    }
				    else
				    {
				        Logger.Debug($"Visible Business Units Not saved in session");
                        
                        //try to recover
				        try
				        {
				            var myMember = Toolbox.do_handle_authentication(1);
				            return HttpContext.Current.Session["global_visible_business_units"].ToString();
				        }
				        catch (Exception e)
				        {
				            throw new NesiException("Visible business units not set", e);
				        }
                        

				    }
                    

				}
			set
				{
				HttpContext.Current.Session["global_visible_business_units"] = value;
				}
			}
		

		public string visible_users
			{
			get
				{
				return HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["global_visible_users"] != null ? HttpContext.Current.Session["global_visible_users"].ToString() : "";
				}
			set
				{
				HttpContext.Current.Session["global_visible_users"] = value;
				}
			}

		    public string visible_reporting_users
		        {
		        get
		            {
		            return HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["global_visible_reporting_users"] != null ? HttpContext.Current.Session["global_visible_reporting_users"].ToString() : "";
		            }
		        set
		            {
		            HttpContext.Current.Session["global_visible_reporting_users"] = value;
		            }
		        }

        public string visible_reporting_users_all_status
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["global_visible_reporting_users_all_status"] != null ? HttpContext.Current.Session["global_visible_reporting_users_all_status"].ToString() : "";
            }
            set
            {
                HttpContext.Current.Session["global_visible_reporting_users_all_status"] = value;
            }
        }

        public Current_User()
			{
       
			//
			//  
			//
			}
		}
	}