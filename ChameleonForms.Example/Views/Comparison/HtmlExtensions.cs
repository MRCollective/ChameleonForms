﻿using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ChameleonForms.Example.Views.Comparison
{
    public static class HtmlExtensions
    {
        public static IDisposable BeginSection<TViewModel>(this HtmlHelper<TViewModel> helper, string heading)
        {
            return new Section(helper.ViewContext.Writer, heading);
        }
    }

    public class Section : IDisposable
    {
        private readonly TextWriter _writer;

        public Section(TextWriter writer, string heading)
        {
            _writer = writer;
            _writer.WriteLine("<fieldset><legend>{0}</legend><dl>", HttpUtility.HtmlEncode(heading));
        }

        public void Dispose()
        {
            _writer.WriteLine("</dl></fieldset>");
        }
    }
}