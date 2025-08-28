using System;
using System.Linq;
using System.Linq.Expressions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propName, bool descending = false)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = descending ? "OrderByDescending" : "OrderBy";

        var result = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.Type },
            source.Expression,
            Expression.Quote(lambda));

        return source.Provider.CreateQuery<T>(result);
    }
}
