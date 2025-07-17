# Multi-stage build for Blazor WebAssembly application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /src

# Copy project file
COPY BlazorApp1/BlazorApp1.csproj BlazorApp1/

# Restore dependencies
RUN dotnet restore BlazorApp1/BlazorApp1.csproj

# Copy source code
COPY . .

# Build and publish the application
WORKDIR /src/BlazorApp1
RUN dotnet build BlazorApp1.csproj -c Release -o /app/build
RUN dotnet publish BlazorApp1.csproj -c Release -o /app/publish

# Production stage with nginx
FROM nginx:alpine

# Copy published output to nginx
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port
EXPOSE 8080

# Start nginx
CMD ["nginx", "-g", "daemon off;"]