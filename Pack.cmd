@echo off
ECHO IMPORTANT TO BUILD IN RELEASE MODE RUN THIS IN THE DEVELOPER COMMAND PROMPT!!!
ECHO ------------------------------------------------------------------------------
set /p nugetLocation="Enter nuget location [specified in path variable]: "
set /p exportLocation="Enter export location [userprofile\Desktop\NugetRelease\]: "

msbuild GPS.NET\GPS.NET.sln /t:Rebuild /p:Configuration=Release

ECHO "Exporting packages"
IF "%nugetLocation%" equ "" (
	nuget pack GPS.NET\Ghostware.GPS.NET\Ghostware.GPS.NET.csproj -Prop Configuration=Release
) ELSE (
%nugetLocation%\nuget.exe pack GPS.NET\Ghostware.GPS.NET\Ghostware.GPS.NET.csproj -Prop Configuration=Release
)

IF "%exportLocation%" equ "" (
	ECHO "Exporting to %userprofile%\Desktop\NugetRelease\"
	IF not exist %userprofile%\Desktop\NugetRelease mkdir %userprofile%\Desktop\NugetRelease
	move *.nupkg %userprofile%\Desktop\NugetRelease\
) ELSE (
	ECHO "Exporting to %exportLocation%"
	IF not exist "%exportLocation%" mkdir "%exportLocation%"
	move *.nupkg %exportLocation%
)

PAUSE