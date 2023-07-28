using System.Linq.Expressions;

namespace JailTalk.Shared.Extensions;

public static class LingExtension
{
    /// <summary>
    /// Creates an db where clause expression such as `where prison_value is 0 or x.prison_field = prison_value`
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="prisonIdFieldExpression"></param>
    /// <param name="prisonId"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereInPrison<T>(this IQueryable<T> query, Expression<Func<T, int?>> prisonIdFieldExpression, int prisonId)
    {
        // If prison id is zero then no need to add any filter.
        if (prisonId == 0)
        {
            return query;
        }
        var parameter = prisonIdFieldExpression.Parameters[0];
        var nullCheck = Expression.Equal(Expression.Constant(prisonId), Expression.Constant(0));
        var equalityCheck = Expression.Equal(prisonIdFieldExpression.Body, Expression.Constant(prisonId, typeof(int?)));
        var combinedCheck = Expression.OrElse(nullCheck, equalityCheck);
        var lambda = Expression.Lambda<Func<T, bool>>(combinedCheck, parameter);
        return query.Where(lambda);
    }
}
