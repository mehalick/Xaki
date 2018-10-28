var cacheName = "201810280813";
var filesToCache = [
    "",
    "https://xaki.azureedge.net/assets/favicon-636762577492363000.ico",
    "https://xaki.azureedge.net/assets/logo-text-only--white-636762353196739215.svg",
    "https://xaki.azureedge.net/assets/screenshot-01-636763134429881325.png",
    "https://xaki.azureedge.net/assets/screenshot-02-636763134444368750.png",
    "https://xaki.azureedge.net/assets/screenshot-03-636763134448780250.png",
    "https://xaki.azureedge.net/assets/screenshot-04-636763134451195384.png",
    "https://fonts.googleapis.com/css?family=Lato:400,700",
    "https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/components/reset.min.css",
    "https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.4.1/components/grid.min.css"
];

self.addEventListener("install", (e) => {
    console.log("[ServiceWorker] Install");
    e.waitUntil(
        caches.open(cacheName).then((cache) => {
            console.log("[ServiceWorker] Caching app shell");
            return cache.addAll(filesToCache);
        })
    );
});

self.addEventListener("activate", (e) => {
    console.log("[ServiceWorker] Activate");
    e.waitUntil(
        caches.keys().then((keyList) => {
            return Promise.all(keyList.map((key) => {
                if (key !== cacheName) {
                    console.log("[ServiceWorker] Removing old cache", key);
                    return caches.delete(key);
                }
            }));
        })
    );
    return self.clients.claim();
});

self.addEventListener("fetch", (e) => {
    console.log("[Service Worker] Fetch", e.request.url);
    e.respondWith(
        caches.match(e.request).then((response) => {
            return response || fetch(e.request);
        })
    );
});
