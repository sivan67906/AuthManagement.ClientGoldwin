window.initializeInvoiceTable = function () {
    const table = document.querySelector("#DataTables_Table_0 tbody");
    if (!table) return;

    const rows = Array.from(table.querySelectorAll("tr"));
    const rowsPerPageSelect = document.querySelector("#dt-length-0");
    const searchInput = document.querySelector("#dt-search-0");
    const pagination = document.querySelector("#pagination");

    let currentPage = 1;
    let rowsPerPage = parseInt(rowsPerPageSelect.value || 6);
    let filteredRows = [...rows];

    function renderTable() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        rows.forEach(r => r.style.display = "none");
        filteredRows.slice(start, end).forEach(r => r.style.display = "");

        renderPagination();
        updateInfo();
    }

    function renderPagination() {
        if (!pagination) return;
        pagination.innerHTML = "";

        const totalPages = Math.ceil(filteredRows.length / rowsPerPage);

        function createPageButton(label, page, disabled = false, active = false, html = null) {
            const li = document.createElement("li");
            li.className = `dt-paging-button page-item ${disabled ? 'disabled' : ''} ${active ? 'active' : ''}`;
            const btn = document.createElement("button");
            btn.className = "page-link";
            btn.type = "button";
            if (html) {
                btn.innerHTML = html;
            } else {
                btn.textContent = label;
            }
            if (!disabled && !active) {
                btn.addEventListener("click", () => {
                    currentPage = page;
                    renderTable();
                });
            }
            li.appendChild(btn);
            return li;
        }

        // First
        pagination.appendChild(createPageButton("", 1, currentPage === 1, false, '<i class="icon-base ti tabler-chevrons-left scaleX-n1-rtl icon-18px"></i>'));
        // Prev
        pagination.appendChild(createPageButton("", currentPage - 1, currentPage === 1, false, '<i class="icon-base ti tabler-chevron-left scaleX-n1-rtl icon-18px"></i>'));

        for (let i = 1; i <= totalPages; i++) {
            pagination.appendChild(createPageButton(i, i, false, currentPage === i));
        }

        // Next
        pagination.appendChild(createPageButton("", currentPage + 1, currentPage === totalPages, false, '<i class="icon-base ti tabler-chevron-right scaleX-n1-rtl icon-18px"></i>'));
        // Last
        pagination.appendChild(createPageButton("", totalPages, currentPage === totalPages, false, '<i class="icon-base ti tabler-chevrons-right scaleX-n1-rtl icon-18px"></i>'));
    }

    function updateInfo() {
        const info = document.querySelector("#DataTables_Table_0_info");
        if (!info) return;
        const start = filteredRows.length === 0 ? 0 : (currentPage - 1) * rowsPerPage + 1;
        const end = Math.min(currentPage * rowsPerPage, filteredRows.length);
        info.textContent = `Showing ${start} to ${end} of ${filteredRows.length} entries`;
    }

    function applySearch() {
        const term = searchInput.value.toLowerCase();
        filteredRows = rows.filter(r => r.textContent.toLowerCase().includes(term));
        currentPage = 1;
        renderTable();
    }

    // Event listeners
    if (searchInput) {
        searchInput.addEventListener("input", applySearch);
    }

    if (rowsPerPageSelect) {
        rowsPerPageSelect.addEventListener("change", () => {
            rowsPerPage = parseInt(rowsPerPageSelect.value);
            currentPage = 1;
            renderTable();
        });
    }

    renderTable();
};
