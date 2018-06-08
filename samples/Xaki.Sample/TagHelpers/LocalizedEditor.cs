using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Xaki.Sample.TagHelpers
{
    [HtmlTargetElement("localized-editor")]
    public class LocalizedEditor : TagHelper
    {
        public ModelExpression For { get; set; }

        private readonly IObjectLocalizer _localizer;

        public LocalizedEditor(IObjectLocalizer localizer)
        {
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "localized-editor list-unstyled");

            var items = _localizer.Deserialize(For.Model.ToString());

            for (var i = 0; i < items.Count; i++)
            {
                var item = items.ElementAt(i);
                var inputId = $"{For.Name}_{i}_Value";
                var inputKey = $"{For.Name}[{i}].Key";
                var inputName = $"{For.Name}[{i}].Value";
                var cultureInfo = new CultureInfo(item.Key);
                var languageDirection = cultureInfo.TextInfo.IsRightToLeft ? "rtl" : "ltr";

                var input = new TagBuilder("input");
                input.AddCssClass("form-control localized-input");
                input.Attributes.Add("type", "text");
                input.Attributes.Add("id", inputId);
                input.Attributes.Add("name", inputName);
                input.Attributes.Add("value", item.Value);
                input.Attributes.Add("lang", item.Key);
                input.Attributes.Add("dir", languageDirection);

                var div = new TagBuilder("div");
                div.AddCssClass("form-input");
                div.InnerHtml.AppendHtml(input);

                var hidden = new TagBuilder("input");
                hidden.Attributes.Add("type", "hidden");
                hidden.Attributes.Add("name", inputKey);
                hidden.Attributes.Add("value", item.Key);

                var li = new TagBuilder("li");
                li.InnerHtml.AppendHtml(hidden);
                li.InnerHtml.AppendHtml(div);

                output.PreContent.AppendHtml(li);
            }
        }
    }
}
