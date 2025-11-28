window.showToast = function (type, message) {
    let bg = type === "success" ? "text-bg-success" : "text-bg-danger";

    let toast = document.createElement("div");
    toast.className = `toast align-items-center text-white ${bg} border-0 position-fixed top-0 end-0 m-3`;
    toast.setAttribute("role", "alert");
    toast.setAttribute("aria-live", "assertive");
    toast.setAttribute("aria-atomic", "true");

    toast.style.zIndex = "99999";

    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    document.body.appendChild(toast);
    let bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    setTimeout(() => toast.remove(), 5000);
};
