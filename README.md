# SwashbuckleEx

SwashbuckleEx is Modify SwaggerUI Info

# 功能
- 支持汉化
- 支持接口搜索功能（模块名、注释名、接口名）
- 支持开发进度说明、开发人员、备注等相关信息
- 支持区域文档
- 增加接口数量统计功能

# How To Use?
You should set `Web.config`
```xml
<system.webServer>
	<modules runAllManagedModulesForAllRequests="true">
		<remove name="WebDAVModule" />
	</modules>
	<handlers>
		<remove name="ExtensionlessUrlHandler-Integrated-4.0"/>
		<remove name="OPTIONSVerbHandler"/>
		<remove name="TRACEVerbHandler"/>
		<add name="ExtensionlessUrlHandler-Integrated-4.0" path="*." verb="*" type="System.Web.Handlers.TransferRequestHandler"
		preCondition="integratedMode,runtimeVersionv4.0"/>
	</handlers>
</system.webServer>
```

# 文档
- [Webapi文档描述-swagger优化](https://www.cnblogs.com/jianxuanbing/p/7376757.html)
- [Swagger使用教程 SwashbuckleEx](https://www.cnblogs.com/jianxuanbing/p/9187640.html)