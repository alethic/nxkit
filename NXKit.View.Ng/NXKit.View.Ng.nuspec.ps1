 param (
    [string]$NuGetDirectory = $(throw "-NuGetDirectory is required."),
    [string]$NuGetExe = $(throw "-NuGetExe is required."),
    [string]$Version = $(throw "-Version is required."),
    [string]$Properties = $(throw "-Properties is required."),
    [string]$OutputDirectory = $(throw "-OutputDirectory is required.")
 )

& $NuGetExe pack "NXKit.View.Ng\NXKit.View.Ng.csproj" -Version $NuGetVersion -Properties $Properties -OutputDirectory $OutputDirectory -Exclude "**\*.dll"