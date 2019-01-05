using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonForms.Metadata
{
    public class ModelMetadataAwareDisplayMetadataProvider : IDisplayMetadataProvider
    {
        private static List<Type> Registry = new List<Type>();

        internal static void RegisterMetadataAwareAttribute(Type Attribute)
        {
            if (Attribute == null)
            {
                throw new ArgumentNullException();
            }

            if (!typeof(IModelMetadataAware).IsAssignableFrom(Attribute))
            {
                throw new ArgumentException("This attribute type is not metadata aware.");
            }

            Registry.Add(Attribute);
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Type Type in Registry)
            {
                IModelMetadataAware Attribute = (IModelMetadataAware)context.Attributes
                    .Where(x => x.GetType() == Type)
                    .FirstOrDefault();

                if (Attribute != null)
                {
                    Attribute.GetDisplayMetadata(context);
                }
            }
        }
    }
}
