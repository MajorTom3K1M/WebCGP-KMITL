using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    class TurnIndicatorPhysicsComponent : PhysicsComponent
    {

        //public float Angle;
        public TurnIndicatorPhysicsComponent(GameScene currentScene) : base(currentScene)
        {
            EntityPhysicsType = PhysicsType.DYNAMICS;
            EntityBoundingBoxType = BoundingBoxType.AABB;
            EntityImpluseType = ImpluseType.NONE;
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

        public void shoot(GameObject parent)
        {
            parent.Velocity.X = (float)Math.Cos(MathHelper.ToRadians(45)) * (300);
            parent.Velocity.Y = -1 * (float)Math.Sin(MathHelper.ToRadians(45)) * 900;
        }
    }
}
