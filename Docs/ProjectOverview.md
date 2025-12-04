# OpenCRM – panoramica del repository

## Struttura della soluzione .NET
- `OpenCRM.sln`: contiene librerie core, moduli, sito web host, setup VB e due front-end Angular (cartella `UI/`).
- Target principale: ASP.NET Core 7 con Identity, EF Core (Npgsql), servizi modulari + CI/CD GitHub Actions verso Azure WebApp.

### Libreria Core (`Core/OpenCRM.Core`)
- **Data layer** (`Data/`): `DataContext` (deriva da `IdentityDbContext`), tracciamento `AddedAt/UpdatedAt/DeletedAt` via `ChangeTracker`. Entità base (`BaseEntity`, `IHasTimestamps`) e tabelle per media, history, data blocks, lingue, traduzioni, identity (user/role/claims/sessions).
- **Crypto**: `RSACryptoService` (generazione, cifratura/decifratura RSA 2048) + `RSAKeyPairsModel` (serializzazione XML delle chiavi).
- **QRCode**: `QRCodeService` (genera bitmap QR, variante colorata, conversione in byte[]).
- **Extensions**: `OpenCRMEnv` per configurare/recuperare `WEBROOT`.
- **Models**: enum `MediaType`.
- **Test** (`Core/OpenCRM.Core.Test`): `RSACryptoServiceTest` verifica generazione chiavi e round-trip cifratura.

### Libreria Web Core (`Core/OpenCRM.Core.Web`)
- `StartupModuleExtensions.AddOpenCRM<TDBContext>()`: registra DbContext Postgres, servizi (media, data block, email SMTP, lingue, traduzioni, identity/ruoli, QRCode, sessioni), Identity Cookie/Antiforgery, Microsoft Identity Web API + Graph.
- `UseOpenCRM<TDBContext>()`: crea il DB se manca, seed di lingue, ruoli e utente admin (`info@opends.io`), invio email demo, conferma email.
- **Servizi**:
  - `DataBlockService`: CRUD generico per blocchi dati tipizzati (JSON in `DataBlocks`).
  - `CardBlockService`: logica per blocchi “card” (usa `DataBlockService` + `MediaService` per immagini).
  - `MediaService`: upload multiplo/singolo, salvataggio su DB e su disco in `wwwroot/media`, URL generation (via `IHttpContextAccessor`).
  - `LanguageService` / `TranslationService`: CRUD + seed lingue base (`EN-gb`, `ESes`), lookup traduzioni (da JSON serializzato).
  - `IdentityService`: registrazione utenti con coppia RSA salvata, login/logout, conferma email, cookie sessione custom `OpenCRM.Session`, seed utente admin e ruoli.
  - `RoleService`: crea ruoli `USER`, `ADMIN`, `SUPER_ADMIN` e assegnazioni.
  - `UserSessionService`: placeholder (vuoto).
  - `EmailService`: invio SMTP configurato via `appsettings` (`Email:SMTP`).
- **UI shared**: componenti Razor (Navbar, Breadcrumb, Dropdown, Table ecc.), modelli view (`CardBlockModel`, `TranslationModel`, `UserModel`, `BreadCrumbModel`, ecc.), pagine Identity/Shared.

### Moduli
- **OpenCRM.Finance**: servizio `AccountingService` basato su `DataBlockService` per registrare movimenti (`AccountingModel` con `DEBIT/CREDIT`, `Ammount`, `Description`), metodo `Seed` di esempio. Startup registra/seed.
- **OpenCRM.SwissLPD**: `EventService` (CRUD eventi via `DataBlockService`), `RoleService` (validazione utenti per CHE code leggendo `UserExtras` JSON), `Startup` registra servizi e chiama seed eventi (stub).
- Entrambi i moduli espongono estensioni `AddOpenCRM*` e `UseOpenCRM*` per essere agganciati all’host.

### Host web (`OpenCRM.Web` in `Modules/OpenCRM.Manager`)
- `Program.cs`: configura servizi `AddOpenCRM<OpenCRMDataContext>()` e modulo SwissLPD, Razor Pages + Controllers, localization (en/fr), forwarded headers, static files, auth, routing; chiama `UseOpenCRM` + `UseOpenCRMSwissLPDAsync`.
- `OpenCRMDataContext`: DbContext concreto (connessione Npgsql di default se non configurata).
- `Controllers/UserController`: endpoint API demo `GET /api/user`.
- Pagine Razor standard (`Index`, `Privacy`, `Error`) più layout/shared.
- `wwwroot`: static assets (Bootstrap, js, css).
- `appsettings*.json`: connessioni, logging, SMTP, AzureAd (per Microsoft Identity Web).

### Setup (`OpenCRM.Setup`)
- Progetto VB WinForms/WPF (file `Setup.vb`, `Setup.Designer.vb`, `ApplicationEvents.vb`) presumibilmente per installazione/configurazione client; servizi placeholder in `Services/`.

### Front-end Angular (`UI/`)
- `OpenCRM.Backoffice`: app Angular 20 (CLI), configurazione standard, nessun dominio applicativo evidente nel codice mostrato (probabile scaffold iniziale).
- `OpenCRM.Universal.Components`: libreria/componenti Angular 20, struttura CLI base.
- Entrambi con `package.json` e lock; README standard con comandi `ng serve/build/test`.

### CI/CD
- GitHub Actions `.github/workflows/opencrm-web.yml` e `development_opencrm-web.yml`: restore/build/test `OpenCRM.Web`, publish artefatti e deploy su Azure WebApp `opencrm-web` (profili di pubblicazione nei secrets).

### Schema architetturale
- Diagramma in `Docs/Files/OpenCRM_Architecture_Diagram.png` (non ispezionato) e note in `Docs/OpenCRM.md` che chiariscono:
  - `OpenCRM.Core.Web` dovrebbe contenere l’UI riusabile in progetti ASP.NET Core esterni.
  - `OpenCRM.Web` è un head project “non necessario”.
  - Uso di Bootstrap per componenti, separazione delle responsabilità tramite servizi.

## Come avviare
- **Backend**: da `Modules/OpenCRM.Manager`, impostare connection string `DBConnection` (Npgsql) in `appsettings.json`; `dotnet run` (crea DB e dati seed). Richiede SMTP configurato se si vuole inviare email.
- **Front-end Angular**: `cd UI/OpenCRM.Backoffice && npm install && ng serve` (idem per `OpenCRM.Universal.Components`).

## Note e possibili punti di attenzione
- Molti metodi hanno TODO/error handling minimo; alcuni seed chiamano invio email hardcoded.
- `MediaService` scrive su filesystem (`wwwroot/media`) usando `WEBROOT` env; assicurarsi di impostarlo o chiamare `OpenCRMEnv.SetWebRoot()` (già fatto in `AddOpenCRM`).
- Le chiavi RSA degli utenti sono salvate come XML in DB (valutare sicurezza/rotazione).
- `UserSessionService` non implementato; cookie custom `OpenCRM.Session` serializza `DataSession` in Base64.
- `TranslationService.GetTranslationValue` usa lingua hardcoded \"En\".
- Test limitati al servizio RSA; nessuna copertura per servizi web/moduli.
