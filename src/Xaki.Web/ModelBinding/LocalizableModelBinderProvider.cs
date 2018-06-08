using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Xaki.Web.ModelBinding
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
            if (propName == null)
            {
                return null;
            }

            var propInfo = context.Metadata.ContainerType.GetProperty(propName);
            if (propInfo == null)
            {
                return null;
            }

            var attribute = propInfo.GetCustomAttributes(typeof(LocalizedAttribute), false).FirstOrDefault();
            if (attribute == null)
            {
                return null;
            }

            return new BinderTypeModelBinder(typeof(LocalizableModelBinder));
        }
    }
}
