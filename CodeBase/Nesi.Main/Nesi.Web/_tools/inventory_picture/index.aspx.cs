using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Web;
using nesi.core;

public partial class inventory_picture_index : System.Web.UI.Page
	{
	NeMember _member;
	NeBusinessUnit _company;
	private const int _page_id								= 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
		{
		var _tools											= new Toolbox();
		_member													= Toolbox.do_handle_authentication(_page_id);
		_company												= new NeBusinessUnit(_member.business_unit_id);
		var _q									= Request.QueryString;
		var func										= new this_functions();
		if(_q["barcode"] == null)
			{
			func.get_image();
			}
		else if(_q["showall"] != null)
			{
			Response.Clear();
			_tools.set_plain_header();
			var fi				= new InstalledFontCollection();
			foreach(var _f in fi.Families)
				{
				Response.Write(_f.Name+"\n");
				}
			Response.End();
			}
		else
			{
			func.get_barcode();
			}
		}
    public class this_functions
    {
        Toolbox _tools = new Toolbox();
        private HttpRequest _req = HttpContext.Current.Request;
        private HttpResponse _resp = HttpContext.Current.Response;
        private NameValueCollection _q = HttpContext.Current.Request.QueryString;
        bool is_master = true;

        public void get_image()
        {
            _resp.Clear();
            _resp.ClearContent();
            _resp.ClearHeaders();
            try
            {
                if (_q["is_master"] != null)
                {
                    is_master = Convert.ToBoolean(_q["is_master"]);
                }
                if (is_master)
                {
                    if (_q["id"] != null)
                    {
                        var _id = _q["id"];
                        if (Convert.ToInt32(_id) > 0)
                        {
                            _resp.ContentType = "Image/jpeg";
                            byte[] _image = null;
                            if (_tools.getSQL_int(@"SELECT COUNT(*) FROM inventory_picture  WHERE master_id =@v0 AND active", new object[] { _id }) > 0)
                            {
                                _image = _tools.getSQL_BLOB(@"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE master_id =@v0 AND active ORDER BY insert_dt DESC limit 1", new object[] { _id });

                                if (_image != null)
                                {
                                    _resp.BinaryWrite(_image);
                                }
                                else
                                {
                                    _image = _tools.getSQL_BLOB(@"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE master_id is null AND active", null);
                                    _resp.BinaryWrite(_image);
                                }
                            }
                            else
                            {
                                _image = _tools.getSQL_BLOB(@"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE master_id is null AND active", null);
                                _resp.BinaryWrite(_image);
                            }
                        }
                    }
                }
                else
                {
                    if (_q["id"] != null)
                    {
                        var _id = _q["id"];
                        if (Convert.ToInt32(_id) > 0)
                        {
                            _resp.ContentType = "Image/jpeg";
                            byte[] _image = null;
                            _image = _tools.getSQL_BLOB(@"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE id =@v0", new object[] { _id });
                            _resp.BinaryWrite(_image);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _resp.Write(ee.ToString());
            }
            _resp.End();
        }
        public void get_barcode()
        {
            _resp.Clear();
            _resp.ClearContent();
            _resp.ClearHeaders();
            _resp.ContentType = "Image/gif";
            _resp.AddHeader("Content-disposition", string.Format("filename=barcode-{0}.gif", _q["id"]));
            var BarCode = CreateBarcode(string.Format("*{0}*", _q["id"]));
            BarCode.Save(_resp.OutputStream, ImageFormat.Gif);
            _resp.End();
            BarCode.Dispose();
        }
        private Bitmap CreateBarcode(string data)
        {
            var barcode = new Bitmap(1, 1);
            var threeofnine = new Font("Free 3 of 9 Extended", 60, FontStyle.Regular, GraphicsUnit.Point);
            var graphics = Graphics.FromImage(barcode);
            var dataSize = graphics.MeasureString(data, threeofnine);
            barcode = new Bitmap(barcode, dataSize.ToSize());
            graphics = Graphics.FromImage(barcode);
            graphics.Clear(Color.White);
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString(data, threeofnine, new SolidBrush(Color.Black), 0, 0);
            graphics.Flush();
            threeofnine.Dispose();
            graphics.Dispose();
            return barcode;
        }
    }
}

