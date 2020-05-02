using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ChameleonForms.Metadata
{
    internal interface IModelMetadataAware
    {
        void GetDisplayMetadata(DisplayMetadataProviderContext context);
    }
}
