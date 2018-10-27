using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Xaki.AspNetCore.TagHelpers
{
    [HtmlTargetElement("input", Attributes = "localized-for", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("textarea", Attributes = "localized-for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LocalizedEditor : TagHelper
    {
        [HtmlAttributeName("localized-for")]
        public ModelExpression Model { get; set; }

        private readonly IObjectLocalizer _localizer;

        public LocalizedEditor(IObjectLocalizer localizer)
        {
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagName = output.TagName;

            var propertyId = $"localized-editor-{Guid.NewGuid():n}";
            var propertyName = Model.Metadata.PropertyName;
            var friendlyName = Regex.Replace(Regex.Replace(propertyName, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2", RegexOptions.Compiled), @"(\p{Ll})(\P{Ll})", "$1 $2", RegexOptions.Compiled);

            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", propertyId);
            output.AddClass("localized-editor", HtmlEncoder.Default);
            output.AddClass("list-unstyled", HtmlEncoder.Default);

            if (IsHtml(Model.Metadata))
            {
                output.AddClass("localized-editor-html", HtmlEncoder.Default);
            }

            var items = _localizer.Deserialize(Model.Model.ToString());

            for (var i = 0; i < _localizer.SupportedLanguages.Count; i++)
            {
                var languageCode = _localizer.SupportedLanguages.ElementAt(i);

                if (!items.TryGetValue(languageCode, out var languageValue))
                {
                    languageValue = string.Empty;
                }

                var inputId = $"{Model.Name}_{i}_Value";
                var inputKey = $"{Model.Name}[{i}].Key";
                var inputName = $"{Model.Name}[{i}].Value";
                var cultureInfo = new CultureInfo(languageCode);
                var languageDirection = cultureInfo.TextInfo.IsRightToLeft ? "rtl" : "ltr";
                var isRequired = Model.Metadata.IsRequired && _localizer.RequiredLanguages.Contains(languageCode);

                var label = new TagBuilder("label");
                label.AddCssClass("sr-only");
                label.Attributes.Add("for", inputId);
                label.InnerHtml.Append($"{friendlyName} in {cultureInfo.NativeName}");

                if (isRequired)
                {
                    label.InnerHtml.Append(" (required)");
                }

                output.PreContent.AppendHtml(label);

                var input = new TagBuilder(tagName);
                input.AddCssClass("form-control localized-input");
                input.Attributes.Add("type", "text");
                input.Attributes.Add("id", inputId);
                input.Attributes.Add("name", inputName);

                input.Attributes.Add("lang", languageCode);
                input.Attributes.Add("dir", languageDirection);

                if (tagName == "input")
                {
                    input.Attributes.Add("value", languageValue);
                }
                else
                {
                    input.InnerHtml.AppendHtml(languageValue);
                }

                if (isRequired)
                {
                    input.Attributes.Add("required", "required");
                    input.Attributes.Add("data-val", "true");
                    input.Attributes.Add("data-val-required", $"The {friendlyName.ToLowerInvariant()} field in {cultureInfo.NativeName} is required.");
                }

                var div = new TagBuilder("div");
                div.AddCssClass("input-group");

                if (!cultureInfo.TextInfo.IsRightToLeft)
                {
                    div.InnerHtml.AppendHtml($@"
                        <div class='input-group-prepend'>
                            <span class='input-group-text justify-content-center' lang='{languageCode}'>
                                <small>{cultureInfo.NativeName}</small>
                            </span>
                        </div>");
                }

                div.InnerHtml.AppendHtml(input);

                if (cultureInfo.TextInfo.IsRightToLeft)
                {
                    div.InnerHtml.AppendHtml($@"
                        <div class='input-group-append'>
                            <span class='input-group-text justify-content-center' lang='{languageCode}'>
                                <small>{cultureInfo.NativeName}</small>
                            </span>
                        </div>");
                }

                var hidden = new TagBuilder("input");
                hidden.Attributes.Add("type", "hidden");
                hidden.Attributes.Add("name", inputKey);
                hidden.Attributes.Add("value", languageCode);

                var li = new TagBuilder("li");
                li.AddCssClass("mb-1");
                li.InnerHtml.AppendHtml(hidden);
                li.InnerHtml.AppendHtml(div);

                if (isRequired)
                {
                    var span = new TagBuilder("span");
                    span.AddCssClass("field-validation-valid");
                    span.Attributes.Add("data-valmsg-for", inputName);
                    span.Attributes.Add("data-valmsg-replace", "true");
                    li.InnerHtml.AppendHtml(span);
                }

                output.PreContent.AppendHtml(li);

                output.PostContent.AppendHtml($@"
                    <script>

                        (function () {{
                            var getMaxWidth = function(elements) {{
                                let width = 0;
                                for (let i = 0; i < elements.length; i++) {{
                                    width = Math.max(width, elements[i].offsetWidth);
                                }}
                                return width;
                            }}

                            var setMinWidth = function (elements, width) {{
                                for (let i = 0; i < elements.length; i++) {{
                                    elements[i].style.minWidth = `${{width}}px`;
                                }}
                            }}

                            let parent = document.getElementById('{propertyId}');
                            let labels = parent.getElementsByClassName('input-group-text');
                            let maxWidth = getMaxWidth(labels);
                            setMinWidth(labels, maxWidth);
                        }})();

                    </script>");
            }
        }

        private bool IsHtml(ModelMetadata modelMetadata)
        {
            return ((DataTypeAttribute)modelMetadata.ContainerType.GetProperty(Model.Name)?.GetCustomAttribute(typeof(DataTypeAttribute)))?.DataType == DataType.Html;
        }
    }
}
