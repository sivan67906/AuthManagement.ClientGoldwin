
window.loadLayoutScripts = async function () {
    const scripts = [
        "assets/vendor/js/helpers.js",
        "assets/js/template-customizer.js"
    ];

    for (const src of scripts) {
        if (!document.querySelector(`script[src='${src}']`)) {
            const script = document.createElement("script");
            script.src = src;
            script.defer = true;
            document.body.appendChild(script);
            await new Promise(r => (script.onload = r));
        }
    }

    console.log("✅ Layout scripts loaded");
};
window.setHtmlClass = function (className) {
    const html = document.documentElement;
    if (!html.classList.contains(className)) {
        html.classList.add(className);
    }
    console.log("✅ HTML class added:", className);
};


window.removeHtmlClass = function (className) {
    document.documentElement.classList.remove(className);
};
