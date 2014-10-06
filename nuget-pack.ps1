$NuGetVersion=((packages\GitVersion.CommandLine.1.2.1\Tools\GitVersion.exe /output json) -join ' ' | ConvertFrom-Json).NuGetVersionV2
$Properties="Configuration=Release;VisualStudioVersion=12.0"
$OutputDirectory="bin\"

if (!(Test-Path $OutputDirectory)) {
    New-Item -ItemType Directory -Path $OutputDirectory
}

foreach ($i in get-item *\*.nuspec) {
    $ProjectFile=( $i.FullName -replace '\.nuspec$','.csproj' )
    .\.nuget\NuGet.exe pack $ProjectFile -Build -Symbols -Version $NuGetVersion -Properties $Properties -OutputDirectory $OutputDirectory
}
