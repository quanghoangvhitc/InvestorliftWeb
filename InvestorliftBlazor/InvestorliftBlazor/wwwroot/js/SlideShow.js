
$('[data-fancybox="gallery"]').fancybox({
    afterLoad: function (instance, current) {
        var pixelRatio = window.devicePixelRatio || 1;

        if (pixelRatio > 1.5) {
            current.width = current.width / pixelRatio;
            current.height = current.height / pixelRatio;
        }
    },
    closeExisting: false,
    loop: true,
    gutter: 50,
    keyboard: true,
    preventCaptionOverlap: false,
    arrows: true,
    infobar: true,
    smallBtn: "auto"
});