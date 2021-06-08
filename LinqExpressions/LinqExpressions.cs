using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqExpressions
{
    public class LinqExpressions : ILinqExpressions
    {
        public Expression<Func<T, bool>> BuildExpression<T>(string paramName, dynamic paramVal) where T : class
        {
            return BuildExpression<T>(paramName, paramVal, Operators.Comparison.EQUAL);
        }

        public Expression<Func<T, bool>> BuildExpression<T>(string paramName, dynamic paramVal, string op) where T : class
        {
            return BuildExpression<T>(1, new Queue<string>(new string[] { paramName }), new Queue<dynamic>(new dynamic[] { paramVal }), new Queue<string>(new string[] { op }));
        }

        public Expression<Func<T, bool>> BuildExpression<T>(int paramCount, Queue<string> paramNames, Queue<dynamic> paramVals, Queue<string> ops) where T : class
        {
            var logicOps = new Queue<string>();
            for (int i = 1; i < paramCount; i++)
                logicOps.Enqueue(Operators.LogicOrCondition.AND_ALSO);

            return BuildExpression<T>(paramCount, paramNames, paramVals, ops, logicOps);
        }

        public Expression<Func<T, bool>> BuildExpression<T>(int paramCount, Queue<string> paramNames, Queue<dynamic> paramVals, Queue<string> ops, Queue<string> logicOps) where T : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            PropertyInfo prop = typeof(T).GetProperty(paramNames.Dequeue());
            MemberExpression propExp = Expression.Property(parameter, prop);
            Expression valExp = Expression.Constant(paramVals.Dequeue());
            valExp = Expression.Convert(valExp, prop.PropertyType);
            var where = ComposeComparisonExpression(ops.Dequeue(), propExp, valExp);

            for (int i = 1; i < paramCount; i++)
            {
                PropertyInfo nextProp = typeof(T).GetProperty(paramNames.Dequeue());
                Expression nextPropExp = Expression.Property(parameter, nextProp);
                Expression nextPropValExp = Expression.Constant(paramVals.Dequeue());
                nextPropValExp = Expression.Convert(nextPropValExp, nextProp.PropertyType);
                var nextPropComparison = ComposeComparisonExpression(ops.Dequeue(), nextPropExp, nextPropValExp);
                where = AppendWhereExpressions(logicOps.Dequeue(), where, nextPropComparison);
            }

            return Expression.Lambda<Func<T, bool>>(where, parameter);
        }

        private Expression ComposeComparisonExpression(string op, Expression property, Expression value)
        {
            return op switch
            {
                Operators.Comparison.GREATER => Expression.GreaterThan(property, value),
                Operators.Comparison.GREATER_EQ => Expression.GreaterThanOrEqual(property, value),
                Operators.Comparison.LESS => Expression.LessThan(property, value),
                Operators.Comparison.LESS_EQ => Expression.LessThanOrEqual(property, value),
                Operators.Comparison.EQUAL => Expression.Equal(property, value),
                Operators.Comparison.NOT_EQUAL => Expression.NotEqual(property, value),
                Operators.Comparison.LIKE => Expression.Call(property, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value),
                _ => throw new ArgumentException($"Unrecognized operator ({op}) provided."),
            };
        }

        private Expression AppendWhereExpressions(string andOrWhr, Expression body, Expression criteria)
        {
            return andOrWhr switch
            {
                Operators.LogicOrCondition.AND => Expression.And(body, criteria),
                Operators.LogicOrCondition.OR => Expression.Or(body, criteria),
                Operators.LogicOrCondition.AND_ALSO => Expression.AndAlso(body, criteria),
                _ => throw new ArgumentException($"Unrecognized operator ({andOrWhr}) provided."),
            };
        }
    }
}
