# OpenCRM v0.0.1-1

Il progetto pretende fungere da base a diversi moduli di gestione contenuti con la particolarita di gestire i dati in modo efficente e sicuro

## Avvio rapido (backend + SPA)
- Prerequisiti: dotnet SDK e Node.js/npm installati nel PATH.
- Avvio standard: `powershell -ExecutionPolicy Bypass -File .\run-all.ps1`.
- Avvio installando prima le dipendenze della SPA: `powershell -ExecutionPolicy Bypass -File .\run-all.ps1 -InstallDeps`.
- La SPA viene avviata con `npm run dev` in `Modules\OpenCRM.Manager\ui`, il backend con `dotnet run --project Modules\OpenCRM.Manager\OpenCRM.Web.csproj`.
- Premi INVIO nella console per terminare entrambi i processi.
