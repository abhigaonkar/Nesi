using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public static class IQuerableExtension
    {
        public static object Sum(this IQueryable source, string member)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (member == null) throw new ArgumentNullException("member");

            // Properties
            PropertyInfo property = source.ElementType.GetProperty(member);
            ParameterExpression parameter = Expression.Parameter(source.ElementType, "s");
            Expression selector = Expression.Lambda(Expression.MakeMemberAccess(parameter, property), parameter);
            // We've tried to find an expression of the type Expression<Func<TSource, TAcc>>,
            // which is expressed as ( (TSource s) => s.Price );

            // Method
            MethodInfo sumMethod = typeof(Queryable).GetMethods().First(
                m => m.Name == "Sum"
                    && m.ReturnType == property.PropertyType // should match the type of the property
                    && m.IsGenericMethod);

            return source.Provider.Execute(
                Expression.Call(
                    null,
                    sumMethod.MakeGenericMethod(new[] { source.ElementType }),
                    new[] { source.Expression, Expression.Quote(selector) }));
        }

        public static object Average(this IQueryable source, string member)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (member == null) throw new ArgumentNullException("member");

            // Properties
            PropertyInfo property = source.ElementType.GetProperty(member);
            ParameterExpression parameter = Expression.Parameter(source.ElementType, "s");
            Expression selector = Expression.Lambda(Expression.MakeMemberAccess(parameter, property), parameter);
            // We've tried to find an expression of the type Expression<Func<TSource, TAcc>>,
            // which is expressed as ( (TSource s) => s.Price );

            // Method
            MethodInfo averageMethod = typeof(Queryable).GetMethods().First(
                m => m.Name == "Average"
                    && m.ReturnType == property.PropertyType // should match the type of the property
                    && m.IsGenericMethod);

            return source.Provider.Execute(
                Expression.Call(
                    null,
                    averageMethod.MakeGenericMethod(new[] { source.ElementType }),
                    new[] { source.Expression, Expression.Quote(selector) }));
        }



        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        public static IQueryable<T> ApplySortingPaging<T>(this IQueryable<T> query, SortOrder[] sortOrders, int pageNumber, int numberRecords)
        {
          var orderedQuery =  query.ApplySortingPaging(sortOrders);
          
            IQueryable<T> result = orderedQuery.ApplyPaging(pageNumber, numberRecords);
            return result;






        }

        public static IOrderedQueryable<T> ApplySortingPaging<T>(this IQueryable<T> query, SortOrder[] sortOrders)
        {
            var firstPass = true;
            IOrderedQueryable<T> orderedQuery = null;


            foreach (var sortOrder in sortOrders)
            {
                if (firstPass)
                {
                    firstPass = false;
                    orderedQuery =
                    sortOrder.ColumnOrder == SortOrder.Order.Ascending
                          ? query.OrderBy(sortOrder.ColumnName) :
                           query.OrderByDescending(sortOrder.ColumnName);
                }
                else
                {
                    orderedQuery =
                        sortOrder.ColumnOrder == SortOrder.Order.Ascending
                   ? orderedQuery.ThenBy(sortOrder.ColumnName) :
                   orderedQuery.ThenByDescending(sortOrder.ColumnName);
                }
            }
          
            return orderedQuery;






        }

        private static IQueryable<T> ApplyPaging<T>(this IOrderedQueryable<T> orderedQuery, int pageNumber, int numberRecords)
        {
            return orderedQuery.Skip((pageNumber - 1) *
        numberRecords).Take(numberRecords);
        }

        private static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int numberRecords)
        {
            return query.Skip((pageNumber - 1) *
        numberRecords).Take(numberRecords);
        }

        private static IQueryable ApplyPaging(this IQueryable query, int pageNumber, int numberRecords)
        {
            return query.Skip((pageNumber - 1) *
        numberRecords).Take(numberRecords);
        }



        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }

}
