using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public class GroupResult<T>
    {
        public string Label { get; set; }
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<GroupResult<T>> SubGroups { get; set; }
        public override string ToString() { return string.Format("{0} ({1})", Key, Count); }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,string[] groupSelectorLabels,
             Func<TElement, object>[] groupSelectors)
        {
            Func<IEnumerable<TElement>, IEnumerable<GroupResult<TElement>>> groupBy = source => null;
            for (int i = groupSelectors.Length - 1; i >= 0; i--)
            {
                var keySelector = groupSelectors[i]; // Capture
                var subGroupsSelector = groupBy; // Capture
                var label = groupSelectorLabels[i];
                groupBy = source => source.GroupBy(keySelector).Select(g => new GroupResult<TElement>
                {
                    Key = g.Key,
                    Count = g.Count(),
                    Label= label,
                    Items = g,
                    SubGroups = subGroupsSelector(g)
                });
            }
            return groupBy(elements);
        }

        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
           this IEnumerable<TElement> elements,
           params string[] groupSelectorStrings)
        {
            var groupSelectors = new List<Func<TElement, object>>();
            foreach (var groupByProperty in groupSelectorStrings)
            {
                var arg = Expression.Parameter(typeof(TElement), "item");
                var body = Expression.Convert(Expression.Property(arg, groupByProperty), typeof(object));
                var lambda = Expression.Lambda<Func<TElement, object>>(body, arg);
                var keySelector = lambda.Compile();
                groupSelectors.Add(keySelector);
            }

           return elements.GroupByMany<TElement>(groupSelectorStrings,groupSelectors.ToArray());
        }
    }
}
