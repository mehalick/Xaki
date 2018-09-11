# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\src\Xaki\artifacts) { 
    Remove-Item .\src\Xaki\artifacts -Force -Recurse 
}

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "beta-{0:D4}" -f [convert]::ToInt32($revision, 10)

exec { & dotnet build Xaki.sln -c Release --version-suffix=$revision }

Push-Location -Path .\tests\Xaki.Tests

try {
    exec { & dotnet test -c Release --no-build --no-restore }
} finally {
    Pop-Location
}

Pop-Location

$samples = Get-ChildItem .\samples\Xaki.Sample.*

foreach ($sample in $samples) {
    Push-Location -Path $sample

    try {
        exec { & dotnet run -c Release --no-build --no-restore }
    } catch {
    } finally {
        Pop-Location
    }
}

exec { & dotnet pack .\src\Xaki\Xaki.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$revision }
exec { & dotnet pack .\src\Xaki.AspNetCore\Xaki.AspNetCore.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$revision }
