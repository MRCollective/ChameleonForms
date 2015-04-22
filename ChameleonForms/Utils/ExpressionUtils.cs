using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChameleonForms.Utils
{
    static class ExpressionExtensions
    {
        public static Expression<Func<T, TProperty>> Combine<T, TNav, TProperty>(Expression<Func<T, TNav>> parent, Expression<Func<TNav, TProperty>> nav)
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
        private readonly Expression oldExpr;
        private readonly Expression newExpr;
        public ReplacementVisitor(Expression oldExpr, Expression newExpr)
        {
            this.oldExpr = oldExpr;
            this.newExpr = newExpr;
        }

        public override Expression Visit(Expression node)
        {
            return node == oldExpr ? newExpr : base.Visit(node);
        }
    }
}
