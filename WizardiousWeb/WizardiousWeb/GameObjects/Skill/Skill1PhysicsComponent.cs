using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    class Skill1PhysicsComponent : PhysicsComponent
    {
        public Skill1PhysicsComponent(GameScene currentScene) : base(currentScene)
        {
            EntityPhysicsType = PhysicsType.KINEMATICS;
            EntityBoundingBoxType = BoundingBoxType.AABB;
            EntityImpluseType = ImpluseType.NONE;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            parent.Velocity.X = (float)Math.Cos(MathHelper.ToRadians(90 - parent.ShootAngle)) * 800;
            parent.Velocity.Y = -1 * (float)Math.Sin(MathHelper.ToRadians(90 - parent.ShootAngle)) * 800;

            //Remove when out of screen
            foreach (GameObject s in gameObjects)
            {
                if (s.IsActive && s.Name.Equals("Player"))
                {
                    if (parent.Position.X > s.Position.X + 1366)
                    {
                        parent.IsActive = false;
                    }
                }

                if (s.IsActive && s.Name.Equals("Floor") && this.IsTouching(s, parent))
                {
                    parent.IsActive = false;
                }
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
