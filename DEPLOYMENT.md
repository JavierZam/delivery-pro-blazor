# 🚀 DeliveryPro Blazor WASM - Cloud Run Deployment

## 📋 Prerequisites

### 1. GitHub Secrets Required
Configure these secrets in your GitHub repository:

#### Environment: `adviz-dev-gcp`
- `GCP_SA_KEY`: Service Account JSON key for GCP authentication
- `GCP_PROJECT_ID`: Your GCP Project ID

#### Environment: `adviz-prod-gcp` 
- `GCP_SA_KEY`: Service Account JSON key for GCP authentication
- `GCP_PROJECT_ID`: Your GCP Project ID

### 2. GCP Service Account Permissions
Your service account needs these IAM roles:
- `Cloud Run Admin`
- `Artifact Registry Administrator`
- `Storage Admin`
- `Service Account User`

### 3. GCP Services Required
Enable these services in your GCP project:
```bash
gcloud services enable run.googleapis.com
gcloud services enable artifactregistry.googleapis.com
gcloud services enable cloudbuild.googleapis.com
```

## 🏗️ Architecture

### Development Environment
- **Service Name**: `blazorapp-dev`
- **Branch**: `dev`
- **Memory**: 512Mi
- **CPU**: 1
- **Min Instances**: 0
- **Max Instances**: 10

### Production Environment
- **Service Name**: `blazorapp-prod`
- **Branch**: `main`
- **Memory**: 1Gi
- **CPU**: 1
- **Min Instances**: 1
- **Max Instances**: 20

## 🚀 Deployment Process

### Automatic Deployment
1. **Development**: Push to `dev` branch triggers dev deployment
2. **Production**: Push to `main` branch triggers prod deployment

### Manual Deployment
Use GitHub Actions "Run workflow" button for manual deployments.

## 🔍 Monitoring

### Health Checks
- **Main App**: `https://your-service-url/`
- **Health Endpoint**: `https://your-service-url/health`

### Logs
```bash
# Development logs
gcloud logs read "resource.type=cloud_run_revision AND resource.labels.service_name=blazorapp-dev" --limit 50

# Production logs
gcloud logs read "resource.type=cloud_run_revision AND resource.labels.service_name=blazorapp-prod" --limit 50
```

## 🛠️ Local Development

### Build Docker Image Locally
```bash
# Build
docker build -t blazorapp-local .

# Run
docker run -p 8080:8080 blazorapp-local

# Test
curl http://localhost:8080/health
```

### Debug Deployment Issues
```bash
# Get service details
gcloud run services describe blazorapp-dev --region=asia-southeast2

# View recent deployments
gcloud run revisions list --service=blazorapp-dev --region=asia-southeast2
```

## 📦 Build Optimizations

The project includes these production optimizations:
- **Trimming**: Reduces bundle size by removing unused code
- **Compression**: Brotli/Gzip compression for static assets
- **Caching**: Aggressive caching for static resources
- **WASM Optimization**: Optimized WebAssembly loading

## 🔒 Security Features

- **HTTPS Only**: Cloud Run enforces HTTPS
- **Security Headers**: Added via nginx configuration
- **No Authentication Required**: Public access for demo
- **CORS**: Configured for web assembly requirements

## 🌐 URLs

After deployment, your applications will be available at:
- **Development**: `https://blazorapp-dev-[hash]-ue.a.run.app`
- **Production**: `https://blazorapp-prod-[hash]-ue.a.run.app`

## 🚨 Troubleshooting

### Common Issues

1. **Build Fails**
   - Check .NET 8.0 SDK compatibility
   - Verify all NuGet packages restore correctly

2. **Container Won't Start**
   - Check nginx configuration
   - Verify port 8080 is exposed

3. **Health Check Fails**
   - Ensure `/health` endpoint returns 200
   - Check container startup time

4. **Authentication Issues**
   - Verify GCP_SA_KEY secret is valid JSON
   - Check service account permissions

### Debug Commands
```bash
# Test container locally
docker run --rm -p 8080:8080 blazorapp-local

# Check Cloud Run logs
gcloud run logs read blazorapp-dev --region=asia-southeast2

# Describe service
gcloud run services describe blazorapp-dev --region=asia-southeast2
```