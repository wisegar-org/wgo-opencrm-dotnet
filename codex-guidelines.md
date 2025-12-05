# Codex Guidelines - OpenCRM

Queste linee guida istruiscono Codex su come operare nel repository OpenCRM. La struttura di cartelle e file descritta di seguito deve rimanere invariata salvo esplicita autorizzazione.

---

## 1. Struttura del repository (immutabile)
- Radice: OpenCRM.sln, run-manager.ps1, README.md, codex-guidelines.md, .dockerignore, .gitignore.
- .github/workflows/: development_opencrm-web.yml, opencrm-web.yml.
- Docs/: OpenCRM.md, ProjectOverview.md, Files/OpenCRM_Architecture_Diagram.png.
- Core/
  - OpenCRM.Core/: Entities/, Extensions/, Models/, Services/, bin/, obj/, OpenCRM.Core.csproj.
  - OpenCRM.Core.Web/: Areas/, Client/, Components/, Extensions/, Models/, Pages/, Services/, wwwroot/, bin/, obj/, ExampleJsInterop.cs, Startup.cs, OpenCRM.Core.Web.csproj. Contiene inoltre un progetto spa vue  nella cartella client. Questo quando compila deve sempre finire dentro la cartella ui che deve esserre servita come file statics da un middleware nel indirizzo "/ui". Devi configurare la spa e le api di conseguenza.
  - OpenCRM.Core.Test/: Services/, bin/, obj/, GlobalUsings.cs, OpenCRM.Core.Test.csproj.
- Modules/
  - OpenCRM.Manager/: Controllers/, Data/, DTO/, Pages/, Resources/, wwwroot/, Properties/, bin/, obj/, OpenCRM.Manager.csproj, Program.cs, appsettings.json, appsettings.Development.json, Dockerfile.
    - Client/ (SPA Quasar): src/, public/, .quasar/, node_modules/, quasar.config.ts, eslint.config.js, package.json, package-lock.json, tsconfig.json, postcss.config.js, .prettierrc.json, .npmrc, index.html.
  - OpenCRM.Finance/: Areas/, Services/, bin/, obj/, Startup.cs, OpenCRM.Finance.csproj.
  - OpenCRM.SwissLPD/: Areas/, Services/, bin/, obj/, Startup.cs, OpenCRM.SwissLPD.csproj.

Mantieni sempre questa struttura: non spostare, rinominare o eliminare cartelle e file senza esplicita approvazione.

---

## 2. Architettura e dipendenze
- Librerie core: OpenCRM.Core per entita, servizi di base e modelli condivisi; OpenCRM.Core.Web per integrazione ASP.NET Core (startup, middleware, client shared).
- Moduli backend: OpenCRM.Manager, OpenCRM.Finance, OpenCRM.SwissLPD dipendono da Core/Core.Web e non il contrario.
- SPA: vive in Modules/OpenCRM.Manager/Client e consuma le API esposte dai moduli backend.
- Ogni progetto Angular deve utilizzare la libreria di componenti Flowbite per l'UI; assicurati che sia installata e importata in ciascun progetto.
- Nessuna dipendenza circolare; i moduli non devono referenziare altri moduli se non tramite API.

---

## 3. Regole per generare codice backend (ASP.NET Core)
- DbContext: quando serve estendere, partire dal DbContext base di OpenCRM.Core.
- Controller: collocarli nel modulo backend corretto (es. Modules/OpenCRM.Manager/Controllers); esporre solo DTO e non entita EF.
- Servizi applicativi: nei rispettivi moduli (es. Modules/OpenCRM.Manager/Data o Services) e registrati in DI con metodi dedicati.
- Riutilizzare sempre i servizi Core disponibili (es. IdentityService, UserSessionService, MediaService, LanguageService, TranslationService, DataBlockService) invece di duplicare logiche.

---

## 4. Regole per generare codice frontend (SPA Quasar + TypeScript)
- Ogni progetto Asp Net Core e il progetto OpenCRM.Core.Web ha in progetto spa dentro una cartella Client. Ogni uno di questi progetti deve servire il compilato del progetto spa come StaticsFiles
- Organizzare nuove feature dentro src usando pages/components/services/store coerenti con Axios, Pinia.
- Tutte le chiamate HTTP passano da un httpClient centralizzato.
- File *.api.ts per le chiamate REST; logica di business nei file *.service.ts dentro la cartella services e file *.store.ts per gli store dentro la cartella store. I componenti come file *.component.ts e *.component.vue dentro la cartella components con la struttura {name}.

---

## 5. Convenzioni di stile
- Backend C#: PascalCase per classi/metodi/proprieta pubbliche; servizi con suffisso Service; handler async/await.
- Frontend TS: camelCase per variabili/funzioni; tipi/interfacce in PascalCase; componenti senza logica di business.

---

## 6. Istruzioni generali per Codex
- Rispettare sempre la struttura immutabile del repository.
- Usare i servizi ed entita di Core quando esistono.
- Non duplicare funzioni gia presenti in OpenCRM.Core/Core.Web.
- Separare dominio, applicazione, infrastruttura e API nei moduli backend; evitare dipendenze circolari.
- Per il frontend generare solo codice Quasar/TypeScript coerente con la struttura esistente.
- Per nuovi moduli, creare cartelle simmetriche lato backend e frontend (se previsto) seguendo i nomi gia in uso.

---

## 7. Output atteso da Codex
- Codice coerente con l'architettura e la struttura fissa.
- Indicazioni precise su dove posizionare file e classi.
- Snippet di integrazione con OpenCRM.Core/Core.Web quando necessari.
- Breve spiegazione delle scelte architetturali.

---

## 8. Cosa Codex non deve fare
- Non introdurre librerie non richieste.
- Non ignorare i servizi base di OpenCRM.Core/Core.Web.
- Non accoppiare direttamente API e Infrastructure bypassando Application nei moduli.
- Non generare file fuori dai percorsi previsti o rompere la struttura dichiarata.

---

## 9. Obiettivo finale
Garantire una base OpenCRM ordinata, coerente e mantenibile, facilitando la migrazione e l'evoluzione dei moduli senza rompere la struttura del repository.
