// Main page function
window.Observer = {
    observer: null,
    Initialize: function (component, observerTargetId) {
        this.observer = new IntersectionObserver(e => {
            component.invokeMethodAsync('OnIntersection');
        });

        let element = document.getElementById(observerTargetId);
        if (element == null) throw new Error("The observable target was not found");
        this.observer.observe(element);
    }
};

// Detail page function
window.onscroll = function () {
    var btn = $("#btnTop");
    if (btn) {
        if (document.body.scrollTop > 300 || document.documentElement.scrollTop > 300) {
            btn.removeClass('hide-button');
            btn.addClass('show-button');
        } else {
            btn.removeClass('show-button');
            btn.addClass('hide-button');
        }
    }
};

backToTop = function () {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
};

scrollVertical = targetId => {
    var target = document.getElementById(targetId);
    if (target) {
        target.scrollIntoView();
    }
};

scrollHorizontal = targetId => {
    var target = document.getElementById(targetId);
    if (target) {
        target.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'center' });
    }
}