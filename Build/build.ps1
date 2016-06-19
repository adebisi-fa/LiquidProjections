param (
	[string] $assemblyVersion = "1.2.3.4",
	[string] $informationalVersion = "1.2.3-unstable.34+34.Branch.develop.Sha.19b2cd7f494c092f87a522944f3ad52310de79e0",
	[string] $nuGetVersion = "1.2.3-unstable0012"
)

"Assembly version $assemblyVersion"
"Informational version $informationalVersion"
"NuGet package version $nuGetVersion"

$packageConfigs = Get-ChildItem . -Recurse -Filter "packages.config" | where {$_.FullName -notlike "*\packages\*"}
foreach($packageConfig in $packageConfigs){
	Write-Host "Restoring" $packageConfig.FullName 
	.\tools\nuget.exe install $packageConfig.FullName -OutputDirectory "Sources\Packages" -ConfigFile "Sources\.nuget\NuGet.Config"
}

Import-Module .\Sources\Packages\psake.4.4.1\tools\psake.psm1
Import-Module .\Build\Modules\BuildFunctions.psm1
Import-Module .\Build\Modules\teamcity.psm1
Import-Module .\build\Modules\ILMerge.psm1
Invoke-Psake .\Build\default.ps1 default -framework "4.0" -properties @{ AssemblyVersion=$assemblyVersion; InformationalVersion=$informationalVersion; NuGetVersion=$nuGetVersion }
Remove-Module BuildFunctions
Remove-Module psake