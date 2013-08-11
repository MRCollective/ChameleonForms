using ChameleonForms.Templates;
using container;

namespace ChameleonForms
{
    public class Config
    {
        internal static Container Container { get; set; }

        /// <summary>
        /// Setup the default:
        /// - FormTemplate
        /// </summary>
        public Config()
        {
            Container = new Container();
            Container.Register<IFormTemplate>(r => new DefaultFormTemplate());
        }

        /// <summary>
        /// The default Chameleon form template renderer
        /// </summary>
        public static IFormTemplate FormTemplate
        {
            get { return Container.Resolve<IFormTemplate>(); }
            set
            {
                Container.Remove<IFormTemplate>();
                Container.Register(r => value);
            }
        }
    }
}
