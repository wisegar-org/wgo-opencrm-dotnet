# Codex Guidelines – Progetto X

Queste linee guida servono per istruire Codex su come generare, modificare o proporre codice per il Progetto X.  
Il progetto consiste nella migrazione di un'applicazione desktop verso un’architettura web basata su ASP.NET Core (API) e una SPA Quasar/TypeScript.

---

## 1. Architettura generale del progetto

Il backend è strutturato in moduli indipendenti che dipendono da due librerie fondamentali:

- **OpenCRM.Core**
  - Contiene il DataContext base di Entity Framework Core.
  - Contiene entità base condivise:
    - Utenti, Gruppi, Ruoli
    - Logs
    - Media/Allegati
    - Linguaggi e Traduzioni
    - Blocchi dati generici (DataBlock)
  - Contiene servizi infrastrutturali:
    - `UserSessionService`
    - `RoleService`
    - `MediaService`
    - `LanguageService` / `TranslationService`
    - `IdentityService`
    - `DataBlockService`

- **OpenCRM.Core.Web**
  - Layer di integrazione per progetti ASP.NET Core.
  - Fornisce setup automatico per:
    - Identity e autorizzazioni
    - Registrazione dei servizi base
    - Inizializzazione del DataContext (versione base o estesa)
    - Middleware comuni

Tutti i moduli applicativi backend dipendono da queste librerie e mai viceversa.

---

## 2. Struttura del progetto

Il progetto segue una divisione in livelli chiari:

OpenCRM.Core
OpenCRM.Core.Web

ProgettoX.Domain // Entità di dominio e regole
ProgettoX.Application // Use case, servizi applicativi, orchestrazione
ProgettoX.Infrastructure // DbContext esteso, repository, integrazioni
ProgettoX.Api // Controller API, DTO, Startup/Program


### Regole Codex:

- **Il dominio non può dipendere da Application o Infrastructure.**
- **Application può dipendere da Domain e OpenCRM.Core.**
- **Infrastructure può dipendere da Domain, Application e OpenCRM.Core/Web.**
- **Api può dipendere da tutti gli altri livelli.**

Codex deve rispettare sempre la separazione dei livelli e non creare dipendenze circolari.

---

## 3. Regole per generare codice backend (ASP.NET Core)

### DbContext
- Codex deve generare entità e configurazioni EF Core nel progetto `ProgettoX.Infrastructure`.
- Le entità devono estendere quelle di `OpenCRM.Core` solo quando necessario.
- Il `DbContext` del progetto deve estendere il DbContext di `OpenCRM.Core`.

### Controller
- Devono vivere in `ProgettoX.Api`.
- Devono esporre solo DTO e mai entità EF direttamente.
- Devono dipendere da servizi presenti in Application.

### Servizi applicativi
- Vanno nel progetto `ProgettoX.Application`.
- Devono usare i servizi base di `OpenCRM.Core` quando disponibili.
- Devono essere sempre registrati tramite metodo DI dedicato (es. `services.AddApplicationServices()`).

### Utilizzo dei servizi Core
Codex deve preferire sempre:
- `IdentityService` per autenticazione/autorizzazione
- `UserSessionService` per recuperare l’utente corrente
- `MediaService` per upload e gestione file
- `LanguageService` e `TranslationService` per i18n
- `DataBlockService` per dati generici spesso cifrati

Non scrivere logiche duplicate di questi componenti.

---

## 4. Regole per generare codice frontend (SPA Quasar + TypeScript)

Struttura dei moduli:

/src/modules/<modulo>/
pages/
components/
services/<modulo>.api.ts
store/<modulo>.store.ts


Regole principali:

- Tutte le chiamate HTTP devono passare da un `httpClient` centralizzato.
- I file `*.api.ts` devono contenere funzioni che chiamano endpoint REST.
- Lo state management deve essere implementato con Pinia.
- Le traduzioni devono essere caricate dal backend tramite il TranslationService.
- Gli upload devono utilizzare FormData e passare tramite gli endpoint gestiti da MediaService.

---

## 5. Convenzioni di stile

### Backend (C#)
- Naming PascalCase per classi, metodi pubblici, proprietà.
- I servizi devono avere nome `<Something>Service`.
- Gli handler devono essere asincroni (`async/await`).

### Frontend (TypeScript)
- Naming camelCase per variabili e funzioni.
- Tipi e interfacce in PascalCase.
- Nessuna logica di business nel componente: deve stare nei service o store.

---

## 6. Istruzioni generali per Codex

Quando generi codice o suggerisci modifiche, devi:

1. **Rispettare la struttura architetturale stabilita.**
2. **Usare sempre i servizi e le entità del Core quando disponibili.**
3. **Non duplicare funzioni già esistenti in OpenCRM.Core.**
4. **Separare chiaramente dominio, applicazione, infrastruttura e API.**
5. **Produrre codice tipico di una soluzione enterprise, pulito, documentato e consistente.**
6. **Per il frontend generare solo codice Quasar/TypeScript coerente con la struttura modules.**
7. **Per ogni nuovo modulo, creare cartelle simmetriche lato backend e frontend.**

---

## 7. Output atteso da Codex

Quando Codex risponde deve produrre:

- codice coerente con l’architettura
- suggerimenti specifici su dove posizionare file e classi
- eventuali snippet di integrazione con OpenCRM.Core/Web
- una breve spiegazione delle scelte architetturali

---

## 8. Cosa Codex non deve fare

- Non deve introdurre librerie non richieste.
- Non deve ignorare i servizi base di OpenCRM.Core.
- Non deve accoppiare direttamente API → Infrastructure bypassando Application.
- Non deve generare file in percorsi non previsti dalla struttura.

---

## 9. Obiettivo finale

Le presenti linee guida servono a garantire:

- una migrazione ordinata del vecchio sistema desktop,
- una codebase pulita e organizzata,
- la massima coerenza tra tutti i nuovi moduli,
- un'architettura flessibile e futura e mantenibile a lungo termine.

Codex deve lavorare sempre rispettando questi principi.
