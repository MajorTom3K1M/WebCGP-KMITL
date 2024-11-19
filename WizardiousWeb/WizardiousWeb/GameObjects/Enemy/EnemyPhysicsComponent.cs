using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class EnemyPhysicsComponent : PhysicsComponent
    {
        private bool isTouch = true;
        private float count;

        public EnemyPhysicsComponent(GameScene currentScene) : base(currentScene)
        {

        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            count = 0;
            foreach (GameObject s in gameObjects)
            {
                if (s.IsActive && IsTouching(s) && s.Name.Equals("Bullet"))
                {
                    parent.SendMessage(100, this);
                    s.IsActive = false;
                }

                if (s.IsActive && IsTouching(s) && s.Name.Equals("PenetrateBullet") && isTouch)
                {
                    parent.SendMessage(100, this);
                    isTouch = false;
                }

                if (s.IsActive && s.Name.Equals("PenetrateBullet"))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                isTouch = true;
            }

            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
            if (sender.Equals(this)) return;
        }
    }
}
