
# CatLib For Unity

This is the boot framework for CatLib For Unity. You should not manually modify anything under this folder, which can make it difficult to upgrade the framework in the future.

## Getting Started

- Create `<project code directory>/Main.cs` inherited from `Framework.cs` and implement the `OnStartCompleted` method.

```csharp
public sealed class Main : Framework
{
    protected override void OnStartCompleted(IApplication appliction, StartCompletedEventArgs args)
    {
    }
}
```

Your entry should start writing in this function.

## Custom bootloader

- Modify `<project code directory>/Main.cs`, override the `GetBootstraps` method, and add your own bootstrap list.

```csharp
protected override IBootstrap[] GetBootstraps()
{
    return Arr.Merge(base.GetBootstraps(), Bootstraps.Bootstrap);
}
```

- Create boot file `<project code directory>/Bootstraps.cs`

```csharp
using CatLib;
using UnityEngine;
```

```csharp
public static class Bootstraps
{
    public static IBootstrap[] GetBoostraps(Component component)
    {
        return new IBootstrap[]
        {
            new BootstrapTypeFinder(Assemblys.Assembly),
            new BootstrapProviderRegister(component, Providers.ServiceProviders),
        };
    }
}
```

> Where `Assemblys` and `Providers` are the corresponding lists, please create them yourself.

**Default provided bootloader**

- `BootstrapProviderRegister`：The service provider registers the bootstrap and can register the specified `service provider list` and the service provider under `GameObject` to the framework.
- `BootstrapTypeFinder`: The type finder registration bootstrap allows the developer to add the specified assembly to the reflection list of the service container so that the specified type can be generated from the service container without any binding.

## Upgrade

All minor versions are upgraded and you only need to overwrite the `CatLib` folder to complete.

For major or special version upgrades, please refer to the [Migration Guide](https://catlib.io/v2/migration.html)

## Support

* Find your own solution via [Documentation](https://catlib.io)(recommended)
* Initiate questions directly via [Issues](https://github.com/CatLib/CatLib/issues)(recommended)
* QQ group: 150371044 (Verification: CatLib Support)
* email: support@catlib.io
* slack: [catlib.slack](https://catlib.slack.com/messages/internals/)
