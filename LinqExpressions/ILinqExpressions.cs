using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqExpressions
{
    public interface ILinqExpressions
    {
        /// <summary>
        /// Builds a predicate based on parameter name and value using default equality comparison.
        /// </summary>
        /// <typeparam name="T">Type of entity for which expression has to be formed.</typeparam>
        /// <param name="paramName">String representation of an entity's property.</param>
        /// <param name="paramVal">Dynamic-typed value of paramName.</param>
        /// <returns>Expression<Func<T, bool>> to be consumed as predicate.</returns>
        Expression<Func<T, bool>> BuildExpression<T>(string paramName, dynamic paramVal) where T : class;

        /// <summary>
        /// Builds a predicate based on parameter name, value and comparison operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T">Type of entity for which expression has to be formed.</typeparam>
        /// <param name="paramVal">Dynamic-typed value of paramName.</param>
        /// <param name="op">String representation of comparison operation - regulated by Comparison constants in GenericConstants class.</param>
        /// <returns>Expression<Func<T, bool>> to be consumed as predicate.</returns>
        Expression<Func<T, bool>> BuildExpression<T>(string paramName, dynamic paramVal, string op) where T : class;

        /// <summary>
        /// Builds a predicate based on queues (FIFO) of parameter names, values and comparison operations.
        /// </summary>
        /// <typeparam name="T">Type of entity for which expression has to be formed.</typeparam>
        /// <param name="paramCount">Count of all parameters to be in the expression.</param>
        /// <param name="paramNames">String queue of entity's properties.</param>
        /// <param name="paramVals">Dynamic queue of values of paramNames in the exact order.</param>
        /// <param name="ops">String queue of comparison operations in the exact order - regulated by Comparison constants in GenericConstants class.</param>
        /// <returns>Attention must be given to the order of queues' members - overall and corresponding to each other.</returns>
        Expression<Func<T, bool>> BuildExpression<T>(int paramCount, Queue<string> paramNames, Queue<dynamic> paramVals, Queue<string> ops) where T : class;

        /// <summary>
        /// Builds a predicate based on queues (FIFO) of parameter names, values, comparison operations and logic/conditional operations.
        /// </summary>
        /// <typeparam name="T">Type of entity for which expression has to be formed.</typeparam>
        /// <param name="paramCount">Count of all parameters to be in the expression.</param>
        /// <param name="paramNames">String queue of entity's properties.</param>
        /// <param name="paramVals">Dynamic queue of values of paramNames in the exact order.</param>
        /// <param name="ops">String queue of comparison operations in the exact order - regulated by Comparison constants in GenericConstants class.</param>
        /// <param name="logicOps">String queue of logic/condition operations in the exact order - regulated by LogicOrCondition constants in GenericConstants class.</param>
        /// <returns>Attention must be given to the order of queues' members - overall and corresponding to each other.</returns>
        Expression<Func<T, bool>> BuildExpression<T>(int paramCount, Queue<string> paramNames, Queue<dynamic> paramVals, Queue<string> ops, Queue<string> logicOps) where T : class;
    }
}
