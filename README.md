# MR.AspNetCore.ApiVersioning

AppVeyor | Travis
---------|-------
[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-aspnetcore-apiversioning/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-aspnetcore-apiversioning) | [![Travis](https://img.shields.io/travis/mrahhal/MR.AspNetCore.ApiVersioning.svg)](https://travis-ci.org/mrahhal/MR.AspNetCore.ApiVersioning)

[![NuGet version](https://img.shields.io/nuget/v/MR.AspNetCore.ApiVersioning.svg)](https://www.nuget.org/packages/MR.AspNetCore.ApiVersioning)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Simple api versioning for Asp.Net Core.

## How

List all available api versions in a base controller.
```cs
[Route("api/v{version}/[controller]")]
[ApiVersion("0.1, 0.2, 1.0")] // List all available versions.
public abstract class MyBaseController : Controller
{
}
```

And that's simply it. If you want constraints on certain actions, you can do stuff like this in addition to the above step:

```cs
public class UsersController : MyBaseController
{
    // /api/v0.1/users
    // /api/v0.2/users
    [ApiVersion("<1.0")]
    public IActionResult Get()
    {
        // ...
    }

    // /api/v1.0/users
    [ApiVersion(">=1.0")]
    public IActionResult GetV1()
    {
        // ...
    }
}
```

Applying `ApiVerions` attributes is additive. An action is a candidate only if all constraints apply (even constraints applied on a base controller).
