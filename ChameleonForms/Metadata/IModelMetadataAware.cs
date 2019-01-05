using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChameleonForms.Metadata
{
    internal interface IModelMetadataAware
    {
        void GetDisplayMetadata(DisplayMetadataProviderContext context);
    }
}
