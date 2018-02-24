set /p key=input key:
call ./Publish.bat "SwashbuckleEx.*.nupkg" "Swashbuckle.WebHost/Swashbuckle.WebHost.csproj" "%key%"
call ./Publish.bat "SwashbuckleEx.Core.*.nupkg" "Swashbuckle.Core/Swashbuckle.Core.csproj" "%key%"
@pause