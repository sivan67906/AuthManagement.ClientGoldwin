window.loadCalendarScripts = async function () {
    const scripts = [
        "assets/vendor/js/formValidation.min.js",
        "assets/vendor/libs/formvalidation/bootstrap5.js",
        "assets/vendor/libs/formvalidation/auto-focus.js",
        "assets/js/fullcalendar.js",
        "assets/js/app-calendar-events.js",
        "assets/js/app-calendar.js",
        "assets/js/form-validation.js"
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

    console.log("✅ Calendar scripts loaded successfully!");
};

window.initCalendar = function () {
    const calendarEl = document.getElementById("calendar");
    if (!calendarEl) {
        console.error("❌ Calendar element not found!");
        return;
    }

    if (typeof FullCalendar === "undefined") {
        console.error("❌ FullCalendar not loaded yet!");
        return;
    }

    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        editable: true,
        selectable: true,
        events: []
    });

    calendar.render();
    console.log("✅ Calendar initialized!");
};
