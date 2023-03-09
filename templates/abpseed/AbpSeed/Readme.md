# 项目说明

## 集成内容

- EFCore
- Swagger UI
- Autofac 依赖注入
- 基本的RBAC, 用户, 菜单等数据接口
- 缓存


## 项目描述
- AbpSeed 主入口项目
- App.Api 写Controller的地方
- App.Contracts 常量\枚举\接口等
- App.Entities 数据库model
- App.Models 请求和返回的model
- EFCore.xxx 对应数据库的DBContext, xxx为数据库类型
- App.DbMigrator 控制台程序, 用于创建和同步数据库以及表结构