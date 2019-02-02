($ => {
    "use strict";

    if ("serviceWorker" in navigator) {
        navigator.serviceWorker
            .register("/service-worker.js")
            .then(function () { console.log("[Service Worker] Registered"); });
    }

    $(() => {
        const $menu = $("#sidebar");
        $menu.sidebar({
            debug: true,
            dimPage: true,
            transition: "overlay",
            mobileTransition: "uncover"
        });
        $menu.sidebar("attach events", ".launch.button, .view-ui, .launch.item");

        const links = document.querySelectorAll(".ui.menu a.item");
        for (let i = 0; i < links.length; i++) {
            if (links[i].pathname === window.location.pathname) {
                links[i].classList.add("active");
            }
        }
    });
})(jQuery);
