using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Xaki.Web.ModelBinding
{
    public class LocalizableModelBinder : IModelBinder
    {
        private readonly IObjectLocalizer _localizer;

        public LocalizableModelBinder(IObjectLocalizer localizer)
        {
            _localizer = localizer;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var localizedValues = GetLocalizedValuesFromForm(bindingContext);
            var serializedContent = _localizer.Serialize(localizedValues);
            var valueProviderResult = new ValueProviderResult(serializedContent);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue);

            return Task.CompletedTask;
        }

        private static Dictionary<string, string> GetLocalizedValuesFromForm(ModelBindingContext bindingContext)
        {
            var localizedValues = new Dictionary<string, string>();

            for (var i = 0; i < 1024; i++) // assume FormOptions.ValueCountLimit is default 1024
            {
                var hasKey = bindingContext.HttpContext.Request.Form.TryGetValue($"{bindingContext.ModelName}[{i}].Key", out var key);
                var hasValue = bindingContext.HttpContext.Request.Form.TryGetValue($"{bindingContext.ModelName}[{i}].Value",
                        out var value);

                if (!(hasKey && hasValue))
                {
                    break;
                }

                localizedValues.Add(key, value);
            }

            return localizedValues;
        }
    }
}
