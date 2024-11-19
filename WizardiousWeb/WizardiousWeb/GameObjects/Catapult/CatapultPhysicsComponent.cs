using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    class CatapultPhysicsComponent : PhysicsComponent
    {
        Vector2 origin;

        public CatapultPhysicsComponent(GameScene currentScene) : base(currentScene)
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
            parent.Velocity.X = (float)Math.Cos(MathHelper.ToRadians(90 - parent.ShootAngle)) * (parent.ShootPower + 400);
            parent.Velocity.Y = -1 * (float)Math.Sin(MathHelper.ToRadians(90 - parent.ShootAngle)) * (parent.ShootPower + 400);

            //Remove after bullet hit Floor
            foreach (GameObject g in gameObjects)
            {
                if (g.IsActive && g.Name.Equals("Floor") && this.IsTouching(g, parent))
                {
                    parent.IsActive = false;
                }
            }

            //Remove after out of screen
            if (parent.Position.Y > Singleton.MAINSCREEN_HEIGHT)
            {
                if (parent.IsActive) parent.IsActive = false;
            }

            if (parent.Position.X > Singleton.Instance.CameraPosition.X + Singleton.MAINSCREEN_WIDTH / 2)
            {
                if (parent.IsActive) parent.IsActive = false;
            }

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
