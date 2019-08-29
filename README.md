[![NuGet Status](https://img.shields.io/nuget/v/Altairis.Services.PwnedPasswordsValidator.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Altairis.Services.PwnedPasswordsValidator/)

# Pwned Passwords Validator

This project is ASP.NET Identity Password Validator that checks candidate password against [Pwned Passwords](https://www.troyhunt.com/ive-just-launched-pwned-passwords-version-2/) by Troy Hunt. If the password is found in leaked passwords, it's refused.

> There is a [blog article](https://www.altair.blog/2019/08/pwned-passwords-validator) and [live coding session recording](https://youtu.be/t_ZleMiX_z8) available, but in Czech language only.

## Basic use

1. Install package `Altairis.Services.PwnedPasswordsValidator`.
2. Register the `PwnedPasswordsValidator` class in the `ConfigureServices` method of your startup class, ie. with the default settings:

```cs
services.AddDefaultIdentity<IdentityUser>()
    .AddDefaultUI(UIFramework.Bootstrap4)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddPasswordValidator<PwnedPasswordsValidator<IdentityUser>>();
```

## Configuration

There is single configuration parameter and that's request timeout, which is by default 5 seconds. If the server does not respond within defined timeout, the password is allowed and error is logged.

To configure the timeout, inject the `PwnedPasswordsValidatorOptions` class:

```cs
services.Configure<PwnedPasswordsValidatorOptions>(c => {
    c.RequestTimeout = TimeSpan.FromSeconds(10);
});
```

## Acknowledgements

* [This tweet](https://twitter.com/troyhunt/status/1165935897075781633) by Troy Hunt was my primary inspiration.
* The [Creating a validator to check for common passwords in ASP.NET Core Identity](https://andrewlock.net/creating-a-validator-to-check-for-common-passwords-in-asp-net-core-identity/) article by Andrew Lock was another source.
* I'm using the [Have I Been Pwned](https://www.haveibeenpwned.com/) service by Troy Hunt

## Author & Legal

* This project was created by [Michal Altair Valášek](https://www.rider.cz/)
* I'm [Microsoft MVP](https://mvp.microsoft.com/en-us/PublicProfile/9610) for Visual Studio and Development Technologies
* Licensed under terms of the [MIT License](LICENSE)
* This project has [No Code of Conduct (NCoC)](CODE_OF_CONDUCT.md)
