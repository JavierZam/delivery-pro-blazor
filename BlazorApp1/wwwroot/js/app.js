// PWA Installation
let deferredPrompt;
let installButton;

// Check if app is already installed
function isAppInstalled() {
    return window.matchMedia('(display-mode: standalone)').matches ||
           window.navigator.standalone === true ||
           document.referrer.includes('android-app://');
}

// Check if device supports PWA installation
function supportsPWAInstall() {
    return 'serviceWorker' in navigator && 
           ('BeforeInstallPromptEvent' in window || 
            /iPad|iPhone|iPod/.test(navigator.userAgent) ||
            /Android/.test(navigator.userAgent));
}

window.addEventListener('beforeinstallprompt', (e) => {
    console.log('beforeinstallprompt fired');
    // Prevent Chrome 67 and earlier from automatically showing the prompt
    e.preventDefault();
    // Stash the event so it can be triggered later
    deferredPrompt = e;
    // Show install button
    showInstallButton();
});

function showInstallButton() {
    // Don't show if already installed
    if (isAppInstalled()) {
        return;
    }
    
    // Show install buttons
    const installButtons = document.querySelectorAll('.pwa-install-btn');
    installButtons.forEach(btn => {
        btn.style.display = 'inline-flex';
        btn.addEventListener('click', installPWA);
    });
}

function installPWA() {
    if (deferredPrompt) {
        // Show the prompt
        deferredPrompt.prompt();
        // Wait for the user to respond to the prompt
        deferredPrompt.userChoice.then((choiceResult) => {
            if (choiceResult.outcome === 'accepted') {
                console.log('User accepted the A2HS prompt');
                showInstallSuccess();
            } else {
                console.log('User dismissed the A2HS prompt');
            }
            deferredPrompt = null;
        });
    } else {
        // Fallback for browsers that don't support PWA installation
        showInstallInstructions();
    }
}

function showInstallSuccess() {
    // Show success message
    const toast = document.createElement('div');
    toast.className = 'fixed bottom-4 right-4 bg-green-500 text-white px-6 py-3 rounded-lg shadow-lg z-50 animate-slide-up';
    toast.innerHTML = '✅ DeliveryPro installed successfully!';
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.remove();
    }, 3000);
}

function showInstallInstructions() {
    // Show manual install instructions based on device
    const modal = document.createElement('div');
    modal.className = 'fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4';
    
    let instructions = '';
    if (/iPad|iPhone|iPod/.test(navigator.userAgent)) {
        instructions = `
            <li>1. Tap the share button <svg class="inline w-4 h-4" fill="currentColor" viewBox="0 0 20 20"><path d="M15 8a3 3 0 10-2.977-2.63l-4.94 2.47a3 3 0 100 4.319l4.94 2.47a3 3 0 10.895-1.789l-4.94-2.47a3.027 3.027 0 000-.74l4.94-2.47C13.456 7.68 14.19 8 15 8z"/></svg> in Safari</li>
            <li>2. Scroll down and select "Add to Home Screen"</li>
            <li>3. Tap "Add" to install DeliveryPro</li>
        `;
    } else if (/Android/.test(navigator.userAgent)) {
        instructions = `
            <li>1. Tap the menu button (⋮) in Chrome</li>
            <li>2. Select "Add to Home Screen" or "Install app"</li>
            <li>3. Tap "Add" to install DeliveryPro</li>
        `;
    } else {
        instructions = `
            <li>1. Look for the install icon in your browser's address bar</li>
            <li>2. Click it and select "Install"</li>
            <li>3. Or use browser menu → "Install DeliveryPro"</li>
        `;
    }
    
    modal.innerHTML = `
        <div class="bg-white rounded-lg p-6 max-w-sm w-full">
            <h3 class="text-lg font-semibold mb-4">Install DeliveryPro</h3>
            <p class="text-gray-600 mb-4">To install this app:</p>
            <ol class="text-sm text-gray-600 space-y-2 mb-6">
                ${instructions}
            </ol>
            <button onclick="this.parentElement.parentElement.remove()" 
                    class="btn btn-primary w-full">Got it!</button>
        </div>
    `;
    document.body.appendChild(modal);
}

// Initialize PWA install button on page load
document.addEventListener('DOMContentLoaded', () => {
    const installButtons = document.querySelectorAll('.pwa-install-btn');
    
    // Initially hide buttons
    installButtons.forEach(btn => {
        btn.style.display = 'none';
    });
    
    // Check if we should show install button (especially for mobile)
    setTimeout(() => {
        if (supportsPWAInstall() && !isAppInstalled()) {
            // Show button for mobile devices even without beforeinstallprompt
            if (/iPad|iPhone|iPod|Android/.test(navigator.userAgent)) {
                showInstallButton();
            }
        }
    }, 1000); // Small delay to ensure page is fully loaded
});

// Service Worker Registration with update notification
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('service-worker.js')
        .then((registration) => {
            console.log('SW registered: ', registration);
            
            // Check for updates
            registration.addEventListener('updatefound', () => {
                const newWorker = registration.installing;
                newWorker.addEventListener('statechange', () => {
                    if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                        showUpdateNotification();
                    }
                });
            });
        })
        .catch((registrationError) => {
            console.log('SW registration failed: ', registrationError);
        });
}

function showUpdateNotification() {
    const toast = document.createElement('div');
    toast.className = 'fixed bottom-4 left-4 bg-blue-500 text-white px-6 py-3 rounded-lg shadow-lg z-50 animate-slide-up';
    toast.innerHTML = `
        <div class="flex items-center space-x-3">
            <span>🔄 App updated!</span>
            <button onclick="window.location.reload()" class="bg-white text-blue-500 px-3 py-1 rounded text-sm font-medium">
                Refresh
            </button>
        </div>
    `;
    document.body.appendChild(toast);
}