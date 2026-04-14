using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DataProcessor
{
    //public class Filter
    //{
    //    public string coulumnname;
    //    public string value;
    //    public OpertaionType Operation;



    //  public string  GetExpression()
    //    {

    //        switch (Operation)
    //        {
    //            case OpertaionType.Equals:

    //                return string.Format("{0} = '{1}'", coulumnname, value);

    //                break;
    //            case OpertaionType.NotEquals:
    //                return string.Format("{0} <> '{1}'", coulumnname, value);
    //                break;
    //            case OpertaionType.GreaterThan:
    //                return string.Format("{0} > '{1}'", coulumnname, value);
    //                break;
    //            case OpertaionType.LessThan:
    //                return string.Format("{0} < '{1}'", coulumnname, value);
    //                break;
    //            case OpertaionType.StartsWith:
    //                return string.Format("{0} Like '{1}%'", coulumnname, value);
    //                break;
    //            case OpertaionType.EndsWith:
    //                return string.Format("{0} Like '%{1}'", coulumnname, value);
    //                break;
    //            case OpertaionType.Contains:
    //                return string.Format("{0} Like '%{1}%'", coulumnname, value);
    //                break;
    //            case OpertaionType.NotContains:
    //                return string.Format("{0} Not Like '%{1}%'", coulumnname, value);
    //                break;
    //            default:
    //                break;
    //        }
    //        return null;
    //    }

    //    public Filter(string filter)
    //    {
    //        var arr=filter.Split(':');
    //        this.coulumnname = arr[0];
    //        this.value = arr[1];
    //        if (arr.Length==3)
    //        this.Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), arr[2]); 
          
    //    }
    //}
    //public enum OpertaionType
    //    {
    //    Equals,
    //    NotEquals,
    //    GreaterThan,
    //    LessThan,
    //    StartsWith,
    //    EndsWith,
    //    Contains,
    //    NotContains

    //    }
}
