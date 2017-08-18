# SwashbuckleEx

SwashbuckleEx is Modify SwaggerUI Info

# How To User?
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
