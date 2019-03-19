
# CatLib For Unity

这是CatLib For Unity的引导框架，您不应该手动修改这个文件夹下的任何内容，这会导致未来升级框架变得困难。

## 如何使用

- 创建`<项目代码目录>/Main.cs`继承自`Framework.cs`,并实现`OnStartCompleted`方法。

```csharp
public sealed class Main : Framework
{
    protected override void OnStartCompleted()
    {
    }
}
```

您的入口应该在这个函数中开始书写。

## 自定义引导程序

- 修改`<项目代码目录>/Main.cs`，覆盖`GetBootstraps`方法，加入自己的引导程序列表。

```csharp
protected override IBootstrap[] GetBootstraps()
{
    return Arr.Merge(base.GetBootstraps(), Bootstraps.Bootstrap);
}
```

- 建立引导文件`<项目代码目录>/Bootstraps.cs`

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

> 其中`Assemblys`和`Providers`为对应的列表，请自行创建。

**框架已经为您提供的引导程序**

- `BootstrapProviderRegister`：服务提供者注册引导，可以将指定`服务提供者列表`和`GameObject下的服务提供者`注册到框架。
- `BootstrapTypeFinder`: 类型查找器注册引导，允许开发者将指定的程序集加入服务容器的反射列表，这样在不进行任何绑定的情况下可以从服务容器生成指定类型。

## 如何升级

所有的次要版本升级，你只需要覆盖`CatLib`文件夹就可以完成。

主要版本或特殊版本升级，请参考[迁移指南](https://catlib.io/v1/migration.html)

## 技术支持

* 通过[框架帮助文档](https://catlib.io)自行查找解决方案（推荐）
* 通过[Issues](https://github.com/CatLib/CatLib/issues)直接发起问题（推荐）
* QQ群: 150371044 (验证: CatLib Support)
* email: support@catlib.io
* slack: [catlib.slack](https://catlib.slack.com/messages/internals/)
