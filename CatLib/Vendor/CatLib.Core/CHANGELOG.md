# Release Notes

## [v1.4.0 (2019-03-20)](https://github.com/CatLib/Core/releases/tag/v1.4.0)

**Added**

- `Arr`增加`Test`函数，支持对数组进行自定义检查([#178](https://github.com/CatLib/Core/issues/178) )
- `Arr`增加`Set`函数，允许替换匹配值或者在尾部增加替换值([#190](https://github.com/CatLib/Core/issues/190) )
- `Str`增加`Is`重载，允许对于数组类型的模式进行匹配([#179](https://github.com/CatLib/Core/issues/179) )
- `Str`增加`JoinList`函数，允许顺序组合给定的字符串数组([#195](https://github.com/CatLib/Core/issues/195) )
- `Str`增加`Levenshtein`函数，允许计算两个字符串之间的近似度([#194](https://github.com/CatLib/Core/issues/194) )
- `Str`增加`Space`常量代表一个含有空格的空字符串([#197](https://github.com/CatLib/Core/issues/197) )
- `Application.Register`增加强制注册支持，允许在已注册服务提供者的基础上强制注册([#184](https://github.com/CatLib/Core/issues/184) )
- `增加CHANGELOG`([#185](https://github.com/CatLib/Core/issues/185) )

**Changed**

- 代码标准规范调整：只有在引导流程完成后才能进行容器构建([#180](https://github.com/CatLib/Core/issues/180) )
- `Application`代码优化([#181](https://github.com/CatLib/Core/issues/181) )
- `Arr`中的`Filter`函数，允许传入一个期望值来决定结果集([#196](https://github.com/CatLib/Core/issues/196) )
- `IContainer.Flash` 标记将会在2.0版本中被移除([#193](https://github.com/CatLib/Core/issues/193) )
- `Str`中的`Pad`函数增加拓展并且标记旧的格式为已过时([#198](https://github.com/CatLib/Core/issues/198) )
- 单元测试项目升级到4.0 framework([#191](https://github.com/CatLib/Core/issues/191) )