# OpenCRM v0.0.1-1

Il progetto pretende fungere da base a diversi moduli di gestione contenuti con la particolarita di gestire i dati in modo efficente e sicuro

## Avvio rapido (backend + SPA)
- Prerequisiti: dotnet SDK e Node.js/npm installati nel PATH.
- Avvio standard: `powershell -ExecutionPolicy Bypass -File .\run-all.ps1`.
- Avvio installando prima le dipendenze della SPA: `powershell -ExecutionPolicy Bypass -File .\run-all.ps1 -InstallDeps`.
- La SPA viene avviata con `npm run dev` in `Modules\OpenCRM.Manager\Client`, il backend con `dotnet run --project Modules\OpenCRM.Manager\OpenCRM.Manager.csproj`.
- Premi INVIO nella console per terminare entrambi i processi.

## Build (backend + SPA + publish multipiattaforma)
- Script: `powershell -ExecutionPolicy Bypass -File .\build-all.ps1 [-Configuration Release|Debug] [-InstallNodeDeps]`
- Cosa fa:
  1. Compila `Core\OpenCRM.Core\OpenCRM.Core.csproj`.
  2. Compila la SPA Angular da `Core\OpenCRM.Core.Web\Client` (`npm run build`, output in `Core\OpenCRM.Core.Web\ui`).
  3. Compila `Web\OpenCRM.Web.csproj`.
  4. Esegue `dotnet publish` self-contained e single-file per `linux-x64`, `win-x64`, `osx-x64` in `publish\<RID>` alla radice repo.
- Prerequisiti: dotnet SDK, Node.js/npm nel PATH.
- Esempi rapidi (bash o PowerShell):
  - Build default (Release):

    ```bash
      pwsh -ExecutionPolicy Bypass -File ./build-all.ps1
    ```

  - Build Debug:

  ```bash
      pwsh -ExecutionPolicy Bypass -File ./build-all.ps1 -Configuration Debug
   ```

  - Build dopo aver installato le dipendenze npm:

  ```bash
      pwsh -ExecutionPolicy Bypass -File ./build-all.ps1 -InstallNodeDeps
  ```

  - Build Debug e install deps:

  ```bash
      pwsh -ExecutionPolicy Bypass -File ./build-all.ps1 -Configuration Debug -InstallNodeDeps
  ```
