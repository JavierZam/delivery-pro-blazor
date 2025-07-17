// PWA Installation
let deferredPrompt;
let installButton;

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
    // Show manual install instructions
    const modal = document.createElement('div');
    modal.className = 'fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4';
    modal.innerHTML = `
        <div class="bg-white rounded-lg p-6 max-w-sm w-full">
            <h3 class="text-lg font-semibold mb-4">Install DeliveryPro</h3>
            <p class="text-gray-600 mb-4">To install this app:</p>
            <ol class="text-sm text-gray-600 space-y-2 mb-6">
                <li>1. Tap the share button in your browser</li>
                <li>2. Select "Add to Home Screen"</li>
                <li>3. Tap "Add" to install</li>
            </ol>
            <button onclick="this.parentElement.parentElement.remove()" 
                    class="btn btn-primary w-full">Got it!</button>
        </div>
    `;
    document.body.appendChild(modal);
}

// Hide install buttons initially
document.addEventListener('DOMContentLoaded', () => {
    const installButtons = document.querySelectorAll('.pwa-install-btn');
    installButtons.forEach(btn => {
        btn.style.display = 'none';
    });
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