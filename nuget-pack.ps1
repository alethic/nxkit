$NuGetDirectory=".nuget"
$NuGetExe="$NuGetDirectory\NuGet.exe"

& $NuGetExe restore "$NuGetDirectory\packages.config" -PackagesDirectory ".\packages"

$NuGetVersion=((packages\GitVersion.CommandLine.2.0.1\Tools\GitVersion.exe /output json) -join ' ' | ConvertFrom-Json).NuGetVersionV2
$Properties="Configuration=Release;VisualStudioVersion=12.0;CodeContractsRunCodeAnalysis=false;NuGetVersion=$NuGetVersion"
$OutputDirectory="bin\"

if (!(Test-Path $OutputDirectory)) {
    New-Item -ItemType Directory -Path $OutputDirectory
}

Remove-Item "$OutputDirectory\*.nupkg"

foreach ($i in get-item *\*.nuspec) {
    $ProjectFile=( $i.FullName -replace '\.nuspec$','.csproj' )
    $PackFile=( $i.FullName -replace '\.nuspec$', '.nuspec.ps1' )
    if (Test-Path $PackFile) {
        & $PackFile -NuGetDirectory $NuGetDirectory -NuGetExe $NuGetExe -Version $NuGetVersion -Properties $Properties -OutputDirectory $OutputDirectory
    } else {
        & $NuGetExe pack $ProjectFile -Version $NuGetVersion -Properties $Properties -OutputDirectory $OutputDirectory -Symbols
    }
}
