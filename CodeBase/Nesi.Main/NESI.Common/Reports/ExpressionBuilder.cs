using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public static class ExpressionBuilder
    {
        private static MethodInfo miTL = typeof(String).GetMethod("ToLower", Type.EmptyTypes);
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

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
        public static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters, bool OrOperation = false)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp1 = null;
            Expression exp2 = null;

            //To Handle complex date scenario
            var filterComplexDateTime = filters.Where(x => x.Operation == OpertaionType.complexDateTime).ToList();

            //re-initialize list without complexDateTime
            filters = filters.Where(x => x.Operation != OpertaionType.complexDateTime).ToList();

            if (filterComplexDateTime.Count > 0)
            {
                int i = 0;

                foreach (Filter fl in filterComplexDateTime)
                {
                    var arrVal = fl.value.ToString().Split('^');

                    if (arrVal.Length == 1)
                    {
                        var flVal1 = Convert.ToString(arrVal[0]).Split(new String[] { " " }, StringSplitOptions.None);
                        DateTime dtTmp;
                        Filter fl1 = null;

                        if (DateTime.TryParse(flVal1[1], out dtTmp))
                            fl1 = new Filter() { coulumnname = fl.coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal1[0]), value = flVal1[1] };
                        else
                            fl1 = new Filter() { coulumnname = fl.coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal1[0]), value = GetDateTime((EnmDateValue)Enum.Parse(typeof(EnmDateValue), flVal1[1])) };


                        if (i == 0)
                        {
                            exp1 = GetExpression<T>(param, fl1);
                        }
                        else
                        {
                            var exp5 = GetExpression<T>(param, fl1);
                            var tmp = Expression.Or(exp1, exp5);
                            exp1 = tmp;
                        }
                    }
                    else
                    {
                        var flVal1 = Convert.ToString(arrVal[0]).Split(new String[] { " " }, StringSplitOptions.None);
                        var flVal2 = Convert.ToString(arrVal[1]).Split(new String[] { " " }, StringSplitOptions.None);

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
                            
                        fl1 = new Filter() { coulumnname = fl.coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal1[0]), value = dateVal1 };
                        fl2 = new Filter() { coulumnname = fl.coulumnname, Operation = (OpertaionType)Enum.Parse(typeof(OpertaionType), flVal2[0]), value = dateVal2 };

                        if (i == 0)
                        {
                            exp1 = GetExpression<T>(param, fl1, fl2);
                        }
                        else
                        {
                            var exp5 = GetExpression<T>(param, fl1, fl2);
                            var tmp = Expression.Or(exp1, exp5);
                            exp1 = tmp;
                        }
                    }

                    i++;
                }
            }

            //To handle other scenarios
            if (filters.Count > 0)
            {
                if (filters.Count == 1)
                    exp2 = GetExpression<T>(param, filters[0]);
                else if (filters.Count == 2)
                    exp2 = GetExpression<T>(param, filters[0], filters[1]);
                else
                {
                    while (filters.Count > 0)
                    {
                        var f1 = filters[0];
                        var f2 = filters[1];

                        if (exp2 == null)
                        {
                            exp2 = GetExpression<T>(param, filters[0], filters[1], OrOperation);
                        }
                        else
                        {

                            if (OrOperation == false)
                                exp2 = Expression.AndAlso(exp2, GetExpression<T>(param, filters[0], filters[1]));
                            else
                                try
                                {
                                    exp2 = Expression.Or(exp2, GetExpression<T>(param, filters[0], filters[1]));
                                }
                                catch (Exception)
                                {


                                }
                        }
                        filters.Remove(f1);
                        filters.Remove(f2);

                        if (filters.Count == 1)
                        {
                            if (OrOperation == false)
                                exp2 = Expression.AndAlso(exp2, GetExpression<T>(param, filters[0]));
                            else
                                exp2 = Expression.Or(exp2, GetExpression<T>(param, filters[0]));

                            filters.RemoveAt(0);
                        }
                    }
                }
            }

            if (exp1 != null && exp2 != null)
            {
                exp2 = Expression.AndAlso(exp1, exp2);
                return Expression.Lambda<Func<T, bool>>(exp2, param);
            }
            else if (exp1 == null)
                return Expression.Lambda<Func<T, bool>>(exp2, param);
            else
                return Expression.Lambda<Func<T, bool>>(exp1, param);


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

        private static ConstantExpression ConvertType(object value, Type type)
        {
            try
            {

                if (type.Name == "String")
                {
                    if (value == null)
                        return Expression.Constant(value, type);
                    else
                        return Expression.Constant(Convert.ToString(value), type);
                }

                var obj = Activator.CreateInstance(type);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (String.IsNullOrEmpty(Convert.ToString(value)))
                        obj = null;
                    else
                    {
                        if (Convert.ToString(type) == "System.Nullable`1[System.DateTime]")
                        {
                            DateTime? dt;
                            if (string.IsNullOrEmpty(Convert.ToString(value)))
                            {
                                dt = (DateTime?)null;
                            }
                            else
                            {
                                //*****System.Globalization.DateTimeStyles.RoundtripKind  **This is to handle ISO dates.
                                dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (DateTime?)null : DateTime.Parse(Convert.ToString(value), null, System.Globalization.DateTimeStyles.RoundtripKind);

                            }
                            return Expression.Constant(dt, type);
                        }
                        if (Convert.ToString(type) == "System.Nullable`1[System.Double]")
                        {
                            Double? dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (Double?)null : Double.Parse(Convert.ToString(value));

                            return Expression.Constant(dt, type);
                        }
                        else if (Convert.ToString(type) == "System.Nullable`1[System.Int32]")
                        {
                            Int32? dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (Int32?)null : Int32.Parse(Convert.ToString(value));

                            return Expression.Constant(dt, type);
                        }
                        else if (Convert.ToString(type) == "System.Nullable`1[System.Int64]")
                        {
                            Int64? dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (Int64?)null : Int64.Parse(Convert.ToString(value));

                            return Expression.Constant(dt, type);
                        }
                        else if (Convert.ToString(type) == "System.Nullable`1[System.Boolean]")
                        {
                            bool? dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (bool?)null : bool.Parse(Convert.ToString(value));

                            return Expression.Constant(dt, type);
                        }
                        if (Convert.ToString(type) == "System.Nullable`1[System.Decimal]")
                        {
                            Decimal? dt = string.IsNullOrEmpty(Convert.ToString(value)) ? (Decimal?)null : Decimal.Parse(Convert.ToString(value));

                            return Expression.Constant(dt, type);
                        }
                        else
                            obj = Convert.ChangeType(value, type.GetGenericArguments()[0]);
                    }
                }
                else
                {
                    obj = Convert.ChangeType(value, type);
                }
                return Expression.Constant(Convert.ChangeType(obj, type), type);
            }
            catch (Exception)
            {
                if (Convert.ToString(type) == "System.String")
                {
                    return Expression.Constant(Convert.ChangeType(Convert.ToString(value).ToLower(), type), type);
                }
                else
                    return Expression.Constant(Convert.ChangeType(value, type), type);
            }
        }

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            string[] str = new string[0];

            if (filter.value != null)
                str = Convert.ToString(filter.value).Split(',');

            if (str.Length > 1)
            {
                Expression exp1 = null;
                int i = 0;

                foreach (var s in str)
                {
                    if (i == 0)
                    {
                        exp1 = GetExpression<T>(param,
                            new Filter() { Operation = filter.Operation, coulumnname = filter.coulumnname, value = s });
                    }
                    else
                    {
                        var exp2 = GetExpression<T>(param,
                            new Filter() { Operation = filter.Operation, coulumnname = filter.coulumnname, value = s });
                        var tmp = Expression.Or(exp1, exp2);
                        exp1 = tmp;
                    }
                    i++;
                }
                return exp1;
            }

            MemberExpression member = Expression.Property(param, filter.coulumnname);
            ConstantExpression constant;

            if (filter.Operation == OpertaionType.complexDateTime)
            {
                constant = ConvertType(filter.value, typeof(string));
            }
            else
                constant = ConvertType(filter.value, member.Type);

            switch (filter.Operation)
            {
                case OpertaionType.equals:
                    {
                        if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                        {
                            var dynamicExpression = Expression.Call(member, miTL);

                            return Expression.Equal(member, constant);
                        }
                        else if (Convert.ToString(member.Type) == "System.Nullable`1[System.DateTime]")
                        {
                            Expression lowerBound = Expression.GreaterThanOrEqual(member, constant);
                            DateTime dt = DateTime.Parse(Convert.ToString(constant.Value));
                            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                            constant = Expression.Constant(dt, member.Type);
                            Expression upperBound = Expression.LessThan(member, constant);

                            Expression and = Expression.AndAlso(lowerBound, upperBound);
                            return and;
                        }
                        else
                            return Expression.Equal(member, constant);
                    }
                case OpertaionType.greaterThan:

                    return Expression.GreaterThan(member, constant);

                case OpertaionType.greaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case OpertaionType.lessThan:
                    return Expression.LessThan(member, constant);

                case OpertaionType.lessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case OpertaionType.contains:
                    if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                    {

                        var neq = Expression.NotEqual(member, Expression.Constant(null, member.Type));
                        var dynamicExpression = Expression.Call(member, miTL);


                        return Expression.Call(dynamicExpression, containsMethod, constant);
                    }
                    else
                        return Expression.Call(member, containsMethod, constant);

                case OpertaionType.startsWith:
                    if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                    {
                        var dynamicExpression = Expression.Call(member, miTL);
                        return Expression.Call(dynamicExpression, startsWithMethod, constant);
                    }
                    else
                        return Expression.Call(member, startsWithMethod, constant);

                case OpertaionType.endsWith:
                    if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                    {
                        var dynamicExpression = Expression.Call(member, miTL);
                        return Expression.Call(dynamicExpression, endsWithMethod, constant);
                    }
                    else
                        return Expression.Call(member, endsWithMethod, constant);
                case OpertaionType.notEquals:
                    if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                    {

                        var dynamicExpression = Expression.Call(member, miTL);


                        return Expression.NotEqual(dynamicExpression, constant);
                    }
                    else
                        return Expression.NotEqual(member, constant);
                case OpertaionType.notContains:

                    if (Convert.ToString(member.Type) == "System.String" && constant.Value != null)
                    {

                        var dynamicExpression = Expression.Call(member, miTL);


                        return Expression.Not(Expression.Call(dynamicExpression, containsMethod, constant));
                    }
                    else
                        return Expression.Not(Expression.Call(member, containsMethod, constant));
            }

            return null;
        }

        private static Expression CalculateComplexDateTimExpression(Filter filter)
        {

            return Expression.Empty();
        }
        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, Filter filter1, Filter filter2, bool OrOperation = false)
        {
            Expression bin1 = GetExpression<T>(param, filter1);

            string[] str = new string[0];

            if (filter2.value != null)
                str = Convert.ToString(filter2.value).Split('^');

            if (str.Length > 1)
            {
                Expression exp1 = null;
                int i = 0;
                foreach (var s in str)
                {
                    if (i == 0)
                    {
                        exp1 = GetExpression<T>(param,
                            new Filter() { Operation = filter2.Operation, coulumnname = filter2.coulumnname, value = s });
                    }
                    else
                    {
                        var exp2 = GetExpression<T>(param,
                            new Filter() { Operation = filter2.Operation, coulumnname = filter2.coulumnname, value = s });
                        var tmp = Expression.Or(exp1, exp2);
                        exp1 = tmp;

                    }
                    i++;

                }

                var x = Expression.AndAlso(bin1, exp1);
                return x;

            }


            Expression bin2 = GetExpression<T>(param, filter2);
            if (OrOperation == false)
                return Expression.AndAlso(bin1, bin2);
            else
                return Expression.Or(bin1, bin2);
        }
    }
}
