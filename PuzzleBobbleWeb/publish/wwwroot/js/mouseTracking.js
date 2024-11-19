// wwwroot/js/mouseTracking.js

window.canvasMouseTracking = {
    dotNetHelper: null,
    initialize: function (canvasId, dotNetObject) {
        console.log("Mouse Tracking Initialize");
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        this.dotNetHelper = dotNetObject;

        // Listen for mouse movement over the canvas
        canvas.addEventListener('mousemove', (event) => {
            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;
            if (this.dotNetHelper) {
                this.dotNetHelper.invokeMethodAsync('UpdateMousePosition', x, y);
            }
        });

        // Handle mouse leaving the canvas
        canvas.addEventListener('mouseleave', () => {
            if (this.dotNetHelper) {
                // Optionally, set mouse position to an off-screen value when the mouse leaves the canvas
                this.dotNetHelper.invokeMethodAsync('UpdateMousePosition', -1, -1);
            }
        });
    }
};
