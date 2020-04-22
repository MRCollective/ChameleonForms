using System;
using System.ComponentModel.DataAnnotations;

namespace ChameleonForms.Tests.ModelBinders.UriModelBinderTests
{
    class TestViewModel
    {
        public Uri Uri { get; set; }

        [DataType(DataType.Url)]
        public Uri UriAsUrl { get; set; }
    }
}
