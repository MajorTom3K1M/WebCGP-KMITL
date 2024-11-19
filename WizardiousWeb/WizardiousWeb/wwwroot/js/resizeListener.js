// wwwroot/js/resizeListener.js

window.resizeListener = {
    initialize: function (dotNetHelper) {
        window.onresize = () => {
            // Debounce or throttle if necessary
            dotNetHelper.invokeMethodAsync('OnWindowResized');
        };
    },
    dispose: function () {
        window.onresize = null;
    }
};
