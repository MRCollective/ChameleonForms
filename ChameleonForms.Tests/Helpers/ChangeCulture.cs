using System;
using System.Globalization;
using System.Threading;

namespace ChameleonForms.Tests.Helpers
{
    public class ChangeCulture : IDisposable
    {
        public static ChangeCulture To(string culture)
        {
            return new ChangeCulture(culture);
        }

        private readonly CultureInfo _existingCulture;

        private ChangeCulture(string culture)
        {
            _existingCulture = Thread.CurrentThread.CurrentCulture;
            SetCulture(CultureInfo.GetCultureInfo(culture));
        }

        private void SetCulture(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        public void Dispose()
        {
            SetCulture(_existingCulture);
        }
    }
}
