# SwashbuckleEx

SwashbuckleEx is Modify SwaggerUI Info

# ����
- ֧�ֺ���
- ֧�ֽӿ��������ܣ�ģ������ע�������ӿ�����
- ֧�ֿ�������˵����������Ա����ע�������Ϣ
- ֧�������ĵ�
- ���ӽӿ�����ͳ�ƹ���

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

# �ĵ�
- [Webapi�ĵ�����-swagger�Ż�](https://www.cnblogs.com/jianxuanbing/p/7376757.html)
- [Swaggerʹ�ý̳� SwashbuckleEx](https://www.cnblogs.com/jianxuanbing/p/9187640.html)