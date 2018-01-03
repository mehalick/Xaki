namespace Xaki
{
    public class LocalizedContent
    {
        /// <summary>
        /// The key property for a localized field, represents the field's language code.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value property for a localized field, represent the field's text value.
        /// </summary>
        public string Value { get; set; }

        public LocalizedContent()
        {
        }

        public LocalizedContent(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
