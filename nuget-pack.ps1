Get-Item *\*.nuspec | %{ & .\.nuget\NuGet.exe pack ( $_.FullName -replace '\.nuspec$','.csproj' ) -Version 1.2.3.4 }
