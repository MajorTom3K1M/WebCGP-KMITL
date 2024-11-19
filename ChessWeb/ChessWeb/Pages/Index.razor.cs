using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace ChessWeb.Pages
{
    public partial class Index
    {
        private DotNetObjectReference<ChessWebGame> _dotNetRef;
        private ChessWebGame _gameInstance;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                // Create the game instance and enforce Singleton
                if (_gameInstance == null)
                {
                    // Set the canvas size via JavaScript (ensure these values are defined)
                    await JsRuntime.InvokeVoidAsync("setCanvasSize");

                    _gameInstance = new ChessWebGame();

                    //// Create a DotNetObjectReference for JavaScript to call back
                    _dotNetRef = DotNetObjectReference.Create(_gameInstance);

                    //// Initialize mouse tracking with the canvas ID and the .NET object reference
                    await JsRuntime.InvokeVoidAsync("canvasMouseTracking.initialize", "theCanvas", _dotNetRef);

                    _gameInstance.Run();
                }

                // Start the game loop via JavaScript interop
                await JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            // init game
            //if (_game == null)
            //{
            //    _game = new ChessWebGame();
            //    _game.Run();
            //}

            //// run gameloop
            //_game.Tick();
            _gameInstance?.Tick();
        }

    }
}
