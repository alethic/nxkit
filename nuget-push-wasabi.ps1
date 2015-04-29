$NuGetDirectory=".nuget"
$NuGetExe="$NuGetDirectory\NuGet.exe"
$OutputDirectory="bin\"

foreach ($i in Get-Item "$OutputDirectory\*.nupkg") {
    & "$NuGetExe" push "$i" -Source https://www.myget.org/F/wasabi/api/v2/package
}
