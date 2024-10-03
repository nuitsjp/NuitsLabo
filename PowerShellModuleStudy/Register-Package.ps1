Unregister-PSRepository -Name NuitsGitHub -ErrorAction SilentlyContinue

Register-PSRepository -Name NuitsGitHub `
    -SourceLocation "https://nuget.pkg.github.com/nuitsjp/index.json" `
    -PublishLocation "https://nuget.pkg.github.com/nuitsjp/index.json" `
    -InstallationPolicy Trusted `
    -Credential $credential