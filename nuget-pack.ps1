$NuGetDirectory=".nuget"
$NuGetExe="$NuGetDirectory\NuGet.exe"
$NuGetVersion=((packages\GitVersion.CommandLine.1.2.1\Tools\GitVersion.exe /output json) -join ' ' | ConvertFrom-Json).NuGetVersionV2
$Properties="Configuration=Release;VisualStudioVersion=12.0;CodeContractsRunCodeAnalysis=false;NuGetVersion=$NuGetVersion"
$OutputDirectory="bin\"

if (!(Test-Path $OutputDirectory)) {
    New-Item -ItemType Directory -Path $OutputDirectory
}

Remove-Item "$OutputDirectory\*.nupkg"

foreach ($i in get-item *\*.nuspec) {
    $ProjectFile=( $i.FullName -replace '\.nuspec$','.csproj' )
    & $NuGetExe pack $ProjectFile -Version $NuGetVersion -Properties $Properties -OutputDirectory $OutputDirectory
}
