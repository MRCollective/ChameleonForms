using System.Collections.Generic;
using System.Linq;
using ChameleonForms.FieldGenerators.Handlers;

namespace ChameleonForms.FieldGenerators
{
    internal static class FieldGeneratorHandlersRouter<TModel, T>
    {
        private static IEnumerable<FieldGeneratorHandler<TModel, T>> GetHandlers(IFieldGenerator<TModel, T> g)
        {
            yield return new PasswordHandler<TModel, T>(g);
            yield return new TextAreaHandler<TModel, T>(g);
            yield return new BooleanHandler<TModel, T>(g);
            //yield return new FileHandler<TModel, T>(g);
            yield return new ListHandler<TModel, T>(g);
            yield return new EnumListHandler<TModel, T>(g);
            yield return new DateTimeHandler<TModel, T>(g);
            yield return new DefaultHandler<TModel, T>(g);
        }

        public static IFieldGeneratorHandler<TModel, T> GetHandler(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return GetHandlers(fieldGenerator)
                .First(h => h.CanHandle());
        }
    }
}