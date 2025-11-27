window.activateHorizontalLayout = function () {
    try {
        // 1️⃣ HTML tag update
        document.documentElement.setAttribute("data-template", "horizontal-menu-template");

        // 2️⃣ aside tag update
        const aside = document.querySelector("aside.layout-menu");
        if (aside) {
            aside.className = "layout-menu-horizontal menu menu-horizontal container-fluid flex-grow-0 null";
        } else {
            console.warn("❌ <aside> element with class 'layout-menu' not found!");
        }

        // 3️⃣ layout-wrapper class update
        const layoutWrapper = document.querySelector(".layout-wrapper");
        if (layoutWrapper) {
            layoutWrapper.className = "layout-wrapper layout-navbar-full layout-horizontal layout-without-menu";
        } else {
            console.warn("❌ .layout-wrapper element not found!");
        }

        // 4️⃣ navbar class update
        const navbar = document.querySelector("nav.layout-navbar");
        if (navbar) {
            navbar.className = "layout-navbar navbar navbar-expand-xl align-items-center";
        } else {
            console.warn("❌ <nav> element with class 'layout-navbar' not found!");
        }

        console.log("✅ Horizontal layout activated successfully!");
    } catch (e) {
        console.error("Layout switch error:", e);
    }
};

window.enableHoverDropdowns = function () {
    const dropdowns = document.querySelectorAll(".menu-horizontal .dropdown");

    dropdowns.forEach(dropdown => {
        dropdown.addEventListener("mouseenter", function () {
            const toggle = this.querySelector("[data-bs-toggle='dropdown']");
            if (toggle) {
                const dropdownInstance = bootstrap.Dropdown.getOrCreateInstance(toggle);
                dropdownInstance.show();
            }
        });

        dropdown.addEventListener("mouseleave", function () {
            const toggle = this.querySelector("[data-bs-toggle='dropdown']");
            if (toggle) {
                const dropdownInstance = bootstrap.Dropdown.getOrCreateInstance(toggle);
                dropdownInstance.hide();
            }
        });
    });

    console.log("✅ Hover dropdown enabled");
};

