<h1><img src="https://xaki.azureedge.net/assets/logo-text-dark-636572324193463289.svg" height="50" alt="Xaki"></h1>

[![Build status](https://ci.appveyor.com/api/projects/status/d217t6s3py0ce6nn?svg=true)](https://ci.appveyor.com/project/mehalick/xaki)
[![AppVeyor](https://img.shields.io/appveyor/ci/mehalick/xaki/master.svg)](https://ci.appveyor.com/project/mehalick/xaki)
[![AppVeyor](https://img.shields.io/appveyor/tests/mehalick/xaki/master.svg)](https://ci.appveyor.com/project/mehalick/xaki/build/tests)

Xaki is a .NET library to add multi-language support to POCO classes. It includes a lightweight service to serialize multi-language properties 
for database storage and to localize them back to specified languages.

Xaki includes ASP.NET Core support to leverage `RequestLocalizationOptions` and `RequestCultureProviders` to access language codes provided by routes, querystrings, cookies, and HTTP headers.

Xaki works well with all versions of Entity Framework and Entity Framework Core. 
