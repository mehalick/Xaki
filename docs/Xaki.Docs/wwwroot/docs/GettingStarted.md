Xaki is a .NET library for adding multi-language support to POCO classes. It includes a lightweight service for persisting and retrieving data to and from databases using any ORM.

Xaki works well with all versions of Entity Framework and includes ASP.NET Core support for automatic localization to language codes provided by routes, querystrings, cookies, and HTTP headers.

<pre class="line-numbers">
    <code class="language-csharp">
        public class Planet : ILocalizable
        {
            public int PlanetId { get; set; }

            [Localized, Required]
            public string Name { get; set; }

            [Localized, DataType(DataType.Html)]
            public string Description { get; set; }
        }
    </code>
</pre>

## Setup

Xaki can be added to any .NET Core, .NET Standard, or .NET Full Framework project by installing the lightweight [Xaki](https://www.nuget.org/packages/Xaki/) package from Nuget:

<pre>
    <code class="language-powershell">Install-Package Xaki</code>
</pre>

Or:

<pre>
    <code class="language-powershell">dotnet add package Xaki</code>
</pre>

If using ASP.NET Core you can install alternatively install the [Xaki.AspNetCore](https://www.nuget.org/packages/Xaki.AspNetCore/) package directly.

You can then add unobtrusive multi-language support to any POCO or ORM class by implementing `ILocalizable` and decorating attributes with `LocalizedAttribute` as shown in the example above.



