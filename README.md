<h1><img src="https://xaki.azureedge.net/assets/logo-text-dark-636572324193463289.svg" height="50" alt="Xaki"></h1>

[![Build status](https://ci.appveyor.com/api/projects/status/d217t6s3py0ce6nn?svg=true)](https://ci.appveyor.com/project/mehalick/xaki)
[![AppVeyor](https://img.shields.io/appveyor/ci/mehalick/xaki/master.svg)](https://ci.appveyor.com/project/mehalick/xaki)
[![AppVeyor](https://img.shields.io/appveyor/tests/mehalick/xaki/master.svg)](https://ci.appveyor.com/project/mehalick/xaki/build/tests)

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

To localize a list, say pulled from a database with Entity Framework, you can use the provided `Localize<T>()` extension method:

```csharp
public async Task<IActionResult> GetPlanets()
{
    var planets = await _db.Planets.ToListAsync();
    var results = planets.Localize<Planet>();
    
    return View(results);
}
```
