using System.Web;

namespace ChameleonForms.FieldGenerators
{
    internal class HandleAction
    {
        public HandleAction()
        {
            HasReturnValue = false;
        }

        public HandleAction(IHtmlString html)
        {
            ReturnValue = html;
            HasReturnValue = true;
        }

        public IHtmlString ReturnValue { get; private set; }
        public bool HasReturnValue { get; private set; }

        public static readonly HandleAction Continue = new HandleAction();
        public static HandleAction Return(IHtmlString html)
        {
            return new HandleAction(html);
        }
    }
}