using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators.Handlers;

namespace ChameleonForms.FieldGenerators
{
    internal static class FieldGeneratorHandlersRouter<TModel, T>
    {
        private static IEnumerable<FieldGeneratorHandler<TModel, T>> GetHandlers(IFieldGenerator<TModel, T> g, IReadonlyFieldConfiguration c)
        {
            yield return new EnumListHandler<TModel, T>(g, c);
            yield return new PasswordHandler<TModel, T>(g, c);
            yield return new TextAreaHandler<TModel, T>(g, c);
            yield return new BooleanHandler<TModel, T>(g, c);
            yield return new FileHandler<TModel, T>(g, c);
            yield return new ListHandler<TModel, T>(g, c);
            yield return new DateTimeHandler<TModel, T>(g, c);
            yield return new DefaultHandler<TModel, T>(g, c);
        }

        public static IHtmlString GetFieldHtml(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            return GetHandlers(fieldGenerator, fieldConfiguration)
                .First(h => h.CanHandle())
                .GenerateFieldHtml();
        }

        public static void PrepareFieldConfiguration(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
        {
            GetHandlers(fieldGenerator, new ReadonlyFieldConfiguration(fieldConfiguration))
                .First(h => h.CanHandle())
                .PrepareFieldConfiguration(fieldConfiguration);
        }
    }
}