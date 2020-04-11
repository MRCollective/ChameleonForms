using System.Threading.Tasks;
using ChameleonForms.Generated;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    public class Class
    {
        [Test]
        public void Test()
        {
           var x = new ChameleonFormsTemplate();
           var y = x.DefaultFormTemplate.MessageParagraph(new HtmlString("asdf"));
        }
    }
}
