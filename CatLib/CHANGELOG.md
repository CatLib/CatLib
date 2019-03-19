## [v1.4.0 (2019-03-20)](https://github.com/CatLib/CatLib/releases/tag/v1.4.0)

**Added**

- 增加对于Unity编辑器下的引导库支持([#14](https://github.com/CatLib/CatLib/issues/14) )
- 增加`HelpURL`链接的支持可以直接跳转到文档网页([#23](https://github.com/CatLib/CatLib/issues/23) )
- 编辑器增加服务提供者的可视化选择支持([#24](https://github.com/CatLib/CatLib/issues/24) )
- 增加了`CHANGELOG.md`([#29](https://github.com/CatLib/CatLib/issues/29) )

**Changed**

- 同步了[CatLib.Core v1.4.0-beta](https://github.com/CatLib/Core/releases/tag/v1.4.0)版本([#22](https://github.com/CatLib/CatLib/issues/22) )
- 所有继承自`Component`的类型会被判定为不可以被构建的类型([#18](https://github.com/CatLib/CatLib/issues/18) )
- 引导程序结构优化([#19](https://github.com/CatLib/CatLib/issues/19) )
- `BootstrapProviderRegister` 会检查`Component`的挂载是否允许被获取，而不是引发一个异常([#20](https://github.com/CatLib/CatLib/issues/20) )
- 将继承自`MonoBehaviour`的服务提供者以代码的形式进行注册会引发一个异常([#25](https://github.com/CatLib/CatLib/issues/25) )

**Removed**

- `Framework.cs` 主引导类移除了默认不必要的事件注册。([#21](https://github.com/CatLib/CatLib/issues/21) )