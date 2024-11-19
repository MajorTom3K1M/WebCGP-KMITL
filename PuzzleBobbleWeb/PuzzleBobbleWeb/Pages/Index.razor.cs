using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace PuzzleBobbleWeb.Pages
{
    public partial class Index
    {
        private DotNetObjectReference<PuzzleBobbleWebGame> _dotNetRef;
        private PuzzleBobbleWebGame _gameInstance;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                // Set the canvas size via JavaScript (ensure these values are defined)
                await JsRuntime.InvokeVoidAsync("setCanvasSize", Singleton.MAINSCREEN_WIDTH, Singleton.MAINSCREEN_HEIGHT);

                // Create the game instance and enforce Singleton
                if (_gameInstance == null)
                {
                    _gameInstance = new PuzzleBobbleWebGame();

                    //// Create a DotNetObjectReference for JavaScript to call back
                    _dotNetRef = DotNetObjectReference.Create(_gameInstance);

                    //// Initialize mouse tracking with the canvas ID and the .NET object reference
                    await JsRuntime.InvokeVoidAsync("canvasMouseTracking.initialize", "theCanvas", _dotNetRef);

                    _gameInstance.Run();
                }

                // Start the game loop via JavaScript interop
                //await JsRuntime.InvokeVoidAsync("initRenderJS", _dotNetRef);
                await JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            //if (_gameInstance == null)
            //{
            //    _gameInstance = new PuzzleBobbleWebGame();
            //    _gameInstance.Run();
            //}

            // Run game loop tick
            _gameInstance?.Tick();
        }
    }
}


//using Microsoft.JSInterop;
//using Microsoft.Xna.Framework;
//using System;

//namespace PuzzleBobbleWeb.Pages
//{
//    public partial class Index
//    {
//        Game _game;

//        protected override void OnAfterRender(bool firstRender)
//        {
//            base.OnAfterRender(firstRender);

//            if (firstRender)
//            {
//                JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
//            }
//        }

//        [JSInvokable]
//        public void TickDotNet()
//        {
//            // init game
//            if (_game == null)
//            {
//                _game = new PuzzleBobbleWebGame();
//                _game.Run();
//            }

//            // run gameloop
//            _game.Tick();
//        }

//    }
//}
