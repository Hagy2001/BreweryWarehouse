(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var overlay = document.getElementById('bw-load-overlay');
        if (!overlay) return;

        if (sessionStorage.getItem('bw-animated')) {
            overlay.parentNode.removeChild(overlay);
            return;
        }

        if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
            overlay.parentNode.removeChild(overlay);
            return;
        }

        setTimeout(function () {
            overlay.classList.add('bw-overlay--dismiss');
            overlay.addEventListener('transitionend', function onDismiss() {
                overlay.removeEventListener('transitionend', onDismiss);
                overlay.parentNode.removeChild(overlay);
                //sessionStorage.setItem('bw-animated', 'true');
            });
        }, 3050);

        setTimeout(function () {
            if (overlay.parentNode) overlay.parentNode.removeChild(overlay);
        }, 3700);
    });
}());
