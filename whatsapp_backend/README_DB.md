# Database Setup (EF Core)

This backend uses Entity Framework Core with:
- SQLite by default (file at Data/app.db inside the app directory)
- Optional PostgreSQL when `DATABASE_PROVIDER=Postgres` and `DATABASE_URL` is defined.

Environment variables (set via the deployment orchestrator, do not hard-code):
- DATABASE_PROVIDER: "SQLite" (default) or "Postgres"
- DATABASE_URL: required when DATABASE_PROVIDER=Postgres (e.g., Host=...;Database=...;Username=...;Password=...)

At startup the app will:
- Apply migrations (Database.Migrate)
- Seed minimal dev data in Development environment

To add new migrations locally (optional):
- dotnet ef migrations add SomeChange --project whatsapp_backend
- dotnet ef database update --project whatsapp_backend
