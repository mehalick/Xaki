using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Xaki.AspNetCore.ModelBinding
{
    public class LocalizableModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.IsComplexType)
            {
                return null;
            }

            var propName = context.Metadata.PropertyName;
            if (string.IsNullOrWhiteSpace(propName))
            {
                return null;
            }

            var propInfo = context.Metadata.ContainerType.GetProperty(propName);
            if (propInfo is null)
            {
                return null;
            }

            var hasLocalizedAttribute = Attribute.IsDefined(propInfo, typeof(LocalizedAttribute), false);
            if (!hasLocalizedAttribute)
            {
                return null;
            }

            return new BinderTypeModelBinder(typeof(LocalizableModelBinder));
        }
    }
}
