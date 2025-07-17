// Generate PWA icons
function generateIcon(size) {
    const canvas = document.createElement('canvas');
    canvas.width = size;
    canvas.height = size;
    const ctx = canvas.getContext('2d');
    
    // Background
    ctx.fillStyle = '#3B82F6';
    ctx.fillRect(0, 0, size, size);
    
    // White circle
    ctx.fillStyle = 'white';
    ctx.beginPath();
    ctx.arc(size/2, size/2, size*0.35, 0, 2 * Math.PI);
    ctx.fill();
    
    // Simple truck icon
    const scale = size / 512;
    ctx.fillStyle = '#3B82F6';
    
    // Truck body
    ctx.fillRect(150*scale, 220*scale, 120*scale, 80*scale);
    
    // Truck cab
    ctx.fillStyle = '#2563EB';
    ctx.fillRect(270*scale, 240*scale, 60*scale, 60*scale);
    
    // Wheels
    ctx.fillStyle = '#1E40AF';
    ctx.beginPath();
    ctx.arc(190*scale, 320*scale, 20*scale, 0, 2 * Math.PI);
    ctx.fill();
    ctx.beginPath();
    ctx.arc(290*scale, 320*scale, 20*scale, 0, 2 * Math.PI);
    ctx.fill();
    
    // Text
    ctx.fillStyle = 'white';
    ctx.font = `bold ${36*scale}px Arial`;
    ctx.textAlign = 'center';
    ctx.fillText('DP', size/2, size*0.8);
    
    return canvas.toDataURL('image/png');
}

// Generate and download icons
function downloadIcon(size, filename) {
    const dataUrl = generateIcon(size);
    const link = document.createElement('a');
    link.download = filename;
    link.href = dataUrl;
    link.click();
}

// Auto-generate icons when page loads
window.addEventListener('load', () => {
    console.log('Generating PWA icons...');
    
    // Generate 192x192 icon
    const icon192 = generateIcon(192);
    fetch(icon192)
        .then(res => res.blob())
        .then(blob => {
            // This would normally save the file, but we'll use a different approach
            console.log('192x192 icon generated');
        });
    
    // Generate 512x512 icon  
    const icon512 = generateIcon(512);
    fetch(icon512)
        .then(res => res.blob())
        .then(blob => {
            console.log('512x512 icon generated');
        });
});