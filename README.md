# CVParser

## Database

EF Core migrations live in `CVParser.Infrastructure.Database/Migrations`. `dotnet-ef` is pinned as a local tool in `dotnet-tools.json` (run `dotnet tool restore` on a fresh clone).

Run these commands from the solution root:

### Add a migration
```powershell
dotnet ef migrations add <MigrationName> --project CVParser.Infrastructure.Database --output-dir Migrations
```

### Undo the last unapplied migration
```powershell
dotnet ef migrations remove --project CVParser.Infrastructure.Database
```

### Check for unmigrated model changes
```powershell
dotnet ef migrations has-pending-model-changes --project CVParser.Infrastructure.Database
```

### Generate an idempotent SQL script (production)
```powershell
dotnet ef migrations script --idempotent --project CVParser.Infrastructure.Database --output migrate.sql
```
