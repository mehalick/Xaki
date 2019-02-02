<h1><img src="https://xaki.azureedge.net/assets/logo-text-636723199289149229.svg" width="512" height="190" alt="Xaki"></h1>

[![appveyor](https://img.shields.io/appveyor/ci/mehalick/xaki/master.svg?label=windows)](https://ci.appveyor.com/project/mehalick/xaki)
[![azure devops](https://img.shields.io/azure-devops/build/mehalick/ac9e6a14-b763-4434-8b15-a9c0defdf0da/1.svg?label=windows/linux/mac)](https://travis-ci.org/mehalick/Xaki)
[![tests](https://img.shields.io/appveyor/tests/mehalick/xaki/master.svg)](https://ci.appveyor.com/project/mehalick/xaki/build/tests)
[![nuget](https://img.shields.io/nuget/v/Xaki.svg)](https://www.nuget.org/packages/Xaki)
[![myget](https://img.shields.io/myget/xaki/v/Xaki.svg?label=myget)](https://www.myget.org/feed/xaki/package/nuget/Xaki)

Xaki is a .NET library for adding multi-language support to POCO classes. It includes a lightweight service for persisting and retrieving data to and from databases using any ORM.

Xaki works well with all versions of Entity Framework and includes ASP.NET Core support for automatic localization to language codes provided by routes, querystrings, cookies, and HTTP headers. 

## Introduction

Setting up classes to be multi-language starts by implementing `ILocalizable` and adding `LocalizedAttribute` to multi-language properties:

```csharp
public class Planet : ILocalizable
{
    public int PlanetId { get; set; }

    [Localized]
    public string Name { get; set; }
}
```

Internally multi-language content is stored as serialized JSON:

```js
planet.Name = "{'en':'Earth','ru':'Земля́','ja':'地球'}";
```

To localize a list, say pulled from a database with Entity Framework, you can use the provided `IObjectLocalizer.Localize<T>()` method:

```csharp
[HttpGet]
public async Task<IActionResult> Index()
{
    var planets = await _context.Planets.ToListAsync();

    planets = _localizer.Localize<Planet>(planets).ToList();

    return View(planets);
}
```

## Getting Started

### ASP.NET Core

#### 1. Add NuGet Packages

For ASP.NET Core projects you'll add the **Xaki** and **Xaki.AspNetCore** NuGet packages to your project. While these packages are beta you'll install from MyGet:

##### Package Manager

```powershell
Install-Package Xaki.AspNetCore
```

##### .NET CLI

```powershell
dotnet add package Xaki.AspNetCore
```

You may also want to add the NuGet feed above to your nuget.config file at the root of your solution:

#### 2. Add Xaki to Startup

Xaki follows the usual pattern to add and configure services in an ASP.NET Core host, to add Xaki and request localization update `Startup.cs` to include:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.AddMvc().AddXaki(new XakiOptions
    {
        RequiredLanguages = new[] { "en", "zh", "ar", "es", "hi" },
        OptionalLanguages = new[] { "pt", "ru", "ja", "de", "el" }
    });
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // ...

    app.UseXaki(); // must precede UseMvc()
    app.UseMvc();
}
```

For a sample ASP.NET Core app see [https://github.com/mehalick/Xaki/tree/master/samples/Xaki.Sample]().

#### 3. Create Localized Entity

Any Entity Framework POCO can be localizable by implementing `ILocalizable` with one or more properties decorated with `LocalizedAttribute`:

```csharp
public class Planet : ILocalizable
{
    public int PlanetId { get; set; }

    [Localized]
    public string Name { get; set; }
}
```

#### 4. Add Localization to Controllers

Similar to ASP.NET Core's `IStringLocalizer` and `IHtmlLocalizer` you can localize objects and collections with `IObjectLocalizer`, simply add it to any controller:

```csharp
[Route("planets")]
public class PlanetsController : Controller
{
    private readonly DataContext _context;
    private readonly IObjectLocalizer _localizer;

    public PlanetsController(DataContext context, IObjectLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }
}
```

You can now fetch entities and send the localized versions to your views:

```csharp
[HttpGet]
public async Task<IActionResult> Index()
{
    var planets = await _context.Planets.ToListAsync();

    planets = _localizer.Localize<Planet>(planets).ToList();

    return View(planets);
}
```

##### How does IObjectLocalizer resolve the current language?

`IObjectLocalizer` uses ASP.NET Core's `RequestLocalizationMiddleware` to resolve the current language and culture using:

1. Querystrings
2. Cookies
3. Accept-Language Header

For more information see [https://andrewlock.net/adding-localisation-to-an-asp-net-core-application/]().

If you'd like to customize how `IObjectLocalizer` resolves languages you can create your own resolver by implementing `Xaki.AspNetCore.LanguageResolvers.ILanguageResolver`.

#### 5. Editing Localized Entities

The **Xaki.AspNetCore** library includes a tag helper and model binder to make edit views and actions extremely simple. 

##### Tag Helper

To convert any input into a rich localization editor simply replace `<input for="Name" />` with `<input localized-for="Name" />`:

```html
<form asp-action="Edit">

    <input asp-for="PlanetId" type="hidden" />

    <div class="form-group">
        <label>Name</label>
        <input localized-for="Name" />
    </div>

    <button type="submit" class="btn btn-dark">Submit</button>

</form>
```

You'll automatically get a rich localization editor:

![editor](https://xaki.azureedge.net/assets/2018-09-11_10-26-05-636722475947053940.png)

The editor automatically lists the individual language textboxes in the order they are specified in `Startup.cs` and client-side validation is included:

![validation](https://xaki.azureedge.net/assets/2018-09-11_10-28-42-636722477515160922.png)

#### Model Binding

The **Xaki.AspNetCore** library includes `LocalizableModelBinder` which is automatically registered via `services.AddMvc().AddXaki()`. 

This allows the localization tag helper to correctly model bind to `ILocalized` entities and view models in your actions:

```csharp
[HttpPost("{planetId:int}")]
public async Task<IActionResult> Edit(Planet planet)
{
    _context.Entry(planet).State = EntityState.Modified;

    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
```

Here your localized properties are automatically bound:

![model binding](https://xaki.azureedge.net/assets/2018-09-11_10-34-56-636722481804812672.png)
