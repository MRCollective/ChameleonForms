using System;
using System.Linq.Expressions;

namespace ChameleonForms.Utils
{
    // http://stackoverflow.com/questions/10898800/combine-two-linq-expressions-to-inject-navigation-property
    static class ExpressionExtensions
    {
        public static Expression<Func<T, TProperty>> Combine<T, TNav, TProperty>(this Expression<Func<T, TNav>> parent, Expression<Func<TNav, TProperty>> nav)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var visitor = new ReplacementVisitor(parent.Parameters[0], param);
            var newParentBody = visitor.Visit(parent.Body);
            visitor = new ReplacementVisitor(nav.Parameters[0], newParentBody);
            var body = visitor.Visit(nav.Body);
            return Expression.Lambda<Func<T, TProperty>>(body, param);
        }
    }

    class ReplacementVisitor : ExpressionVisitor
    {
        private readonly Expression _oldExpr;
        private readonly Expression _newExpr;

        public ReplacementVisitor(Expression oldExpr, Expression newExpr)
        {
            _oldExpr = oldExpr;
            _newExpr = newExpr;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldExpr ? _newExpr : base.Visit(node);
        }
    }
}
