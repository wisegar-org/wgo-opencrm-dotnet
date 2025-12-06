param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    [switch]$InstallNodeDeps
)

$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
$coreProj = Join-Path $root 'Core\OpenCRM.Core\OpenCRM.Core.csproj'
$webProj = Join-Path $root 'Web\OpenCRM.Web.csproj'
$clientDir = Join-Path $root 'Core\OpenCRM.Core.Web\Client'
$publishRoot = Join-Path $root 'publish'

$dotnetExe = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnetExe) {
    throw 'dotnet SDK non trovato. Installa la versione usata dal progetto prima di eseguire lo script.'
}

$npmCmd = Get-Command npm.cmd -ErrorAction SilentlyContinue
if (-not $npmCmd) {
    $npmCmd = Get-Command npm -ErrorAction SilentlyContinue
}
if (-not $npmCmd) {
    throw 'npm non trovato. Installa Node.js (include npm) prima di eseguire lo script.'
}

if (-not (Test-Path $coreProj)) {
    throw "Progetto non trovato: $coreProj"
}
if (-not (Test-Path $webProj)) {
    throw "Progetto non trovato: $webProj"
}
if (-not (Test-Path (Join-Path $clientDir 'package.json'))) {
    throw "Cartella SPA non trovata: $clientDir"
}

Write-Host "Build OpenCRM.Core ($Configuration)..." -ForegroundColor Cyan
& $dotnetExe.Source build $coreProj -c $Configuration

if ($InstallNodeDeps -or -not (Test-Path (Join-Path $clientDir 'node_modules'))) {
    Write-Host 'Installazione dipendenze SPA (npm install)...' -ForegroundColor Cyan
    Push-Location $clientDir
    try {
        & $npmCmd.Source install --no-fund --no-audit
    }
    finally {
        Pop-Location
    }
}

Write-Host 'Build SPA Angular (ng build)...' -ForegroundColor Cyan
Push-Location $clientDir
try {
    & $npmCmd.Source run build
}
finally {
    Pop-Location
}

Write-Host "Build Web project ($Configuration)..." -ForegroundColor Cyan
& $dotnetExe.Source build $webProj -c $Configuration

if (Test-Path $publishRoot) {
    Write-Host "Pulizia cartella publish ($publishRoot)..." -ForegroundColor Cyan
    Remove-Item -Path $publishRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $publishRoot | Out-Null

$rids = @('linux-x64', 'win-x64', 'osx-x64')
foreach ($rid in $rids) {
    $output = Join-Path $publishRoot $rid
    Write-Host "Publish Web self-contained single-file for $rid..." -ForegroundColor Cyan
    & $dotnetExe.Source publish $webProj -c $Configuration -r $rid --self-contained true `
        /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=false `
        -o $output
}

Write-Host "Build completato. Output publish: $publishRoot" -ForegroundColor Green
