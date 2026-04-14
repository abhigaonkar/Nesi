using NESI.Common.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public class Filter
    {
        public string coulumnname;
        public object value;
        public OpertaionType Operation;
        private Dictionary<string, string> simpleTypeValues = new Dictionary<string, string>();
        private Dictionary<string,string> simpleTypes = new Dictionary<string, string>();
        private const string COMPLEX_DATETIME = "complexDateTime";
        public string filterType;

        enum EnmDateValue
        {
            localDateTimeYesterday,
            localDateTimeToday,
            localDateTimeTomorrow,
            localDateTimeLastWeek,
            localDateTimeThisWeek,
            localDateTimeNextWeek,
            localDateTimeTwoWeeksAway,
            localDateTimeThisMonth,
            localDateTimeNextMonth,
            localDateTimeThisYear,
            localDateTimeNextYear,
            LocalDateTimeDayAfterTomorrow
        }

        enum EnmDateFunction
        {
            addMonths,
            addYears
        }

        private string ExpressionQuery()
        {
            if (!String.IsNullOrEmpty(filterType))
                if (filterType == "boolean")
                {
                    var strVal = Convert.ToString(value);
                    value = (strVal.ToLower() == "checked" || strVal.ToLower() == "true" || strVal.ToLower() == "1") ? 1 : 0;

                }


            switch (Operation)
            {
                case OpertaionType.equals:
                    {
                        DateTime dtTmp = DateTime.Today;
                        bool result;

                        if(!String.IsNullOrEmpty(filterType))
                            if (filterType.ToLower() == "datetime")
                            {
                                DateTime.TryParse(Convert.ToString(value), out dtTmp);
                                var lowerBound = dtTmp;
                                var upperBound = dtTmp.AddDays(1);
                                var combinedFilter =
                                    $"{coulumnname} >= '{Convert.ToString(lowerBound)}' AND {coulumnname} < '{Convert.ToString(upperBound)}'"; //(String.Format("{0} >= {1}", coulumnname, lowerBound.ToString()) + " AND " + String.Format("{0} < {1}", coulumnname, upperBound.ToString()));
                                return combinedFilter;
                            }

                        if (value == null)
                        {
                            return $"{coulumnname} is null ";
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(value)))
                        {
                            return $"({coulumnname} = '') or ({coulumnname} is null ) ";
                        }
                        else if (bool.TryParse(Convert.ToString(value), out result))
                        {
                            return $"{coulumnname} = {(result ? true : false)}";
                        }
                        else
                            return $"{coulumnname} = '{value.ToString().Replace("'", "''")}'";


                    }
                case OpertaionType.greaterThan:
                    return $"{coulumnname} > '{value.ToString().Replace("'", "''")}'";

                case OpertaionType.lessThan:
                    return $"{coulumnname} < '{value.ToString().Replace("'", "''")}'";

                case OpertaionType.startsWith:
                    return $"{coulumnname} Like '{value.ToString().Replace("'", "''")}%'";

                case OpertaionType.endsWith:
                    return $"{coulumnname} Like '%{value.ToString().Replace("'", "''")}'";

                case OpertaionType.contains:
                    return $"{coulumnname} Like '%{value.ToString().Replace("'", "''")}%'";
                case OpertaionType.notContains:
                    return $"{coulumnname} Not Like '%{value.ToString().Replace("'", "''")}%'";
                case OpertaionType.greaterThanOrEqual:
                    return $"{coulumnname} >= '{value.ToString().Replace("'", "''")}'";
                case OpertaionType.notEquals:
                    return $"{coulumnname} <> '{value.ToString().Replace("'", "''")}'";
                case OpertaionType.lessThanOrEqual:
                    return $"{coulumnname} <= '{value.ToString().Replace("'", "''")}'";
                default:
                    break;
            }
            return null;
        }

        private string HandleComplexDateExpression()
        {

            return "";
        }
        public string GetExpression()
        {
            List<string> str = new List<string>();

            if (value != null)
            {
                str = Convert.ToString(value).Split('^').ToList();
            }

            if (Operation == OpertaionType.complexDateTime)
            {
                if (str.Count == 1)
                {
                    var flVal1 = Convert.ToString(str[0]).Split(new String[] { " " }, StringSplitOptions.None);
                    DateTime dtTmp;
                    Filter fl1 = null;

                    if (DateTime.TryParse(flVal1[1], out dtTmp))
                    {
                        fl1 = new Filter()
                        {
                            coulumnname = coulumnname,
                            Operation = (OpertaionType) Enum.Parse(typeof(OpertaionType), flVal1[0]),
                            value = flVal1[1]
                        };

                        if ((OpertaionType) Enum.Parse(typeof(OpertaionType), flVal1[0]) == OpertaionType.lessThan)
                        {
                            var qry = fl1.ExpressionQuery();
                            qry = qry + " OR " + coulumnname + " IS NULL ";
                            return qry;
                        }
                        else
                            return fl1.ExpressionQuery();
                    }
                    else
                    {
                        fl1 = new Filter()
                        {
                            coulumnname = coulumnname,
                            Operation = (OpertaionType) Enum.Parse(typeof(OpertaionType), flVal1[0]),
                            value = GetDateTime((EnmDateValue) Enum.Parse(typeof(EnmDateValue), flVal1[1], true))
                        };
                        return fl1.ExpressionQuery();
                    }
                    
                }
                else
                {
                    var flVal1 = Convert.ToString(str[0]).Split(new String[] { " " }, StringSplitOptions.None);
                    var flVal2 = Convert.ToString(str[1]).Split(new String[] { " " }, StringSplitOptions.None);

                    Filter fl1, fl2;
                    DateTime dtTmp;
                    EnmDateFunction enDtFunc;
                    DateTime dateVal1 = DateTime.Today;
                    DateTime dateVal2 = DateTime.Today;

                    if (DateTime.TryParse(flVal1[1], out dtTmp))
                    {
                        dateVal1 = dtTmp;
                    }
                    else
                    {
                        if (flVal1[1].IndexOf('(') > -1)
                        {
                            var strFunc = Convert.ToString(flVal1[1]).Substring(0, flVal1[1].IndexOf('('));
                            if (Enum.TryParse(strFunc, true, out enDtFunc))
                            {
                                switch (enDtFunc)
                                {
                                    case EnmDateFunction.addMonths:
                                        {
                                            var arrParam = Convert.ToString(flVal1[1]).Substring(flVal1[1].IndexOf('(') + 1,
                                                (flVal1[1].Length - flVal1[1].IndexOf('(') - 2)).Split(',');
                                            dateVal1 = AddMonth(
                                                GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
                                                    arrParam[0], true)), Convert.ToInt32(arrParam[1]));
                                        }
                                        break;
                                    case EnmDateFunction.addYears:
                                        {
                                            var arrParam = Convert.ToString(flVal1[1]).Substring(flVal1[1].IndexOf('(') + 1,
                                                (flVal1[1].Length - flVal1[1].IndexOf('(') - 2)).Split(',');
                                            dateVal1 = AddYear(
                                                GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
                                                    arrParam[0], true)), Convert.ToInt32(arrParam[1]));
                                        }
                                        break;
                                }
                            }
                        }
                        else
                            dateVal1 = GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue), flVal1[1], true));
                    }

                    if (DateTime.TryParse(flVal2[1], out dtTmp))
                    {
                        dateVal2 = dtTmp;
                    }
                    else
                    {
                        if (flVal2[1].IndexOf('(') > -1)
                        {
                            var strFunc = Convert.ToString(flVal2[1]).Substring(0, flVal2[1].IndexOf('('));
                            if (Enum.TryParse(strFunc, true, out enDtFunc))
                            {
                                switch (enDtFunc)
                                {
                                    case EnmDateFunction.addMonths:
                                        {
                                            var arrParam = Convert.ToString(flVal2[1]).Substring(flVal2[1].IndexOf('(') + 1,
                                                (flVal2[1].Length - flVal2[1].IndexOf('(') - 2)).Split(',');
                                            dateVal2 = AddMonth(
                                                GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
                                                    arrParam[0], true)), Convert.ToInt32(arrParam[1]));
                                        }
                                        break;
                                    case EnmDateFunction.addYears:
                                        {
                                            var arrParam = Convert.ToString(flVal2[1]).Substring(flVal2[1].IndexOf('(') + 1,
                                                (flVal2[1].Length - flVal2[1].IndexOf('(') - 2)).Split(',');
                                            dateVal2 = AddYear(
                                                GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
                                                    arrParam[0], true)), Convert.ToInt32(arrParam[1]));
                                        }
                                        break;
                                }
                            }
                        }
                        else
                            dateVal2 = GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue), flVal2[1], true));
                    }

                    fl1 = new Filter() { coulumnname = coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal1[0]), value = dateVal1, filterType = this.filterType };
                    fl2 = new Filter() { coulumnname = coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal2[0]), value = dateVal2, filterType = this.filterType };

                    return ($"({fl1.GetExpression()} AND {fl2.GetExpression()})");
                }
            }
            else
            {
                if (str.Count > 1)
                {
                    string result = "";
                    foreach (var p in str)
                    {
                        result += (string.IsNullOrEmpty(result)
                            ? new Filter() { coulumnname = this.coulumnname, Operation = this.Operation, value = p, filterType = this.filterType }
                                .GetExpression()
                            : " OR " + new Filter() { coulumnname = this.coulumnname, Operation = this.Operation, value = p, filterType = this.filterType }
                                  .GetExpression());
                    }
                    return "(" + result + ")";
                }
                else
                    return ExpressionQuery();
            }

        }
        public Filter()
        {
            this.populateSimpleTypes();
            this.populateSimpleTypeValues();
        }
        public Filter(string filter, bool ignorecase = true)
        {
            var arr = filter.Split(new[] { "||" }, StringSplitOptions.None);
            this.coulumnname = arr[0];
            if (arr.Length > 1)
            {
                this.value = ignorecase ? arr[1].ToLower() : (arr[1].ToLower() == "null" ? null : arr[1]);
            }
            if (arr.Length == 3)
                this.Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), arr[2]);


        }

        public string ConvertStringToQuery(string filterBuilderString, IReport reportSchema)
        {
            //string query = "";
            //var replacedString = "";
            StringBuilder sb = new StringBuilder(filterBuilderString);
            var index0 = filterBuilderString.IndexOf("|", StringComparison.Ordinal);
            
            if(index0 == -1)
            {
                return filterBuilderString;
            }
            var index1 = filterBuilderString.IndexOf("^", StringComparison.Ordinal);
            var columnName = filterBuilderString.Substring(index0 + 1, (index1 - index0 - 1));
            var columnType = GetColumnType(columnName, reportSchema);
            sb[index0]= ' ';
            var index2 = filterBuilderString.IndexOf("^", index1 + 1, StringComparison.Ordinal);
            var index3 = filterBuilderString.IndexOf("^", index2 + 1, StringComparison.Ordinal);

            var operandToReplace = filterBuilderString.Substring(index1 + 1, (index2 - index1-1));
            var operandToReplaceWith = simpleTypes[operandToReplace];

            var actualValue = filterBuilderString.Substring(index2 + 1, (index3 - index2 - 1));
            
            var valueToReplace = simpleTypeValues[operandToReplace];
            var finalValue = "";
            var length = 0;
            bool changed = false;
            if (!string.IsNullOrEmpty(actualValue))
            {
	            switch (columnType.ToLower())
	            {
		            case "boolean":
			            finalValue = valueToReplace.Replace(operandToReplace.ToLower() == "in" ? "#" : "'#'", actualValue);
			            break;
		            case "datetime":
			            var dateVal = DateTime.Today;

			            if (actualValue.IndexOf('(') > -1)
			            {

				            var dateFunction = Convert.ToString(actualValue).Substring(0, actualValue.IndexOf('('));
				            if (Enum.TryParse(dateFunction, true, out EnmDateFunction enDtFunc))
				            {
					            switch (enDtFunc)
					            {
						            case EnmDateFunction.addMonths:
						            {
							            var arrParam = Convert.ToString(actualValue).Substring(actualValue.IndexOf('(') + 1,
								            (actualValue.Length - actualValue.IndexOf('(') - 2)).Split(',');
							            dateVal = AddMonth(
								            GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
									            arrParam[0], true)), Convert.ToInt32(arrParam[1]));
						            }
							            break;
						            case EnmDateFunction.addYears:
						            {
							            var arrParam = Convert.ToString(actualValue).Substring(actualValue.IndexOf('(') + 1,
								            (actualValue.Length - actualValue.IndexOf('(') - 2)).Split(',');
							            dateVal = AddYear(
								            GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue),
									            arrParam[0], true)), Convert.ToInt32(arrParam[1]));
						            }
							            break;
						            default:
										break;
							           // throw new ArgumentOutOfRangeException();
					            }
					            finalValue = valueToReplace.Replace("#", Convert.ToString(dateVal, CultureInfo.InvariantCulture));
				            }
			            }
			            else
			            {
				            var dateValues = Enum.GetNames(typeof(EnmDateValue)).ToList();
				            var isExist = dateValues.Exists(d => d.ToLower().Equals(actualValue.ToLower()));
				            if (isExist)
				            {
					            var val = GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue), actualValue, true));

					            finalValue = valueToReplace.Replace("#", Convert.ToString(val, CultureInfo.InvariantCulture));
				            }
				            else
				            {
					            if (operandToReplace.ToLower().Equals("equals"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var lowerBound = dtTmp;
							            var upperBound = dtTmp.AddDays(1);

							            var combinedFilter = $" >= '#{Convert.ToString(lowerBound, CultureInfo.InvariantCulture)}#' And {columnName} < '#{Convert.ToString(upperBound, CultureInfo.InvariantCulture)}#'";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else if (operandToReplace.ToLower().Equals("notequals"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var lowerBound = dtTmp;
							            var upperBound = dtTmp.AddDays(1);
							            // var combinedFilter = $" > '#1900-1-1#' And ( {columnName} < '#{Convert.ToString(lowerBound)}#' Or {columnName} >= '#{Convert.ToString(upperBound)}#')";
							            var combinedFilter = $" = {columnName} And ( {columnName} < '#{Convert.ToString(lowerBound, CultureInfo.InvariantCulture)}#' Or {columnName} >= '#{Convert.ToString(upperBound, CultureInfo.InvariantCulture)}#')";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else if (operandToReplace.ToLower().Equals("lessthanorequal"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var lowerBound = dtTmp;
							            var upperBound = dtTmp.AddDays(1);
							            var combinedFilter = $" < '#{upperBound}#'";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else if (operandToReplace.ToLower().Equals("lessthan"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var combinedFilter = $" < '#{dtTmp}#'";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else if (operandToReplace.ToLower().Equals("greaterthan"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var upperBound = dtTmp.AddDays(1);
							            var combinedFilter = $" >= '{upperBound}'";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else if (operandToReplace.ToLower().Equals("greaterthanorequal"))
					            {
						            if (DateTime.TryParse(Convert.ToString(actualValue), out var dtTmp))
						            {
							            var upperBound = dtTmp.AddDays(-1);
							            var combinedFilter = $" >= '#{dtTmp}#'";
							            operandToReplaceWith = combinedFilter;
						            }
					            }
					            else
					            {
						            finalValue = valueToReplace.Replace("#", !string.IsNullOrEmpty(actualValue) ? Convert.ToString(actualValue) :
							            Convert.ToString(DateTime.Now.ToString(CultureInfo.InvariantCulture)));
					            }

				            }
			            }
			            break;
		            default:
                        if (columnType.ToLower() == "string" && (operandToReplace == "startsWith" || operandToReplace == "contains" || operandToReplace == "notContains" || operandToReplace == "endsWith" || operandToReplace == "equals" || operandToReplace == "notEquals"))
                        {
                            if (actualValue.Contains("'"))
                            {
                                length = actualValue.Length;
                                actualValue = actualValue.Replace("'", "''");
                                finalValue = valueToReplace.Replace("#", actualValue);
                                changed = true;
                                break;
                            }

                            if (actualValue.Contains("["))
                            {
                                length = actualValue.Length;
                                actualValue = actualValue.Replace("[", "[[]");
                                finalValue = valueToReplace.Replace("#", actualValue);
                                changed = true;
                                break;
                            }

                            if (actualValue.Contains("]"))
                            {
                                length = actualValue.Length;
                                actualValue = actualValue.Replace("]", "[]]");
                                finalValue = valueToReplace.Replace("#", actualValue);
                                changed = true;
                                break;
                            }

                            if (actualValue.Contains("*"))
                            {
                                length = actualValue.Length;
                                actualValue = actualValue.Replace("*", "[*]");
                                finalValue = valueToReplace.Replace("#", actualValue);
                                changed = true;
                                break;
                            }
                        }

                        if (valueToReplace.Contains("%") && actualValue.StartsWith("'") && actualValue.EndsWith("'"))
			            {
				            actualValue=actualValue.Trim('\'');
			            }

                        finalValue = valueToReplace.Replace("#", actualValue);
			            break;
	            }
            }
            else
            {
                if (operandToReplace != "isBlank" && operandToReplace != "isNotBlank")
                {
                    operandToReplaceWith = "is";
                    finalValue = "null";
                }

                if (operandToReplace != "isBlank" && operandToReplace != "isNotBlank" && columnType.ToLower() == "string")
                {
                    operandToReplaceWith = "<>";
                    finalValue = "";
                }

                if (columnType.ToLower() == "string")
                {
                    if (operandToReplace == "isBlank")
                    {
                        finalValue = "(" + columnName + "='' OR " + columnName + " is null) ";
                        // finalValue = "'' OR " + columnName + " is null";
                    }

                    if (operandToReplace == "isNotBlank")
                    {
                        finalValue = "(" + columnName + "<>'' AND " + columnName + " is not null) ";
                        // finalValue = "'' AND " + columnName + " is not null ";
                    }

                    if (operandToReplace == "notEquals")
                    {
                        finalValue = "(" + columnName + "<>'' AND " + columnName + " is not null) ";
                    }
                }

                if (columnType.ToLower() == "datetime")
                {
                    if (operandToReplace == "isBlank")
                    {
                        finalValue = "(" + columnName + " is null) ";
                        // finalValue = "'' OR " + columnName + " is null";
                    }

                    if (operandToReplace == "isNotBlank")
                    {
                        finalValue = "(" + columnName + " is not null) ";
                        // finalValue = "'' AND " + columnName + " is not null ";
                    }

                    if (operandToReplace == "notEquals")
                    {
                        finalValue = "(" + columnName + " is not null) ";
                        // finalValue = "'' AND " + columnName + " is not null ";
                    }
                }
            }


            sb[index1] = ' ';
            sb[index2] = ' ';
            sb.Remove(index1 + 1, (index2 - index1 - 1));
            sb.Insert(index1 + 1, operandToReplaceWith);

            filterBuilderString = sb.ToString();
            var valueEndIndex = filterBuilderString.IndexOf("^", StringComparison.Ordinal);

            if (columnType.ToLower() == "boolean")
            {
                if (!finalValue.ToLower().Equals("null"))
                {
                    if (!operandToReplace.ToLower().Equals("in"))
                    {
                        finalValue = (finalValue.ToLower() == "true" || finalValue.ToLower() == "checked" || finalValue.ToLower() == "1") ? "1" : "0";
                    }
                }
                sb = new StringBuilder(filterBuilderString);
                sb.Remove((valueEndIndex - actualValue.Length), actualValue.Length + 1);
                sb.Insert((valueEndIndex - actualValue.Length), finalValue);

            }
            else if (columnType.ToLower() == "datetime" && (operandToReplace.ToLower() == "equals" || operandToReplace.ToLower() == "notequals"))
            {
                if (finalValue != "null")
                {
                    sb = new StringBuilder(filterBuilderString);
                    sb.Remove((valueEndIndex - actualValue.Length), actualValue.Length + 1);
                }
                else
                {
                    sb = new StringBuilder(filterBuilderString);
                    sb.Remove((valueEndIndex - actualValue.Length), actualValue.Length + 1);

                    sb.Insert((valueEndIndex - actualValue.Length), finalValue);
                }
            }
            else
            {
                sb = new StringBuilder(filterBuilderString);

                if (changed)
                {
                    sb.Remove((valueEndIndex - length), length + 1);
                    sb.Insert((valueEndIndex - length), finalValue);
                }
                else
                {
                    if (columnType.ToLower() == "string")
                    {
                        if (operandToReplace == "isBlank")
                        {
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }

                        if (operandToReplace == "isNotBlank")
                        {
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }

                        if (operandToReplace == "notEquals" && actualValue == "")
                        {
                            
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }
                    }

                    if (columnType.ToLower() == "datetime")
                    {
                        if (operandToReplace == "isBlank")
                        {
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }

                        if (operandToReplace == "isNotBlank")
                        {
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }

                        if (operandToReplace == "notEquals" && actualValue == "")
                        {
                            var len = columnName.Length + " ".Length + operandToReplaceWith.Length + " ".Length;
                            sb.Remove((valueEndIndex - len), len + 1);
                            sb.Insert((valueEndIndex - len), finalValue);
                        }
                    }

                    if (operandToReplace != "isBlank" && operandToReplace != "isNotBlank" && (operandToReplace != "notEquals" || actualValue != ""))
                    {
                        sb.Remove((valueEndIndex - actualValue.Length), actualValue.Length + 1);
                        sb.Insert((valueEndIndex - actualValue.Length), finalValue);
                    }

                }
            }

            


            filterBuilderString = sb.ToString();


            filterBuilderString = ConvertStringToQuery(filterBuilderString, reportSchema);
            

            
            return filterBuilderString;
        }

        private string GetColumnType(string columnName, IReport reportSchema)
        {
            var columnType = "";
            var schemaCol = reportSchema.columns.First(sc => string.Equals(sc.name, columnName, StringComparison.CurrentCultureIgnoreCase));
            columnType = schemaCol.rawType;

            return columnType;
        }

        public static string EscapeLikeValue(string valueWithoutWildcards)
        {
            var sb = new StringBuilder();
            foreach (char c in valueWithoutWildcards)
            {
	            switch (c)
	            {
		            case '[':
			            sb.Append("[").Append(c).Append("]");
			            break;
                    case ']':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '%':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
			            sb.Append("''");
			            break;
		            default:
			            sb.Append(c);
			            break;
	            }
            }
            return sb.ToString();
        }

        private object GetFormattedValue(string _value, string columnType)
        {
            var val = new object();
            switch (columnType.ToLower())
            {
	            case "boolean":
		            val = (_value.ToLower() == "true" || _value.ToLower() == "1") ? true : false;
		            break;
	            case "string":
	            case "email":
	            case "notes":
	            case "textbox":
	            case "phone":
		            val = "'" + _value + "'";
		            break;
	            case "number":
		            val = int.TryParse(_value, out var num) ? num : 0;
		          //  val = value;
		            break;
	            case "currency":
		            val = decimal.TryParse(_value, out var dc) ? (object) dc : 0.0f;
		            // val = value;
		            break;
	            case "datetime":
		            val = DateTime.TryParse(_value, out var dt) ? dt : DateTime.Now;
		            //val = value;
		            break;
            }

            return val;
        }

        private static string GetReplaceWith(string replaceTo)
        {
            string replaceWith = string.Empty;
            switch (replaceTo)
            {
                case "startsWith":
                case "endsWith":
                case "contains":
                    replaceWith = " Like ";
                    break;
                default:
                    break;
            }

            return replaceWith;
        }

        public static string SafeReplace(string input, string toReplace, string replaceWith, bool matchWholeWord)
        {
            string textToFind = matchWholeWord ? $@"\b{toReplace}\b" : toReplace;
            return input.Replace(toReplace, replaceWith);
            //return Regex.Replace(input, textToFind, replaceWith);
        }

        private static DateTime GetDateTime(EnmDateValue datePart)
        {
            switch (datePart)
            {
                case EnmDateValue.localDateTimeYesterday:
                    return DateTime.Today.AddDays(-1);
                case EnmDateValue.localDateTimeToday:
                    return DateTime.Today;
                case EnmDateValue.localDateTimeTomorrow:
                    return DateTime.Today.AddDays(1);
                case EnmDateValue.localDateTimeLastWeek:
                    return DateTime.Today.AddDays(-(int)DateTime.Now.DayOfWeek - 7);
                case EnmDateValue.localDateTimeThisWeek:
                    return DateTime.Today.AddDays(-(int)DateTime.Now.DayOfWeek - 1);
                case EnmDateValue.localDateTimeNextWeek:
                    return DateTime.Today.AddDays(-(int)DateTime.Now.DayOfWeek + 7);
                case EnmDateValue.localDateTimeTwoWeeksAway:
                    return DateTime.Today.AddDays(-(int)DateTime.Now.DayOfWeek + 13);
                case EnmDateValue.localDateTimeThisMonth:
                    return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                case EnmDateValue.localDateTimeNextMonth:
                    {
                        var firstDayOfMonth = GetDateTime(EnmDateValue.localDateTimeThisMonth);
                        return firstDayOfMonth.AddMonths(1);
                    }
                case EnmDateValue.localDateTimeThisYear:
                    return new DateTime(DateTime.Today.Year, 1, 1);
                case EnmDateValue.localDateTimeNextYear:
                    {
                        var firstDayOfYear = GetDateTime(EnmDateValue.localDateTimeThisYear);
                        return firstDayOfYear.AddYears(1);
                    }
                case EnmDateValue.LocalDateTimeDayAfterTomorrow:
                    {
                        var dateTomorrow = GetDateTime(EnmDateValue.localDateTimeTomorrow);
                        return dateTomorrow.AddDays(1);
                    }
                default:
                    return DateTime.Today;
            }
        }

        private static DateTime AddMonth(DateTime dt, int val)
        {
            return dt.AddMonths(val);
        }

        private static DateTime AddYear(DateTime dt, int val)
        {
            return dt.AddYears(val);
        }

        private void populateSimpleTypeValues()
        {
            simpleTypeValues.Clear();

            simpleTypeValues.Add("equals", "'#'");
            simpleTypeValues.Add("greaterThan", "'#'");
            simpleTypeValues.Add("lessThan", "'#'");
            simpleTypeValues.Add("startsWith", "'#%'");
            simpleTypeValues.Add("endsWith", "'%#'");
            simpleTypeValues.Add("contains", "'%#%'");
            simpleTypeValues.Add("notContains", "'%#%'");
            simpleTypeValues.Add("greaterThanOrEqual", "'#'");
            simpleTypeValues.Add("notEquals", "'#'");
            simpleTypeValues.Add("lessThanOrEqual", "'#'");
            simpleTypeValues.Add("in", "(#)");
	        simpleTypeValues.Add("not in", "(#)");


            simpleTypeValues.Add("isBlank", "'#'");
            simpleTypeValues.Add("isNotBlank", "'#'");
        }

        private void populateSimpleTypes()
        {
            simpleTypes.Clear();

            
            simpleTypes.Add("startsWith", "Like");
            simpleTypes.Add("endsWith", "Like");
            simpleTypes.Add("contains", "Like");
            simpleTypes.Add("notContains", "Not Like");
            simpleTypes.Add("in", "In");
	        simpleTypes.Add("not in", "Not In");

			simpleTypes.Add("equals", "=");
            simpleTypes.Add("notEquals", "<>");
            simpleTypes.Add("greaterThan", ">");
            simpleTypes.Add("greaterThanOrEqual", ">=");
            simpleTypes.Add("lessThan", "<");
            simpleTypes.Add("lessThanOrEqual", "<=");

            simpleTypes.Add("isBlank", "=");
            simpleTypes.Add("isNotBlank", "<>");

            simpleTypes.Add("localDateTimeYesterday", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeToday", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeTomorrow", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeLastWeek", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeThisWeek", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeNextWeek", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeTwoWeeksAway", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeThisMonth", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeNextMonth", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeThisYear", COMPLEX_DATETIME);
            simpleTypes.Add("localDateTimeNextYear", COMPLEX_DATETIME);
            simpleTypes.Add("LocalDateTimeDayAfterTomorrow", COMPLEX_DATETIME);
        }
        
    }

    public enum OpertaionType
    {
        equals,
        greaterThan,
        lessThan,
        greaterThanOrEqual,
        lessThanOrEqual,
        contains,
        startsWith,
        endsWith,
        notEquals,
        notContains,
        complexDateTime
    }
}
