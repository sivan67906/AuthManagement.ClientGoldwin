window.loadedScripts = window.loadedScripts || [];

window.loadScriptAndTrigger = async function (path, forceReload = false) {
    if (Array.isArray(path)) {
        for (const p of path) {
            await window.loadScriptAndTrigger(p, forceReload);
        }
        return;
    }

    // force reload support
    if (!forceReload && window.loadedScripts.includes(path)) {
        console.log(`Skipped reloading ${path}`);
        return;
    }

    if (forceReload) {
        const existing = document.querySelector(`script[src="${path}"]`);
        if (existing) {
            existing.remove();
            window.loadedScripts = window.loadedScripts.filter(p => p !== path);
            console.log(`Reloading ${path}...`);
        }
    }

    await new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = path + '?v=' + Date.now(); // 💥 cache-busting
        script.onload = () => {
            console.log(`${path} loaded ✅`);
            if (!window.loadedScripts.includes(path)) {
                window.loadedScripts.push(path);
            }
            resolve();
        };
        script.onerror = reject;
        document.body.appendChild(script);
    });

    // Trigger DataTable-ready event
    document.dispatchEvent(new Event('DOMContentLoaded'));
};


