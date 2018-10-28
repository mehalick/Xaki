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
            dimPage: true,
            transition: "overlay",
            mobileTransition: "uncover"
        });
        $menu.sidebar("attach events", ".launch.button, .view-ui, .launch.item");
    });
})(jQuery);
