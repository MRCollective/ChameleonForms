using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using MyTested.AspNetCore.Mvc.Internal.Application;
using NUnit.Framework;
using static MyTested.AspNetCore.Mvc.Internal.Application.TestApplication;

namespace ChameleonForms.Tests
{
    public class Class
    {
        [Test]
        public async Task Test()
        {
            TestApplication.TryInitialize();
            var x = Services.GetRequiredService<DefaultTemplate>();
            var y = await x.MessageParagraph(new HtmlString("test"));

            var z = new AspNetCore.Views_MessageParagraph();
            await z.ExecuteAsync();

        }
    }
}
