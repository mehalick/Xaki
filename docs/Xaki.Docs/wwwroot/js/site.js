($ => {
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
