# Linee guida per **OpenCRM.Core**

Queste indicazioni descrivono lo scopo del progetto `Core/OpenCRM.Core` e come strutturare entità, servizi, modelli/DTO ed estensioni. Il progetto fornisce le primitive condivise per tutti i moduli OpenCRM e non deve dipendere da `OpenCRM.Core.Web` o dai moduli applicativi.

## Scopo del progetto
- Esporre il `DataContext` base e le entità condivise (utenti, ruoli, media, lingue, traduzioni, data block, history) riutilizzabili nei moduli.
- Offrire servizi infrastrutturali generici (es. crittografia RSA, QRCode) senza logica di UI o ASP.NET Core.
- Centralizzare modelli e costanti comuni per mantenere uniformi naming e serializzazione.

## Struttura delle cartelle
- `Data/`: entità EF Core e `DataContext` base. Qualsiasi nuova entità comune deve vivere qui ed essere registrata nel `DbSet` corrispondente e nelle convenzioni di `DataContext`.
- `Crypto/`: servizi di crittografia (es. `RSACryptoService`). Nuovi servizi devono essere stateless, thread-safe e configurati per l'iniezione in `OpenCRM.Core.Web`.
- `QRCode/`: servizi per la generazione di QR code riutilizzabili nei moduli.
- `Models/`: enum e modelli di supporto che non dipendono da ASP.NET Core (es. `MediaType`). Evitare di inserire DTO API qui.
- `Extensions/`: helper statici condivisi (es. `OpenCRMEnv`). Limitarsi a utility pure o a wrapper di configurazione ambiente.
- `Core/OpenCRM.Core.Test`: test unitari a copertura dei servizi di base. Aggiungere qui i test dei nuovi servizi di `OpenCRM.Core`.

## Entità e DbContext
- Le entità devono estendere `BaseEntity` quando richiedono timestamp (`AddedAt`, `UpdatedAt`, `DeletedAt`) o audit comune.
- Conservare le entità nel namespace `OpenCRM.Core.Data` e un file per entità dentro `Data/` (es. `LanguageEntity.cs`).
- Per ogni nuova entità:
  - Aggiungere il relativo `DbSet<T>` in `DataContext`.
  - Configurare chiavi, lunghezze e relazioni nel metodo `OnModelCreating` di `DataContext` evitando configurazioni specifiche di moduli.
  - Non usare attributi di data annotation quando la configurazione fluente è sufficiente.
- Non inserire logica di dominio complessa nelle entità: mantenere proprietà e invarianti minime, demandando i comportamenti a servizi nei moduli applicativi.

## Servizi
- I servizi devono essere classi stateless e thread-safe, con naming `*Service` (es. `RSACryptoService`).
- Non dipendere da ASP.NET Core o da provider concreti non configurabili; utilizzare interfacce del framework BCL quando possibile.
- Esporre API asincrone quando si interagisce con I/O o CPU intensivo.
- Documentare i requisiti di configurazione (es. dimensione chiave RSA) direttamente nel file e lasciare l'onere di registrazione al livello `OpenCRM.Core.Web`.

## Modelli e DTO
- Usare `Models/` per tipi condivisi cross-modulo (enum, record semplici, value object serializzabili).
- Evitare di inserire DTO specifici delle API o di front-end; questi vivono nei progetti modulo o in `OpenCRM.Core.Web` se cross-host.
- Mantenere i nomi in PascalCase e preferire tipi immutabili (record) quando non è richiesta la mutabilità.

## Estensioni e helper
- Gli helper in `Extensions/` devono essere minimali e privi di stato globale. Se necessario, leggere configurazioni tramite variabili d'ambiente o parametri espliciti.
- Evitare side-effect automatici nel costruttore statico; fornire metodi espliciti (`SetWebRoot`, `ResolvePath`, ecc.).

## Test e qualità
- Ogni nuovo servizio deve avere test in `Core/OpenCRM.Core.Test` che coprano scenari nominali ed edge case.
- Evitare dipendenze esterne nei test; per operazioni su filesystem usare percorsi temporanei/isolati.
- Mantenere la nomenclatura dei test con pattern `Method_ShouldExpectation_WhenCondition`.

## Checklist di contribuzione rapida
- [ ] Nuova entità creata in `Data/` con `DbSet` e configurazione in `DataContext`.
- [ ] Servizi stateless in `Crypto/` o cartella dedicata, con test unitari.
- [ ] Modelli condivisi in `Models/` senza dipendenze da ASP.NET Core.
- [ ] Estensioni in `Extensions/` senza side-effect impliciti.
- [ ] Aggiornata la documentazione se cambia il comportamento pubblico.
