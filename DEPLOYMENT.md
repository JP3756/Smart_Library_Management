# 🚀 Deployment Guide - Smart Library Management System

This guide covers deploying the frontend to Vercel and preparing for backend integration.

## 📦 Pre-Deployment Checklist

- [ ] All code is committed to Git
- [ ] `.env` file is added to `.gitignore`
- [ ] Environment variables are documented in `.env.example`
- [ ] Application runs successfully locally (`npm run dev`)
- [ ] Production build works (`npm run build`)
- [ ] All routes are tested

## 🌐 Deploy Frontend to Vercel

### Method 1: GitHub Integration (Recommended)

1. **Push your code to GitHub**
   ```bash
   git init
   git add .
   git commit -m "Initial commit: Smart Library Frontend"
   git branch -M main
   git remote add origin https://github.com/YOUR_USERNAME/smart-library-frontend.git
   git push -u origin main
   ```

2. **Deploy to Vercel**
   - Visit [vercel.com](https://vercel.com) and sign in with GitHub
   - Click "Add New Project"
   - Import your GitHub repository
   - Configure build settings:
     - **Framework Preset**: Vite
     - **Build Command**: `npm run build`
     - **Output Directory**: `dist`
     - **Install Command**: `npm install`

3. **Add Environment Variables**
   - In Vercel Dashboard → Settings → Environment Variables
   - Add: `VITE_API_URL` = `http://localhost:5000/api` (update this later with your live backend URL)

4. **Deploy**
   - Click "Deploy"
   - Wait for deployment to complete
   - Your app will be live at `https://your-project-name.vercel.app`

### Method 2: Vercel CLI

```bash
# Install Vercel CLI globally
npm i -g vercel

# Login to Vercel
vercel login

# Deploy
vercel

# Follow the prompts:
# - Set up and deploy? Yes
# - Which scope? Select your account
# - Link to existing project? No
# - What's your project's name? smart-library-frontend
# - In which directory is your code located? ./
# - Want to override the settings? No

# Deploy to production
vercel --prod
```

## 🔧 Backend API Deployment

### Option 1: Deploy to Render

1. **Create account at render.com**
2. **Create New Web Service**
   - Connect your ASP.NET Core backend repository
   - Environment: Docker
   - Build Command: `dotnet build`
   - Start Command: `dotnet run`

3. **Add Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ConnectionStrings__DefaultConnection=YOUR_MYSQL_CONNECTION_STRING
   JWT__SecretKey=YOUR_SECRET_KEY
   AllowedOrigins=https://your-frontend-on-vercel.app
   ```

4. **Configure CORS in ASP.NET**
   ```csharp
   // In Program.cs or Startup.cs
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowVercel",
           builder => builder
               .WithOrigins("https://your-project-name.vercel.app")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
   });

   app.UseCors("AllowVercel");
   ```

### Option 2: Deploy to Azure App Service

```bash
# Install Azure CLI
az login

# Create resource group
az group create --name smart-library-rg --location eastus

# Create App Service plan
az appservice plan create --name smart-library-plan --resource-group smart-library-rg --sku B1

# Create web app
az webapp create --name smart-library-api --resource-group smart-library-rg --plan smart-library-plan --runtime "DOTNETCORE|8.0"

# Deploy
dotnet publish -c Release
az webapp deployment source config-zip --resource-group smart-library-rg --name smart-library-api --src ./bin/Release/net8.0/publish/app.zip
```

## 🗄️ Database Setup

### Option 1: PlanetScale (MySQL)

1. Create account at [planetscale.com](https://planetscale.com)
2. Create new database
3. Get connection string
4. Add to backend environment variables

### Option 2: Azure MySQL

```bash
az mysql flexible-server create \
  --resource-group smart-library-rg \
  --name smart-library-db \
  --location eastus \
  --admin-user mysqladmin \
  --admin-password YOUR_PASSWORD \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --version 8.0.21
```

## 🔗 Connecting Frontend to Backend

Once your backend is deployed:

1. **Update Vercel Environment Variable**
   - Go to Vercel Dashboard → Settings → Environment Variables
   - Update `VITE_API_URL` to your deployed backend URL
   - Example: `https://smart-library-api.onrender.com/api`
   - Redeploy: `vercel --prod`

