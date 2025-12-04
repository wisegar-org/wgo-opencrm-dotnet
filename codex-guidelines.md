# Linee guida per Copilot / Codex

## Architettura generale
- Pattern: Clean Architecture (Domain, Application, Infrastructure, Presentation)
- Linguaggio: C#, Typescript
- Repo: progetto modulare, indipendenza dalle librerie esterne.

## Regole che Copilot deve seguire
1. Nessuna dipendenza dal livello Infrastructure dentro Domain o Application.
2. Gli entity devono essere anemici ma con metodi di dominio.
3. Gli adapter di persistence devono vivere in `/Infrastructure/Persistence`.
4. Le interfacce devono essere definite nel livello Application.
5. Ogni nuovo file deve seguire la naming convention: `{Nome}Service.cs`, `{Nome}Repository.cs`,`{Nome}Entity.cs`.

## Esempio di struttura di cartelle
- /Code
- /Code/{Nome}
- /Modules
- /Modules/{Nome}

## Linee guida specifiche per questo repo
- Progetto web: `Modules/OpenCRM.Manager` (csproj `OpenCRM.Manager.csproj`). Aggiorna sempre percorsi relativi se sposti file nel modulo.
- SPA Quasar: `Modules/OpenCRM.Manager/ui`. Usa `npm run dev` in sviluppo; evita di committare `node_modules`.
- Script di avvio: `run-all.ps1` lancia backend e SPA. Se modifichi porte o argomenti, aggiorna README e lo script insieme.
- Docker: `Modules/OpenCRM.Manager/Dockerfile` copia anche i progetti core/moduli. Mantieni percorsi coerenti con la soluzione.
- Config: `appsettings*.json` e `launchSettings.json` sono versionati nel modulo; se aggiungi nuove chiavi documentale in README/Docs.

## Principi Clean Architecture operativi
- Boundary netti: Domain/Application non dipendono da ASP.NET, EF o UI; controller e UI traducono solo richieste/risposte verso i use case.
- Use case espliciti: ogni operazione è un handler/comando in Application; dipende da porte (interfacce) definite lì e implementate in Infrastructure.
- Modellazione dominio: entità con invarianti/metodi di dominio; non esporre entità EF verso API/UI, usa DTO/ViewModel dedicati.
- Dipendenze invertite: repository/adapter in Infrastructure implementano interfacce in Application; registra le implementazioni via DI in Program.cs.
- Separazione UI: la SPA usa solo API HTTP con DTO stabili/versionati; nessun riferimento a EF o servizi interni.
- Config/IO: accessi a file/email/storage/API terze incapsulati in servizi Infrastructure; inietta via interfacce e opzioni tipizzate validate in startup.
- Testabilità: handler/use case senza I/O diretto, dipendenze mockabili; copri i confini (EF/HTTP) con test di integrazione.
- Modularità: moduli (Finance, SwissLPD, Manager) dipendono da Core/Application, non viceversa; accoppiamenti espliciti via interfacce/extension method di configurazione.
- Migrazioni/seed: tieni migrazioni EF nel progetto host/infrastructure; seed minimale nei bootstrap, non nel dominio.
- Logging/telemetria: log strutturato nei servizi applicativi/infrastructure, non nei domain model.
