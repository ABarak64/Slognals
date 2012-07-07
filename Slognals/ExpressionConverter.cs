using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Slognals
{
    /// <summary>
    /// Used to convert a Where() Expression tree using bare parameters into the necessary Expression tree that wraps the parameters 
    /// in their Tuple so that users don't have to access their parameters through Tuple properties.
    /// </summary>
    internal class ExpressionConverter : ExpressionVisitor
    {
        private ParameterExpression _replacementParam;
        private Type _replacementLambdaType;
        private Dictionary<String, String> _parameterMap;

        /// <summary>
        /// The main method of this class, it converts a user-defined expression tree into something we can use in a Where() extension method.
        /// </summary>
        /// <param name="expression">Represents the user-defined expression tree condition.</param>
        /// <param name="replacementParam">Represents the new ParameterExpression that needs to replace all other ParExps in the tree.</param>
        /// <param name="replacementType">Represents the replacement lambda type for the Expression tree we are about to return.</param>
        /// <param name="parameters">Collection that allows us to identify each parameter in the user-defined expression.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Turns all of the parameter expressions in the expression tree into member accesses to facilitate the conversion between the raw 
        /// expression and the expression using the Tuple wrapper.
        /// </summary>
        /// <param name="p">The parameter expression node being recursively visited.</param>
        /// <returns>The newly made expression to replace the node on the expression tree.</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
                                  // replace all bare parameter expressions from the user-defined expression tree into appropriate
                                  //    member access expressions, effectively wrapping parameters as properties in their tuples.
            return Expression.MakeMemberAccess(_replacementParam, _replacementParam.Type.GetMember(_parameterMap[p.Name])[0]);
        }

        /// <summary>
        /// Replaces the main body of the expression with the body of the newly formed expression.
        /// </summary>
        /// <typeparam name="T">Whatever type of expression.</typeparam>
        /// <param name="node">Initial node that acts as a container to the expression tree.</param>
        /// <returns>The new expression tree after modification.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Expression body = this.Visit(node.Body);
            return Expression.Lambda(_replacementLambdaType, body, _replacementParam);
        }
    }
}
