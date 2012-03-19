using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Slognals
{
    /* --- EXPRESSIONCONVERTER - Used to convert a Where() Expression tree using bare parameters into the necessary Expression
     *                           tree that wraps the parameters in their Tuple so that users don't have to access their parameters
     *                           through Tuple properties.
     */

    internal class ExpressionConverter : ExpressionVisitor
    {
        private ParameterExpression _replacementParam;
        private Type _replacementLambdaType;
        private Dictionary<String, String> _parameterMap;

        /* --- MODIFY - The main method of this class, it converts a user-defined expression tree into something we can use in 
         *              a Where() extension method.
         *      TAKES - Expression that represents the user-defined expression tree condition.
         *              ParameterExpression that represents the new ParameterExpression that needs to replace all other ParExps in the tree.
         *              Type that represents the replacement lambda type for the Expression tree we are about to return.
         *              IEnumerable<ParameterExpression> that allows us to identify each parameter in the user-defined expression.
         *    RETURNS - Expression that is now the converted tree, usable in the Signal.Connect() methods.
         */

        public Expression Modify(Expression expression, ParameterExpression replacementParam, Type replacementType, IEnumerable<ParameterExpression> parameters)
        {
            this._replacementParam = replacementParam;
            this._replacementLambdaType = replacementType;

            var dict = new Dictionary<String, String>();

            int i = 1;                                // map the expression's parameters to appropriate Tuple properties.
            foreach (ParameterExpression param in parameters)
            {
                dict.Add(param.Name, "Item" + i.ToString());
                i++;
            }
            this._parameterMap = dict;

            return Visit(expression);
        }

        /* --- VISITS - The following methods are overrides of ExpressionVisitor methods that allow me to specialize the functionality
         *             of what is otherwise an Expression tree traverser.
         *             */

        protected override Expression VisitParameter(ParameterExpression p)
        {
                                  // replace all bare parameter expressions from the user-defined expression tree into appropriate
                                  //    member access expressions, effectively wrapping parameters as properties in their tuples.
            return Expression.MakeMemberAccess(_replacementParam, _replacementParam.Type.GetMember(_parameterMap[p.Name])[0]);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Expression body = this.Visit(node.Body);
            return Expression.Lambda(_replacementLambdaType, body, _replacementParam);
        }
    }
}
