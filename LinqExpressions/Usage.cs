using System;
using System.Collections.Generic;

namespace LinqExpressions
{
    public class Usage
    {
        protected ILinqExpressions _linqExpressions;

        public Usage(ILinqExpressions le)
        {
            _linqExpressions = le;
        }

        public void Easiest()
        {
            var predicate = _linqExpressions.BuildExpression<Entity>("IntProperty", 6);
            var predicate2 = _linqExpressions.BuildExpression<Entity>("StringProperty", "Sight");
            var predicate3 = _linqExpressions.BuildExpression<Entity>("DateTimeProperty", DateTime.Today);
        }

        public void EasiestPlusComparisonOperator()
        {
            var predicate = _linqExpressions.BuildExpression<Entity>("IntProperty", 4, Operators.Comparison.GREATER);
        }

        public void MultipleParametersAndComparisonOperators()
        {
            var predicate = _linqExpressions.BuildExpression<Entity>(4,
                        new Queue<string>(new string[] { "IntProperty", "StringProperty", "DateTimeProperty", "BoolProperty" }),
                        new Queue<dynamic>(new dynamic[] { 5, "Tangent", DateTime.Now, true }),
                        new Queue<string>(new string[] { Operators.Comparison.EQUAL, Operators.Comparison.NOT_EQUAL, Operators.Comparison.EQUAL, Operators.Comparison.NOT_EQUAL }));

            // Dynamically adjust
            var aCondition = true;
            var predicate2 = _linqExpressions.BuildExpression<Entity>(2,
               new Queue<string>(new string[] { "IntProperty", "StringProperty" }),
               new Queue<dynamic>(new dynamic[] { 9, "Shade" }),
               new Queue<string>(new string[] { Operators.Comparison.EQUAL,
                    aCondition ? Operators.Comparison.EQUAL : Operators.Comparison.NOT_EQUAL }));
        }

        public void MultipleParametersAndComparisonAndConditionOperators()
        {
            var predicate = _linqExpressions.BuildExpression<Entity>(4,
                        new Queue<string>(new string[] { "IntProperty", "StringProperty", "DateTimeProperty", "BoolProperty" }),
                        new Queue<dynamic>(new dynamic[] { 7, "Breeze", DateTime.Now, true }),
                        new Queue<string>(new string[] { Operators.Comparison.EQUAL, Operators.Comparison.NOT_EQUAL, Operators.Comparison.EQUAL, Operators.Comparison.NOT_EQUAL }),
                        new Queue<string>(new string[] { Operators.LogicOrCondition.AND, Operators.LogicOrCondition.OR, Operators.LogicOrCondition.AND, Operators.LogicOrCondition.OR }));
        }

        public void AllDynamic<T>() where T : class
        {
            // Caution to be exercised.
            var param = "AProperty";
            var value = 34.433m;

            var predicate = _linqExpressions.BuildExpression<T>(param, value);
        }
    }
}
