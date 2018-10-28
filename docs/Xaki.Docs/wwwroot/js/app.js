(function () {
    "use strict";

    var app = {
        isLoading: true,
        spinner: document.querySelector(".loader")
    };

    if ("serviceWorker" in navigator) {
        navigator.serviceWorker
            .register("./service-worker.js")
            .then(function () { console.log("Service Worker Registered"); });
    }

    app.loadPasswords = function () {
        console.log("loaded");

        if (app.isLoading) {
            app.spinner.setAttribute("hidden", true);
            app.container.removeAttribute("hidden");
            app.isLoading = false;
        }
    };

    app.loadPasswords();
})();
