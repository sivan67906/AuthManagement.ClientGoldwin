(function () {
    try {
        if (navigator.permissions && typeof navigator.permissions.query === 'function') {
            const originalQuery = navigator.permissions.query;
            Object.defineProperty(navigator.permissions, 'query', {
                value: function (...args) {
                    try { return originalQuery.apply(navigator.permissions, args); }
                    catch (err) { console.warn('Patched Permissions.query:', err); return Promise.resolve({ state: 'prompt' }); }
                },
                writable: false, configurable: false
            });
            console.log('✅ Permissions API safely patched (inline)');
        }
    } catch (e) { console.warn('Permissions patch skipped:', e); }
})();