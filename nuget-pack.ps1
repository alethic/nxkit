Get-Item *\*.nuspec | %{ & .\.nuget\NuGet.exe pack ( $_.FullName -replace '\.nuspec$','.csproj' ) }