2. **Update API Calls** (if needed)
   - In `src/services/api.js`, replace mock functions with real API calls
   - The Axios instance is already configured to use `VITE_API_URL`

3. **Test the Integration**
   - Visit your Vercel URL
   - Open DevTools → Network tab
   - Perform actions (login, view books, etc.)
   - Verify API calls are going to your backend

## 📊 Post-Deployment Testing

### Frontend Testing
- [ ] Login/authentication works
- [ ] All pages load correctly
- [ ] Navigation functions properly
- [ ] Dark/light theme toggle works
- [ ] Forms submit correctly
- [ ] Toast notifications appear
- [ ] Responsive design on mobile/tablet

### Backend Integration Testing
- [ ] API calls return data
- [ ] CORS is configured correctly
- [ ] JWT authentication works
- [ ] CRUD operations function
- [ ] Error handling works
- [ ] Database queries execute properly

## 🔄 Continuous Deployment

### Automatic Deployments with Vercel + GitHub

Once connected to GitHub, Vercel automatically:
- Deploys on every push to `main` branch
- Creates preview deployments for pull requests
- Runs build checks before deploying

### Update Workflow

```bash
# Make changes
git add .
git commit -m "Add new feature"
git push origin main

# Vercel automatically deploys
# Check deployment status in Vercel Dashboard
```

## 🛡️ Security Best Practices

1. **Environment Variables**
   - Never commit `.env` files
   - Use different values for dev/prod
   - Rotate secrets regularly

2. **API Security**
   - Use HTTPS only
   - Implement rate limiting
   - Validate all inputs
   - Use JWT with expiration

3. **Frontend Security**
   - Store minimal data in localStorage
   - Sanitize user inputs
   - Use CSP headers
   - Keep dependencies updated

## 📈 Monitoring & Analytics

### Vercel Analytics
```bash
npm i @vercel/analytics

# Add to main.jsx
import { Analytics } from '@vercel/analytics/react';

// In your App component
<Analytics />
```

### Error Tracking (Sentry)
```bash
npm i @sentry/react

# Configure in main.jsx
import * as Sentry from "@sentry/react";

Sentry.init({
  dsn: "YOUR_SENTRY_DSN",
  environment: import.meta.env.MODE,
});
```

## 🔧 Troubleshooting

### Build Fails on Vercel
- Check build logs in Vercel Dashboard
- Ensure all dependencies are in `package.json`
- Test build locally: `npm run build`
- Check Node.js version compatibility

### API Calls Fail
- Verify `VITE_API_URL` is set correctly
- Check browser console for CORS errors
- Ensure backend allows your Vercel domain
- Test backend API with Postman/Thunder Client

### Environment Variables Not Working
- Prefix must be `VITE_` for Vite to expose them
- Redeploy after adding/changing variables
- Check if variables are set in Vercel Dashboard

## 📞 Support Resources

- **Vercel Docs**: https://vercel.com/docs
- **Render Docs**: https://render.com/docs
- **Azure Docs**: https://docs.microsoft.com/azure
- **Vite Docs**: https://vitejs.dev

---

## ✅ Deployment Checklist

### Before First Deployment
- [ ] Code is production-ready
- [ ] All tests pass
- [ ] Environment variables configured
- [ ] Git repository created
- [ ] README.md updated

### Frontend Deployment (Vercel)
- [ ] GitHub repository connected
- [ ] Build settings configured
- [ ] Environment variables added
- [ ] Custom domain configured (optional)
- [ ] SSL certificate active

### Backend Deployment
- [ ] API deployed to Render/Azure
- [ ] Database created and connected
- [ ] CORS configured for frontend domain
- [ ] Environment variables set
- [ ] Health check endpoint working

### Post-Deployment
- [ ] All features tested on production
- [ ] Monitoring/analytics setup
- [ ] Error tracking configured
- [ ] Documentation updated
- [ ] Team notified of URLs

---

**Your Smart Library System is now live! 🎉**

Frontend: `https://your-project-name.vercel.app`  
Backend API: `https://your-api-name.onrender.com` or `https://your-api.azurewebsites.net`
