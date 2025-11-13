# PostgreSQL Password Setup Guide

## Change PostgreSQL Password (Windows)

### Method 1: Using pgAdmin (GUI)
1. Open pgAdmin (installed with PostgreSQL)
2. Connect to PostgreSQL server
3. Right-click on "postgres" user
4. Select "Properties"
5. Go to "Definition" tab
6. Enter new password: `Library2025!`
7. Click "Save"

### Method 2: Using psql Command Line
```powershell
# Open PowerShell as Administrator
# Navigate to PostgreSQL bin folder
cd "C:\Program Files\PostgreSQL\16\bin"

# Connect to PostgreSQL
.\psql -U postgres

# At the psql prompt, run:
ALTER USER postgres PASSWORD 'Library2025!';

# Exit
\q
```

### Method 3: Using SQL Shell
1. Open "SQL Shell (psql)" from Start Menu
2. Press Enter for all prompts (uses defaults)
3. Enter current password
4. Run this command:
```sql
ALTER USER postgres PASSWORD 'Library2025!';
```
5. Type `\q` to exit

## Update Your Connection String

After setting the password, update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=SmartLibraryDB;Username=postgres;Password=Library2025!"
  }
}
```

## Security Note for Development
For development, you can use a simple password like:
- `Library2025!`
- `admin123`
- `postgres`

For production, use a strong password with:
- At least 12 characters
- Mix of uppercase, lowercase, numbers, symbols
