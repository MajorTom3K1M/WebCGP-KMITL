﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WizardiousWeb
{
    public class FloorPhysicsComponent : PhysicsComponent
    {
        public FloorPhysicsComponent(GameScene currentScene) : base(currentScene)
        {
            EntityPhysicsType = PhysicsType.STATICS;
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
        }
    }
}
