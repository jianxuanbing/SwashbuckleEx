:: 包搜索字符串
echo %1
:: 项目方案地址
echo %2
:: ApiKey
echo %3


:: 删除历史包
del %1 /f /q /a

:: 包名称
set nupkg=""

:: 打包
nuget Pack %2 -Build -Properties Configuration=Release

:: 更新包名称
for %%a in (dir /s /a /b ./%1) do (set nupkg=%%a)

:: 推送包
nuget push %nupkg% %3 -Source https://www.nuget.org/api/v2/package