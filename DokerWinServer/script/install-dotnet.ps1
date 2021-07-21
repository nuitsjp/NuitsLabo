$DotNetRoot='C:\Program Files\dotnet'

&([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -Channel 6.0 -Quality preview -InstallDir $DotNetRoot; `
[System.Environment]::SetEnvironmentVariable('DOTNET_ROOT', $DotNetRoot, 'Machine');
[System.Environment]::SetEnvironmentVariable('Path', $DotNetRoot + ';' + $env:Path, 'Machine'); `
[System.Environment]::SetEnvironmentVariable('DOTNET_MULTILEVEL_LOOKUP', 0, 'Machine')
