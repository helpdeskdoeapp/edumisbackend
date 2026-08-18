# Deploying EduMIS Backend to Render with Docker

This guide explains how to deploy the **EduMIS Backend** (.NET 10 Web API) to [Render](https://render.com) using Docker containers.

---

## Architecture Overview

- **Runtime**: Docker (`mcr.microsoft.com/dotnet/aspnet:10.0`)
- **Build**: Multi-stage build (`mcr.microsoft.com/dotnet/sdk:10.0`)
- **Port Binding**: ASP.NET Core dynamically binds to the `$PORT` environment variable provided by Render.
- **Reverse Proxy**: `ForwardedHeaders` middleware is enabled for TLS/HTTPS termination and client IP forwarding.
- **Database**: PostgreSQL (Render Managed Database or external instance).
- **API Documentation**: Scalar UI at `/scalar` and Swagger UI at `/swagger`.

---

## Option 1: Deploy with Render Blueprint (Recommended)

Render Blueprints let you provision the Web Service and PostgreSQL database automatically from the repository using [render.yaml](file:///Users/ashishbisht/RiderProjects/edumisbackend/render.yaml).

### Steps:

1. **Push your code to GitHub / GitLab / Bitbucket**:
   ```bash
   git add .
   git commit -m "Add Docker and Render deployment configuration"
   git push origin main
   ```

2. **Open Render Dashboard**:
   - Go to [dashboard.render.com](https://dashboard.render.com).
   - Click **New +** > **Blueprint**.
   - Connect your Git repository containing `edumisbackend`.
   - Render will detect `render.yaml` and show:
     - Web Service: `edumis-backend` (Docker)
     - Database: `edumis-db` (PostgreSQL)

3. **Apply Blueprint**:
   - Click **Apply**.
   - Render will create the PostgreSQL database and build the Docker image for the web service.

4. *(Optional)* **Add Firebase Service Account**:
   - In your Web Service settings on Render, go to **Environment** > **Secret Files**.
   - Add a Secret File named `smc-service-account.json` with file path `/etc/secrets/smc-service-account.json`.
   - Paste the contents of your `smc-service-account.json`.

---

## Option 2: Manual Deployment via Render Dashboard

If you prefer configuring services manually in the UI:

### Step 1: Create a PostgreSQL Database on Render

1. On the Render Dashboard, click **New +** > **PostgreSQL**.
2. Set the details:
   - **Name**: `edumis-db`
   - **Database**: `edumisdb`
   - **User**: `postgres`
   - **Region**: Choose the region closest to your users (e.g., `Oregon`, `Frankfurt`, `Singapore`).
   - **Plan**: Free or Starter.
3. Click **Create Database**.
4. Once created, copy the **Internal Database URL** (for services hosted in the same Render region).

### Step 2: Create the Web Service

1. Click **New +** > **Web Service**.
2. Connect your Git repository.
3. Configure the settings:
   - **Name**: `edumis-backend`
   - **Region**: Same region as your database.
   - **Runtime**: **Docker**
   - **Dockerfile Path**: `./Dockerfile`
   - **Docker Context**: `.`
   - **Instance Type**: Free or Starter.

### Step 3: Configure Environment Variables

Under the **Environment Variables** section of your Web Service, add:

| Key | Value / Example | Description |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Sets production mode |
| `ConnectionStrings__edumisConStr` | `Host=...;Database=edumisdb;Username=postgres;Password=...` | PostgreSQL connection string (or Render Internal DB URL) |
| `JWTAuth__ValidIssuer` | `https://education.delhi.gov.in` | JWT Valid Issuer |
| `JWTAuth__ValidAudience` | `Users` | JWT Valid Audience |
| `JWTAuth__Secret` | *(A strong 32+ character key)* | Secret key for signing JWT tokens |
| `JWTAuth__AccessTokenExpirationMinutes` | `15` | Access token lifetime |
| `JWTAuth__RefreshTokenExpirationDays` | `7` | Refresh token lifetime |
| `Cors__AllowedOrigins__0` | `https://your-frontend.onrender.com` | Your frontend production URL |
| `Cors__AllowedOrigins__1` | `http://localhost:3000` | Local development frontend URL |
| `GOOGLE_APPLICATION_CREDENTIALS` | `/etc/secrets/smc-service-account.json` | Path to Firebase secret file |

### Step 4: Add Secret Files (Firebase Service Account)

1. Under the **Environment** tab, scroll to **Secret Files**.
2. Click **Add Secret File**.
3. **Filename**: `smc-service-account.json`
4. **Contents**: Paste the JSON content of your `smc-service-account.json`.
5. Set `GOOGLE_APPLICATION_CREDENTIALS` env variable to `/etc/secrets/smc-service-account.json`.

### Step 5: Deploy

Click **Create Web Service** (or **Manual Deploy** > **Deploy latest commit**). Render will clone your repository, build the Docker container, and start the service.

---

## Applying Database Migrations

To apply Entity Framework Core migrations to your Render PostgreSQL database:

### Option A: From your local machine
Set the connection string pointing to Render's **External Database URL** and run:
```bash
dotnet ef database update --project edumis.DataAccess --startup-project edumisbackend --connection "<RENDER_EXTERNAL_DB_URL>"
```

### Option B: Automatic Migration on Startup (Optional)
You can call `context.Database.Migrate()` in `Program.cs` during application initialization if automatic schema migration is desired.

---

## Testing Locally with Docker Compose

You can test the entire stack (Backend + PostgreSQL) locally before deploying to Render:

1. **Start all services**:
   ```bash
   docker compose up --build
   ```

2. **Access the application**:
   - Scalar Documentation: [http://localhost:5170/scalar](http://localhost:5170/scalar)
   - Swagger Documentation: [http://localhost:5170/swagger](http://localhost:5170/swagger)
   - PostgreSQL Database: `localhost:5432`

3. **Stop the services**:
   ```bash
   docker compose down
   ```

---

## Health Check and Verification

- **Health Endpoint**: `https://<your-render-subdomain>.onrender.com/swagger/v1/swagger.json`
- **Interactive API Docs**: `https://<your-render-subdomain>.onrender.com/scalar` or `/swagger`
- **Render Logs**: View live container output in the **Logs** tab of the Render dashboard.
