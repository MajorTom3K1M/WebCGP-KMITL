using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class ShopPhysicsComponent : PhysicsComponent
    {
        public ShopPhysicsComponent(GameScene currentScene) : base(currentScene)
        {
        }
        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {

            base.Update(gameTime, gameObjects, parent);
        }


        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);

            if (sender.Equals(this)) return;

            if (message == 101)
            {

            }
            else if (message == 100)
            {

            }
        }
    }
}
