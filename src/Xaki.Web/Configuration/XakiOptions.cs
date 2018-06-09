using System.Collections.Generic;
using System.Linq;

namespace Xaki.Web.Configuration
{
    public class XakiOptions
    {
        public IEnumerable<string> RequiredLanguages { get; set; }
        public IEnumerable<string> OptionalLanguages { get; set; }
        public IEnumerable<string> SupportedLanguages => RequiredLanguages.Union(OptionalLanguages);
    }
}
