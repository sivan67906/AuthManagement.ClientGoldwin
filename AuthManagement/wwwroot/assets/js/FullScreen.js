window.fullscreen = function () {
    const btn = document.getElementById("themeToggleBtn1");
    const icon = document.getElementById("fsIcon");

    if (!btn) return;

    btn.addEventListener("click", function (event) {
        event.preventDefault();
        event.stopPropagation();

        if (!document.fullscreenElement) {
            // ENTER FULLSCREEN
            document.documentElement.requestFullscreen().then(() => {
                icon.classList.remove("bi-fullscreen");
                icon.classList.add("bi-fullscreen-exit");
            });
        } else {
            // EXIT FULLSCREEN
            document.exitFullscreen().then(() => {
                icon.classList.remove("bi-fullscreen-exit");
                icon.classList.add("bi-fullscreen");
            });
        }
    });
};


