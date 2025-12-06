# Modifiche apportate
- Angular API: la serializzazione dei parametri salta valori null/undefined e la conferma email usa query esplicita.
- Hosting SPA: nuovo middleware per servire `Client/dist` su `/manager`, percorso corretto nel bootstrap web e output Angular fissato.
- Tooling: ignorati gli artefatti di build SPA/publish, script di build ripulisce la cartella publish, README aggiornato ai comandi PowerShell.
- Build fix: rimossi i `Content` impliciti sul dist Angular per evitare duplicati durante `dotnet build`.
