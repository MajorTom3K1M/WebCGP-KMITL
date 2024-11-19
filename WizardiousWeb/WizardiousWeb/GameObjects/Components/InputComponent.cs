using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace WizardiousWeb
{
    public class InputComponent : Component
    {
        public int MaxTurnSpeed = 5;
        public int TurnSpeed;

        public Dictionary<string, Keys> InputList;

        public InputComponent(GameScene currentScene)
        {
            InputList = new Dictionary<string, Keys>();
            TurnSpeed = MaxTurnSpeed;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent) { }

        public override void Reset() { }

        public virtual void ChangeMappingKey(string Key, Keys newInput)
        {
            InputList[Key] = newInput;
        }

        public override void ReceiveMessage(int message, Component sender) { }
    }
}
