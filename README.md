# DeliveryPro - Order Delivery System (Prototype)

A modern Blazor WebAssembly application for food delivery with offline capabilities, PWA support, and complete shopping cart functionality.

## 🚀 Features

- **Modern UI**: Clean, responsive design with Tailwind CSS
- **PWA Ready**: Installable as mobile app with offline support
- **Shopping Cart**: Full cart functionality with localStorage persistence
- **Real-time Tracking**: Order status tracking with visual progress
- **Mobile Responsive**: Optimized for mobile with hamburger navigation
- **Clean Architecture**: Separation of concerns with Domain/Application/Infrastructure layers

## 📱 Pages

- **Home** (`/`) - Landing page with hero section and features
- **Menu** (`/menu`) - Product catalog with category filtering
- **Orders** (`/orders`) - Order history with status tracking
- **Track** (`/track`) - Real-time order tracking

## 🛠 Tech Stack

- **Frontend**: Blazor WebAssembly 8.0
- **Styling**: Tailwind CSS
- **Storage**: LocalStorage (Blazored.LocalStorage)
- **Architecture**: Clean Architecture pattern

## 🎯 Demo Data

The application includes hardcoded sample data for:
- 18+ food products across 6 categories
- Sample orders with different statuses
- Mock delivery tracking
- Shopping cart with persistence across sessions

## 🏃‍♂️ Quick Start

```bash
cd BlazorApp1
dotnet restore
dotnet run
```

Navigate to `https://localhost:5001` to view the application.

## 📦 PWA Installation

1. Open the app in a browser
2. Click the "Install" button in the address bar
3. Enjoy the native app experience!

## 🔧 Project Structure

```
BlazorApp1/
├── Components/           # Blazor components and pages
├── Domain/              # Domain entities and enums
├── Application/         # Business logic and services
├── Infrastructure/      # Data storage services
└── wwwroot/            # Static assets and PWA files
```

## 🎨 Design System

- **Primary Color**: Blue (#3B82F6)
- **Typography**: System fonts with modern styling
- **Components**: Cards, buttons, forms with consistent styling
- **Animations**: Smooth transitions and micro-interactions

---

Ready for client demonstration and further development! 🚀