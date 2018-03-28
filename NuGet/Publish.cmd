..\.nuget\nuget pack DCommon\DCommon.nuspec -Version 1.1.0 -OutputDirectory DCommon
..\.nuget\nuget pack DCommon.Mvc4\DCommon.Mvc4.nuspec -Version 1.1.0 -OutputDirectory DCommon.Mvc4
..\.nuget\nuget pack DCommon.Mvc.Bootstrap\DCommon.Mvc.Bootstrap.nuspec -Version 1.1.0 -OutputDirectory DCommon.Mvc.Bootstrap
..\.nuget\nuget pack DCommon.EF\DCommon.EF.nuspec -Version 1.1.0 -OutputDirectory DCommon.EF
#..\.nuget\nuget pack DCommon.EF6\DCommon.EF6.nuspec -Version 1.1.0 -OutputDirectory DCommon.EF6
..\.nuget\nuget pack DCommon.NHibernate\DCommon.NHibernate.nuspec -Version 1.1.0 -OutputDirectory DCommon.NHibernate
..\.nuget\nuget pack DCommon.LinqToNPOI\DCommon.LinqToNPOI.nuspec -Version 1.1.0 -OutputDirectory DCommon.LinqToNPOI
pause

..\.nuget\nuget push DCommon\DCommon.1.1.0.nupkg
..\.nuget\nuget push DCommon.Mvc4\DCommon.Mvc4.1.1.0.nupkg
..\.nuget\nuget push DCommon.Mvc.Bootstrap\DCommon.Mvc.Bootstrap.1.1.0.nupkg
..\.nuget\nuget push DCommon.EF\DCommon.EF.1.1.0.nupkg
#..\.nuget\nuget push DCommon.EF6\DCommon.EF6.1.1.0.nupkg
..\.nuget\nuget push DCommon.NHibernate\DCommon.NHibernate.1.1.0.nupkg
..\.nuget\nuget push DCommon.LinqToNPOI\LinqToNPOI.1.1.0.nupkg
pause



