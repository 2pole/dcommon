..\.nuget\nuget pack DCommon\DCommon.nuspec -Version 1.0.20 -OutputDirectory DCommon
pause

..\.nuget\nuget push DCommon\DCommon.1.0.20.nupkg
pause



