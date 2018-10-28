var cacheName = "xk-0.0.1";
var filesToCache = [
    "",
    //"/index.html",
    "https://xaki.azureedge.net/assets/favicon-636762577492363000.ico",
    "https://fonts.googleapis.com/css?family=Lato:400,700",
    "https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/components/reset.min.css",
    "https://fonts.googleapis.com/css?family=Lato:400,700",
    "https://s3.amazonaws.com/github/ribbons/forkme_right_darkblue_121621.png",
    "https://xaki.azureedge.net/assets/logo-text-only--white-636762353196739215.svg"
];

self.addEventListener("install", function (e) {
    console.log("[ServiceWorker] Install");
    e.waitUntil(
        caches.open(cacheName).then(function (cache) {
            console.log("[ServiceWorker] Caching app shell");
            return cache.addAll(filesToCache);
        })
    );
});

self.addEventListener("activate", function (e) {
    console.log("[ServiceWorker] Activate");
    e.waitUntil(
        caches.keys().then(function (keyList) {
            return Promise.all(keyList.map(function (key) {
                if (key !== cacheName) {
                    console.log("[ServiceWorker] Removing old cache", key);
                    return caches.delete(key);
                }
            }));
        })
    );
    return self.clients.claim();
});

self.addEventListener("fetch", function (e) {
    console.log("[Service Worker] Fetch", e.request.url);
    e.respondWith(
        caches.match(e.request).then(function (response) {
            return response || fetch(e.request);
        })
    );
});
