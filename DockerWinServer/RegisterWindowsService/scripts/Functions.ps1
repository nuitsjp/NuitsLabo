class WindowsService
{
        [string]$Name
        [string]$BinaryPathName
        [string]$DisplayName
        [string]$Description
}

function Get-WindowsServices()
{
        [WindowsService[]]$services = [WindowsService[]]::new(1)
        $service = [WindowsService]::new()
        $service.Name = "SimpleWindowsService1"
        $service.BinaryPathName = "C:\services\シンプルサービス1\SimpleWindowsService.exe"
        $service.DisplayName = "シンプルサービス1"
        $service.Description = "ダミー登録用のWindowsサービスです"
        $services[0] = $service;
        return $services
}

function Start-WindowsService ([String] $serviceName)
{
        $service = Get-Service $serviceName
        if ( $service.Status -eq [ServiceProcess.ServiceControllerStatus]::Running ) {
                return
        }
        $service.Start()
}

function Register-WindowsService([WindowsService]$service)
{
        New-Service `
                -Name $service.Name `
                -BinaryPathName $service.BinaryPathName `
                -DisplayName $service.DisplayName `
                -StartupType AutomaticDelayedStart `
                -Description $service.Description `
                | Out-Null
        # Start-WindowsService $service.Name

        Write-Host ("Windowsサービス " + $service.DisplayName + " を登録しました") -ForegroundColor Blue
}

function Write-Exception([System.Exception]$exception)
{
        $msg = ''
        $err = $exception
        do
        {
            $msg += [System.Environment]::NewLine
            $msg += $err
            $err = $err.InnerException
        } while ($err)

        Write-Host $msg -ForegroundColor Red
}

