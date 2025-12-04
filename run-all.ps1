param(
    [switch]$InstallDeps
)

# Avvia backend ASP.NET Core e frontend Quasar in parallelo.
$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
$backendProj = Join-Path $root 'Modules\OpenCRM.Manager\OpenCRM.Web.csproj'
$uiDir = Join-Path $root 'Modules\OpenCRM.Manager\ui'

$dotnetExe = (Get-Command dotnet -ErrorAction SilentlyContinue)
if (-not $dotnetExe) {
    throw 'dotnet SDK non trovato. Installa la versione usata dal progetto prima di eseguire lo script.'
}

# Usa npm.cmd esplicitamente per evitare problemi di esecuzione su Windows
$npmCmd = (Get-Command npm.cmd -ErrorAction SilentlyContinue)
if (-not $npmCmd) {
    $npmCmd = (Get-Command npm -ErrorAction SilentlyContinue)
}
if (-not $npmCmd) {
    throw 'npm non trovato. Installa Node.js (include npm) prima di eseguire lo script.'
}

if (-not (Test-Path $backendProj)) {
    throw "Progetto backend non trovato in $backendProj"
}

if (-not (Test-Path (Join-Path $uiDir 'package.json'))) {
    throw "Cartella SPA non trovata in $uiDir"
}

if ($InstallDeps) {
    Write-Host 'Installazione dipendenze SPA (npm install)...' -ForegroundColor Cyan
    Push-Location $uiDir
    try {
        & $npmCmd.Source install --no-fund --no-audit
    }
    finally {
        Pop-Location
    }
}
elseif (-not (Test-Path (Join-Path $uiDir 'node_modules'))) {
    Write-Warning "node_modules non esiste. Esegui 'npm install' in $uiDir oppure lancia lo script con -InstallDeps."
}

Write-Host 'Avvio backend ASP.NET Core...' -ForegroundColor Cyan
$backend = Start-Process -FilePath $dotnetExe.Source -ArgumentList @('run','--project', $backendProj) -WorkingDirectory $root -PassThru -WindowStyle Normal

Write-Host 'Avvio SPA Quasar (npm run dev)...' -ForegroundColor Cyan
$frontend = Start-Process -FilePath $npmCmd.Source -ArgumentList @('run','dev') -WorkingDirectory $uiDir -PassThru -WindowStyle Normal

Write-Host "Processi avviati. Backend PID: $($backend.Id) | Frontend PID: $($frontend.Id)" -ForegroundColor Green
Write-Host 'Premi INVIO per chiuderli entrambi...' -ForegroundColor Yellow

try {
    [Console]::ReadLine() | Out-Null
}
finally {
    foreach ($p in @($frontend, $backend)) {
        if ($p -and -not $p.HasExited) {
            Stop-Process -Id $p.Id -Force -ErrorAction SilentlyContinue
        }
    }
    Write-Host 'Processi terminati.' -ForegroundColor Green
}
