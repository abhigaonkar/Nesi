using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using log4net;
using Enumerable = System.Linq.Enumerable;
using System.Net;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NeEMail
	/// </summary>
	public class NeEMail
	{
	    public string To { get; set; }
		public string CC { get; set; }
		public string Bcc { get; set; }
		public string From { get; set; }
        public string fromQuote { get; set; }
        public string Subject { get; set; }
		public string Body { get; set; }
		public bool isHTML { get; set; }
		public bool israting { get; set; }
		public string passport_expiry { get; set; }
		public string URLyes { get; set; }
		public string URLno { get; set; }
		public string yes_text { get; set; }
		public string no_text { get; set; }
		public ArrayList passport_array { get; set; }
		public int to_member_id { get; set; }
		public Attachment Attachment { get; set; }
		public List<LinkedResource> embedded_images { get; set; } = new List<LinkedResource>();
	    private static readonly ILog Logger =
	        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        public void Send()
		{
			var mail = new MailMessage();
			#region TO address(es)
			if (to_member_id != 0)
			{
				var user = new NeMember(to_member_id);
				To = user.isContact ? user.Email : user.NEEmail;
			}
			if (To.Contains(";"))
			{
				var addresses = To.Split(';');
				for (var a = 0; a < addresses.Length; a++)
				{
					if (Toolbox.CheckEmail(addresses[a]))
					{
						mail.To.Add(new MailAddress(addresses[a]));
					}
				}
			}
			else
			{
				if (To != "")
				{
					mail.To.Add(new MailAddress(To));
				}
				else { return; }
			}
			#endregion TO
			mail.Subject = Subject;
            //mail.From = string.IsNullOrWhiteSpace(From) ? new MailAddress("nomail@" + Toolbox.app_setting("DomainForEmail")) : new MailAddress(From);
            mail.From = new MailAddress(Toolbox.app_setting("Email_From"));
            #region CC address(es)
            if (!string.IsNullOrEmpty(CC))
			{
				if (CC.Contains(";"))
				{
					var addresses = CC.Split(';');
					for (var a = 0; a < addresses.Length; a++)
					{
						if (Toolbox.CheckEmail(addresses[a]))
						{
							mail.CC.Add(new MailAddress(addresses[a]));
						}
					}
				}
				else
				{
					try
					{
						mail.CC.Add(new MailAddress(CC));
					}
					catch { mail.CC.Clear(); }
				}
			}
			#endregion CC
			#region BCC address(es)
			if (!string.IsNullOrEmpty(Bcc))
			{
				if (Bcc.Contains(";"))
				{
					var addresses = Bcc.Split(';');
					for (var a = 0; a < addresses.Length; a++)
					{
						if (Toolbox.CheckEmail(addresses[a]))
						{
							mail.Bcc.Add(new MailAddress(addresses[a]));
						}
					}
				}
				else
				{
					try
					{
						mail.Bcc.Add(new MailAddress(Bcc));
					}
					catch { mail.Bcc.Clear(); }

				}
			}
            #endregion BCC


            var extra = "";
            if (Toolbox.app_setting("debug_redirect") == "1")
			{
				if (System.Web.HttpContext.Current == null)
				{
					return;
				}	
				cleanForRedirect(ref mail, ref extra,fromQuote);
			}
			else
			{
                ExtendEmailBody(ref mail, ref extra,fromQuote);
            }
            Body += extra;

            if (Attachment != null)
			{
				mail.Attachments.Add(Attachment);
			}

			var avHtml = AlternateView.CreateAlternateViewFromString(Body ?? "", null, MediaTypeNames.Text.Html);
			foreach (var lr in embedded_images)
			{
				avHtml.LinkedResources.Add(lr);
			}

			mail.AlternateViews.Add(avHtml);
			mail.IsBodyHtml = isHTML;
			var do_send = true;
			foreach (var a in mail.To)
			{
				if (do_send)
				{
					do_send = !a.Address.Contains("nomail@");
				}
			}
			if (do_send)
			{
				ThreadPool.QueueUserWorkItem(delegate { background_send(mail); });
			}
		}

		private async void Sendback()
		{
			var mail = new MailMessage();
			#region TO address(es)
			if (to_member_id != 0)
			{
				var user = new NeMember(to_member_id);
				To = user.isContact ? user.Email : user.NEEmail;
			}
			if (To.Contains(";"))
			{
				var addresses = To.Split(';');
				for (var a = 0; a < addresses.Length; a++)
				{
					if (Toolbox.CheckEmail(addresses[a]))
					{
						mail.To.Add(new MailAddress(addresses[a]));
					}
				}
			}
			else
			{
				if (To != "")
				{
					mail.To.Add(new MailAddress(To));
				}
				else { return; }
			}
			#endregion TO
			mail.Subject = Subject;
			//mail.From = string.IsNullOrWhiteSpace(From) ? new MailAddress("nomail@" + Toolbox.app_setting("DomainForEmail")) : new MailAddress(From);
			mail.From = new MailAddress(Toolbox.app_setting("Email_From"));
			#region CC address(es)
			if (!string.IsNullOrEmpty(CC))
			{
				if (CC.Contains(";"))
				{
					var addresses = CC.Split(';');
					for (var a = 0; a < addresses.Length; a++)
					{
						if (Toolbox.CheckEmail(addresses[a]))
						{
							mail.CC.Add(new MailAddress(addresses[a]));
						}
					}
				}
				else
				{
					try
					{
						mail.CC.Add(new MailAddress(CC));
					}
					catch { mail.CC.Clear(); }
				}
			}
			#endregion CC
			#region BCC address(es)
			if (!string.IsNullOrEmpty(Bcc))
			{
				if (Bcc.Contains(";"))
				{
					var addresses = Bcc.Split(';');
					for (var a = 0; a < addresses.Length; a++)
					{
						if (Toolbox.CheckEmail(addresses[a]))
						{
							mail.Bcc.Add(new MailAddress(addresses[a]));
						}
					}
				}
				else
				{
					try
					{
						mail.Bcc.Add(new MailAddress(Bcc));
					}
					catch { mail.Bcc.Clear(); }

				}
			}
			#endregion BCC
			if (Attachment != null)
			{
				mail.Attachments.Add(Attachment);
			}

            var extra = "";
            if (Toolbox.app_setting("debug_redirect") == "1")
			{
				cleanForRedirect(ref mail, ref extra,fromQuote);
			}
            else
            {
                ExtendEmailBody(ref mail, ref extra,fromQuote);
            }
            Body += extra;

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Body ?? "", null, MediaTypeNames.Text.Html);
			foreach (LinkedResource lr in embedded_images)
			{
				avHtml.LinkedResources.Add(lr);
			}

			mail.AlternateViews.Add(avHtml);
			mail.IsBodyHtml = isHTML;
			var do_send = true;
			foreach (var a in mail.To)
			{
				if (do_send)
				{
					do_send = !a.Address.Contains("nomail@");
				}
			}
			if (do_send)
			{
				background_send(mail);
			}
		}
		public void Send_Background()
		{
			//Thread myWorkThread = new Thread(Sendback);
			//myWorkThread.Start();

			Task.Run(() => Sendback());

		}
		private void background_send(MailMessage m)
		{
            try
            {
                //var smtp = new SmtpClient(Toolbox.app_setting("mx_address"))

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtp = new SmtpClient(Toolbox.app_setting("mx_address"), int.Parse(Toolbox.app_setting("smtp_port")));
                smtp.EnableSsl = bool.Parse(Toolbox.app_setting("smtp_enable_ssl"));
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Toolbox.app_setting("smtp_username"), Toolbox.app_setting("smtp_password"));

                smtp.Send(m);
                Toolbox.do_debug_note(string.Format("Email sent to: {0} - {1}",m.To, m.Subject));
            }
            catch (Exception ee)
            {

                Logger.Error($"Failed to send email", ee);
                //Logger.Error($"Cred: " + $"Username: {Toolbox.app_setting("smtp_username")}, " + $"Password: {Toolbox.app_setting("smtp_password")}, " + $"Port: {Toolbox.app_setting("smtp_port")}, " + $"MX Address: {Toolbox.app_setting("mx_address")}, " + $"Enable SSL: {Toolbox.app_setting("smtp_enable_ssl")}", ee);
                Toolbox.do_errorLog_errorStack(ee);
                Toolbox.do_debug_note(string.Format("Email not sent to: {0} - {1}", m.To, m.Subject));
                Toolbox.do_debug_note(ee);
				Toolbox.do_debug_note($"Cred: " + $"Username: {Toolbox.app_setting("smtp_username")}, " + $"Password: {Toolbox.app_setting("smtp_password")}, " + $"Port: {Toolbox.app_setting("smtp_port")}, " + $"MX Address: {Toolbox.app_setting("mx_address")}, " + $"Enable SSL: {Toolbox.app_setting("smtp_enable_ssl")}");
			}


        }
		public void file_passport()
		{
			if (passport_array == null)
			{
				passport_array = new ArrayList();
			}
			if (passport_array.Count == 0)
			{
				passport_array = new ArrayList();
				var pao = new passport.array_object() { url_yes = URLyes, url_no = URLno, text_yes = "Approve", text_no = "Deny" };
				passport_array.Add(pao);
			}

			if (yes_text == null)
			{
				yes_text = "Approve";
			}

			if (no_text == null)
			{
				no_text = "Deny";
			}

			foreach (passport.array_object pao in passport_array)
			{
				pao.reqhash = Toolbox.do_md5(256);
				var exists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM passport WHERE reqhash = @v0 ", new object[] { pao.reqhash });
				while (exists > 0)
				{
					pao.reqhash = Toolbox.do_md5(256);
					exists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM passport WHERE reqhash = @v0 ", new object[] { pao.reqhash });
				}
				var p = new passport
				{
					to = To,
					from = From,
					reqhash = pao.reqhash,
					urlyes = pao.url_yes,
					urlno = pao.url_no,
					to_member_id = to_member_id,
					passport_expiry = passport_expiry,
					yes_text = pao.text_yes,
					no_text = pao.text_no
				};
				p.save();
			}

			var mail = new MailMessage();
			mail.To.Add(To);
			if (!string.IsNullOrEmpty(CC))
			{
				mail.CC.Add(new MailAddress(CC));
			}
			mail.Subject = Subject;
			// Updating FROM to service Account
			//mail.From = new MailAddress(From);
			mail.From = new MailAddress(Toolbox.app_setting("Email_From"));
		    
											
			var body = new StringBuilder();
			body.AppendFormat(@"
<table width='100%' cellspacing='0' cellpadding='5'>
	<tr>
		<td style='font-family:arial;font-size:12px;'>
		{0}
		</td>
	</tr>
	
	<tr>
		<td>
			<table cellpadding='5' cellspacing='0'>", Body);

			foreach (passport.array_object pao in passport_array)
			{
				body.AppendFormat(@"
				<tr>
					<td align='center'>
<div>

 
  <table cellspacing='0' cellpadding='0'> <tr> 
  <td align='center' width='300' height='40' bgcolor='#289628' style='-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;'>
    <a href='{1}/passport.aspx?req={0}&type=approve' style='font-size:12px; font-weight: bold; font-family:sans-serif; text-decoration: none; line-height:40px; width:100%; display:inline-block'>
    <span style='color: #ffffff;'>
      {2}
    </span>
    </a>
  </td> 
  </tr> </table> 

</div>
					</td>
					<td align='center'>
<div>

  <table cellspacing='0' cellpadding='0'> <tr> 
  <td align='center' width='300' height='40' bgcolor='#d62828' style='-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; color: #ffffff; display: block;'>
    <a href='{1}/passport.aspx?req={0}&type=deny' style='font-size:12px; font-weight: bold; font-family:sans-serif; text-decoration: none; line-height:40px; width:100%; display:inline-block'>
    <span style='color: #ffffff;'>
      {3}
    </span>
    </a>
  </td> 
  </tr> </table> 
 
</div>
					</td>
				</tr>
				",
					pao.reqhash,
					Toolbox.app_setting("Domain"),
					pao.text_yes,
					pao.text_no
				);
			}
			body.Append(@"
			</table>
		</td>
	</tr>
<tr>
		<td>
			<ul style='font-family:arial;font-size:10px;color:#f00;'>
				<li>Using any button in this email will automatically log you into " + Toolbox.app_setting("Domain") + @" with a passport.</li>
			</ul>
		</td>
	</tr>
</table>");
			mail.IsBodyHtml = true;
			if (Attachment != null)
			{
				mail.Attachments.Add(Attachment);
			}
			//		mail.Body						= body.ToString();
			AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body.ToString(), null, MediaTypeNames.Text.Html);
			foreach (LinkedResource lr in embedded_images)
			{
				avHtml.LinkedResources.Add(lr);
			}

			mail.AlternateViews.Add(avHtml);
			mail.Priority = MailPriority.High;
            var extra = "";
			if (Toolbox.app_setting("debug_redirect") == "1")
			{
				cleanForRedirect(ref mail, ref extra,fromQuote);
			}
			else
			{
				ExtendEmailBody(ref mail, ref extra,fromQuote);
			}
			mail.Body += extra;
            ThreadPool.QueueUserWorkItem(delegate { background_send(mail); });
		}
		private void cleanForRedirect(ref MailMessage mail, ref string extra,string fromQuote)
		{
			if (!string.IsNullOrEmpty(fromQuote))
			{
				extra += "<br/> This email was originally SENT FROM: " + fromQuote;
			}
			if (mail.To.Count > 0)
			{
				extra += "<br/> This email was originally SENT to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.To, x => x.Address)));
			}
			if (mail.CC.Count > 0)
			{
				extra += "<br/> This email was originally CC'ed to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.CC, x => x.Address)));
			}
			if (mail.Bcc.Count > 0)
			{
				extra += "<br/> This email was originally BCC'ed to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.Bcc, x => x.Address)));
			}
			mail.To.Clear();
			mail.CC.Clear();
			mail.Bcc.Clear();
			var redirEmail = shared.properties.exists("debug_redirect_email")
								? new shared.properties("debug_redirect_email").value
								: Toolbox.app_setting("debug_redirect_email");

			mail.To.Add(redirEmail);
		}
        private void ExtendEmailBody(ref MailMessage mail, ref string extra, string fromQuote)
        {
			if (!string.IsNullOrEmpty(fromQuote))
			{
				extra += "<br/> This email was originally SENT FROM: " + fromQuote;
			}
			//if (mail.To.Count > 0)
   //         {
   //             extra += "<br/> This email was originally SENT to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.To, x => x.Address)));
   //         }
   //         if (mail.CC.Count > 0)
   //         {
   //             extra += "<br/> This email was originally CC'ed to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.CC, x => x.Address)));
   //         }
   //         if (mail.Bcc.Count > 0)
   //         {
   //             extra += "<br/> This email was originally BCC'ed to: " + string.Join(",", Enumerable.ToArray(Enumerable.Select(mail.Bcc, x => x.Address)));
   //         }
        }
    }
	public class passport
	{
		public class array_object
		{
			public string url_yes { get; set; }
			public string url_no { get; set; }
			public string text_yes { get; set; }
			public string text_no { get; set; }
			public string reqhash { get; set; }
			public bool process_all { get; set; }
		}
		private readonly Toolbox _tools = new Toolbox();
	    public string to { get; set; }

	    public string from { get; set; }

	    public bool active { get; set; }

	    public int valid { get; set; }

	    public string reqhash { get; set; }

	    public string urlyes { get; set; }

	    public string urlno { get; set; }

	    public string yes_text { get; set; }

	    public string no_text { get; set; }

	    public int to_member_id { get; set; }
		public string passport_expiry { get; set; } = null;

	    public passport()
		{
		}

		public passport(string _hash)
		{
			if (string.IsNullOrEmpty(_hash))
			{
				throw new Exception("Invalid Request");
			}
			reqhash = _hash;
			get();
		}
		private void get()
		{
			var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM passport WHERE reqhash = @v0 ", new object[] { reqhash });
			if (c > 0)
			{
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM passport WHERE reqhash = @v0 ", new object[] { reqhash }).Rows[0];
				to = dr["to"].ToString();
				@from = dr["from"].ToString();
				active = Convert.ToBoolean(dr["active"]);
				try
				{
					valid = Convert.ToInt32(dr["valid"]);
				}
				catch { }
				urlyes = dr["url_yes"].ToString();
				urlno = dr["url_no"].ToString();
				to_member_id = Convert.ToInt32(dr["to_member_id"]);
				passport_expiry = dr["expiry_date"] == DBNull.Value ? null : dr["expiry_date"].ToString();
			}
			else
			{
				throw new Exception("Invalid Request");
			}
		}

        /// <summary>
        /// Deactivate associated passports to a certain request
        /// </summary>
        /// <param name="page"><para>Provide a partial URL to search by like:</para><para>/sections/member/expense</para></param>
        /// <param name="id">Provide the associated module's id to deactivate</param>
        /// <returns></returns>
        public static Toolbox.boolstr DeactivatePassport(string page, int id)
        {
            var bs = new Toolbox.boolstr();
            var passCount = Toolbox.doSQL_int($@"SELECT COUNT(*) FROM passport WHERE url_yes LIKE '{page}%' AND url_yes LIKE '%{id}%'");
            if (passCount == 1)
            {
                Toolbox.doSQL_void($@"UPDATE passport SET active = 0 WHERE url_yes LIKE '{page}%' AND url_yes LIKE '%{id}%'");
                bs.success = true;
                bs.message = "Success";
            }
            else
            {
                bs.success = false;
                bs.message = "Passport Doesn't Exist";
            }

            return bs;
        }

        public bool check()
		{
			var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM passport WHERE reqhash = @v0  and active = 1", new object[] { reqhash });
			return (c > 0);
		}

		public void save()
		{
			var p_expiry = passport_expiry == null || (passport_expiry != null && passport_expiry.Trim() == "")
				? "NULL"
				: passport_expiry;
			_tools.getSQL_void(@"
INSERT INTO passport
	(
	`dt`,
	`to`, 
	`from`, 
	`active`, 
	`reqhash`,
	`valid`,
	`url_yes`, 
	`url_no`,
	`to_member_id`,
	`expiry_date`,
    `yes_text`,
    `no_text`
	) 
VALUES 
	(
	NOW(),
	@v0, 
	@v1, 
	true, 
	@v2,
	-1,
	@v3, 
	@v4,
	@v5,
	@v6,
    @v7,
    @v8
	)",
	new object[] {
				to, // {0}
				@from, // {1}
				reqhash, // {2} 
				urlyes, // {3}
				urlno, // {4}
				to_member_id, // {5}
				p_expiry, // {6}
                yes_text,
				no_text
			});
		}

		public string process(bool which)
		{
			if (string.IsNullOrEmpty(reqhash)) { throw new Exception("Invalid Request"); }
			var column = which ? "url_yes" : "url_no";

            if (!string.IsNullOrEmpty(passport_expiry))
            {
                if ((Convert.ToDateTime(passport_expiry) < DateTime.Today))
                {
                    _tools.getSQL_void(@"UPDATE passport SET active = false, valid = @v0 WHERE reqhash = @v1 LIMIT 1", new object[] { which, reqhash });
                }
            }

			return _tools.getSQL_string($@"SELECT {column}  FROM passport WHERE reqhash = @v0  LIMIT 1", new object[] { reqhash });
		}
	}
}

//using System.Net.Mail;