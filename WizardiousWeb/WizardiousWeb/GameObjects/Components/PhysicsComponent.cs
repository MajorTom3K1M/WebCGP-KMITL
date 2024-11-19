using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WizardiousWeb
{
    public class PhysicsComponent : Component
    {
        public float GRAVITY = 600f;
        //public float GRAVITY = 981f;
        public float Restitution;
        public float Mass;

        public enum PhysicsType
        {
            STATICS,
            KINEMATICS,
            DYNAMICS
        }
        public PhysicsType EntityPhysicsType;

        public enum BoundingBoxType
        {
            NONE,
            AABB,
            CIRCLE
        }
        public BoundingBoxType EntityBoundingBoxType;

        public enum ImpluseType
        {
            NONE,
            SURFACE,
            NORMAL
        }
        public ImpluseType EntityImpluseType;

        public List<CollisionManifold> CollideeManifold;
        public struct CollisionManifold
        {
            public GameObject Collidee;
            public float Penetration;
            public Vector2 Normal;
        }

        public PhysicsComponent(GameScene currentScene)
        {
            Mass = float.PositiveInfinity;
            Restitution = 0f;
            EntityPhysicsType = PhysicsType.STATICS;
            EntityBoundingBoxType = BoundingBoxType.AABB;
            EntityImpluseType = ImpluseType.NONE;
            CollideeManifold = new List<CollisionManifold>();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            //Store past value
            Vector2 previousPosition = parent.Position;
            Vector2 previousVelocity = parent.Velocity;

            //Position calculation
            switch (EntityPhysicsType)
            {
                case PhysicsType.STATICS:
                    // do not do anything, this entity ignores all physics laws
                    break;
                case PhysicsType.KINEMATICS:
                    // entity is affected by everything except gravity
                    parent.Velocity += parent.Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    parent.Position += parent.Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    break;
                case PhysicsType.DYNAMICS:
                    // entity is affected by everything including gravity
                    parent.Acceleration.Y += GRAVITY;
                    parent.Velocity += parent.Acceleration * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    parent.Position += parent.Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    break;
            }

            //Detect collision
            CollideeManifold.Clear();
            CollisionManifold manifold;

            for (int i = gameObjects.IndexOf(parent) + 1; i < gameObjects.Count; i++)
            {
                switch (EntityBoundingBoxType)
                {
                    case BoundingBoxType.AABB:
                        if (gameObjects[i].Physics.EntityBoundingBoxType == BoundingBoxType.AABB)
                        {
                            if (IsTouchingAABBAABB(parent, gameObjects[i], out manifold))
                            {
                                CollideeManifold.Add(manifold);
                            }
                        }
                        else if (gameObjects[i].Physics.EntityBoundingBoxType == BoundingBoxType.CIRCLE)
                        {
                            if (IsTouchingAABBCircle(parent, gameObjects[i], out manifold))
                            {
                                CollideeManifold.Add(manifold);
                            }
                        }
                        break;
                    case BoundingBoxType.CIRCLE:
                        if (gameObjects[i].Physics.EntityBoundingBoxType == BoundingBoxType.AABB)
                        {
                            if (IsTouchingCircleAABB(parent, gameObjects[i], out manifold))
                            {
                                CollideeManifold.Add(manifold);
                            }
                        }
                        else if (gameObjects[i].Physics.EntityBoundingBoxType == BoundingBoxType.CIRCLE)
                        {
                            if (IsTouchingCircleCircle(parent, gameObjects[i], out manifold))
                            {
                                CollideeManifold.Add(manifold);
                            }
                        }
                        break;
                }
            }

            //Resolve collision

            foreach (CollisionManifold c in CollideeManifold)
            {
                switch (EntityImpluseType)
                {
                    case ImpluseType.SURFACE:
                        if (c.Collidee.Physics.EntityImpluseType == ImpluseType.SURFACE)
                        {
                            //Calculate the move back amount 
                            float moveBackPenetrationA = parent.Velocity.Length() * c.Penetration / (parent.Velocity + c.Collidee.Velocity).Length();
                            float moveBackPenetrationB = c.Collidee.Velocity.Length() * c.Penetration / (parent.Velocity + c.Collidee.Velocity).Length();

                            if (parent.Velocity.Length() != 0) parent.Position = parent.Position - Vector2.Normalize(parent.Velocity) * moveBackPenetrationA;
                            if (c.Collidee.Velocity.Length() != 0) c.Collidee.Position = c.Collidee.Position - Vector2.Normalize(c.Collidee.Velocity) * moveBackPenetrationB;

                            //No move after collision
                            parent.Velocity = Vector2.Zero;
                            c.Collidee.Velocity = Vector2.Zero;
                        }
                        else if (c.Collidee.Physics.EntityImpluseType == ImpluseType.NORMAL)
                        {
                            //Calculate the move back amount (only to dynamics one)
                            c.Collidee.Position = c.Collidee.Position + c.Normal * c.Penetration;

                            //Calculate the direction
                            Vector2 reflectDirection = c.Collidee.Velocity - 2 * Vector2.Dot(c.Collidee.Velocity, c.Normal) * c.Normal;

                            //Calculate the force
                            reflectDirection += parent.Velocity;

                            //calculate restitution
                            //clamp value to be between 0 and 1 first
                            float collideeRestitution = MathHelper.Clamp(c.Collidee.Physics.Restitution, 0, 1);

                            c.Collidee.Velocity = reflectDirection * collideeRestitution;
                        }
                        break;
                    case ImpluseType.NORMAL:
                        if (c.Collidee.Physics.EntityImpluseType == ImpluseType.SURFACE)
                        {
                            //Calculate the move back amount (only to dynamics one)
                            parent.Position = parent.Position - c.Normal * c.Penetration;

                            //Calculate the direction
                            Vector2 reflectDirection = parent.Velocity - 2 * Vector2.Dot(parent.Velocity, c.Normal) * c.Normal;

                            //Calculate the force
                            reflectDirection += c.Collidee.Velocity;

                            //calculate restitution
                            //clamp value to be between 0 and 1 first
                            float colliderRestitution = MathHelper.Clamp(Restitution, 0, 1);

                            parent.Velocity = reflectDirection * colliderRestitution;
                        }
                        else if (c.Collidee.Physics.EntityImpluseType == ImpluseType.NORMAL)
                        {
                            float moveBackPenetrationA = parent.Velocity.Length() * c.Penetration / (parent.Velocity + c.Collidee.Velocity).Length();
                            float moveBackPenetrationB = c.Collidee.Velocity.Length() * c.Penetration / (parent.Velocity + c.Collidee.Velocity).Length();

                            if (parent.Velocity.Length() != 0) parent.Position = parent.Position - Vector2.Normalize(parent.Velocity) * moveBackPenetrationA;
                            if (c.Collidee.Velocity.Length() != 0) c.Collidee.Position = c.Collidee.Position - Vector2.Normalize(c.Collidee.Velocity) * moveBackPenetrationB;

                            // Calculate relative velocity
                            Vector2 rv = c.Collidee.Velocity - parent.Velocity;

                            // Calculate relative velocity in terms of the normal direction
                            float velAlongNormal = Vector2.Dot(rv, c.Normal);

                            // Do not resolve if velocities are separating
                            if (velAlongNormal <= 0)
                            {
                                // Calculate restitution
                                float e = Math.Min(Restitution, c.Collidee.Physics.Restitution);

                                // Calculate impulse scalar
                                float j = -(1 + e) * velAlongNormal / 2.0f;

                                // Apply impulse
                                Vector2 impulse = j * c.Normal;
                                parent.Velocity -= impulse;
                                c.Collidee.Velocity += impulse;
                            }


                        }
                        break;
                }
            }
        }

        public override void Reset()
        {
        }


        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }


        #region Collision

        public bool IsTouching(GameObject s)
        {
            foreach (CollisionManifold c in CollideeManifold)
            {
                if (c.Collidee.Equals(s)) return true;
            }
            return false;
        }

        private bool IsTouchingAABBCircle(GameObject parent, GameObject s, out CollisionManifold manifold)
        {
            manifold = new CollisionManifold();

            Vector2 halfA = new Vector2(parent.Rectangle.Width / 2.0f, parent.Rectangle.Height / 2.0f);
            Vector2 halfB = new Vector2(s.Rectangle.Width / 2.0f, s.Rectangle.Height / 2.0f);


            //find the centre of each sprite's hit area (using .Center() will give a loss of precision)
            Vector2 centreA = new Vector2(parent.Position.X + parent.Rectangle.X + parent.Rectangle.Width / 2, parent.Position.Y + parent.Rectangle.Y + parent.Rectangle.Height / 2);
            Vector2 centreB = new Vector2(s.Position.X + s.Rectangle.X + s.Rectangle.Width / 2, s.Position.Y + s.Rectangle.Y + s.Rectangle.Height / 2);

            // Vector from A to B
            Vector2 n = centreB - centreA;

            // Closest point on A to center of B
            Vector2 closest = n;

            // Clamp point to edges of the AABB
            closest.X = MathHelper.Clamp(closest.X, -halfA.X, halfA.X);
            closest.Y = MathHelper.Clamp(closest.Y, -halfA.Y, halfA.Y);

            bool inside = false;

            // Circle is inside the AABB, so we need to clamp the circle's center
            // to the closest edge
            if (n == closest)
            {
                inside = true;

                // Find closest axis
                if (Math.Abs(n.X) > Math.Abs(n.Y))
                {
                    // Clamp to closest extent
                    closest.X = closest.X > 0 ? halfA.X : -halfA.X;
                }
                // y axis is shorter
                else
                {
                    // Clamp to closest extent
                    closest.Y = closest.Y > 0 ? halfA.Y : -halfA.Y;
                }
            }

            Vector2 normal = n - closest;
            float d = normal.LengthSquared();
            float r = s.Rectangle.Width < s.Rectangle.Height ? s.Rectangle.Width / 2.0f : s.Rectangle.Height / 2.0f;

            // Early out of the radius is shorter than distance to closest point and
            // Circle not inside the AABB
            if (d > r * r && !inside)
                return false;

            // AABBs have collided, now compute manifold
            manifold.Collidee = s;

            // Avoided sqrt until we needed
            d = (float)Math.Sqrt(d);

            // Collision normal needs to be flipped to point outside if circle was
            // inside the AABB
            if (inside)
            {
                manifold.Normal = -Vector2.Normalize(normal);
                manifold.Penetration = r - d;
            }
            else
            {
                manifold.Normal = Vector2.Normalize(normal);
                manifold.Penetration = r - d;
            }
            return true;
        }

        private bool IsTouchingCircleCircle(GameObject parent, GameObject s, out CollisionManifold manifold)
        {
            manifold = new CollisionManifold();

            float radiusA = parent.Rectangle.Width < parent.Rectangle.Height ? parent.Rectangle.Width / 2.0f : parent.Rectangle.Height / 2.0f;
            float radiusB = s.Rectangle.Width < s.Rectangle.Height ? s.Rectangle.Width / 2.0f : s.Rectangle.Height / 2.0f;

            //find the centre of each sprite's hit area (using .Center() will give a loss of precision)
            Vector2 centreA = new Vector2(parent.Position.X + parent.Rectangle.X + parent.Rectangle.Width / 2, parent.Position.Y + parent.Rectangle.Y + parent.Rectangle.Height / 2);
            Vector2 centreB = new Vector2(s.Position.X + s.Rectangle.X + s.Rectangle.Width / 2, s.Position.Y + s.Rectangle.Y + s.Rectangle.Height / 2);

            float r = radiusA + radiusB;
            float r2 = r * r;
            float distanceSquared = Vector2.DistanceSquared(centreA, centreB);
            if (r2 < distanceSquared)
            {
                //Not collided
                return false;
            }
            else
            {
                // Circles have collided, now compute manifold
                manifold.Collidee = s;

                // Calculate distance
                float distance = (float)Math.Sqrt(distanceSquared);

                if (distance != 0)
                {
                    // If distance between circles is not zero
                    // Normal vector is points from A to B, and is a unit vector
                    manifold.Normal = (centreB - centreA) / distance;

                    // Distance is difference between radius and distance
                    manifold.Penetration = r - distance;
                }
                else
                {
                    // If distance between circles is zero
                    manifold.Normal = Vector2.UnitX;

                    // Distance is difference between radius and distance
                    manifold.Penetration = (float)Math.Min(parent.Rectangle.Width / 2.0, s.Rectangle.Width / 2.0);

                }

                return true;
            }
        }

        private bool IsTouchingCircleAABB(GameObject parent, GameObject s, out CollisionManifold manifold)
        {
            manifold = new CollisionManifold();

            Vector2 halfA = new Vector2(parent.Rectangle.Width / 2.0f, parent.Rectangle.Height / 2.0f);
            Vector2 halfB = new Vector2(s.Rectangle.Width / 2.0f, s.Rectangle.Height / 2.0f);


            //find the centre of each sprite's hit area (using .Center() will give a loss of precision)
            Vector2 centreA = new Vector2(parent.Position.X + parent.Rectangle.X + parent.Rectangle.Width / 2, parent.Position.Y + parent.Rectangle.Y + parent.Rectangle.Height / 2);
            Vector2 centreB = new Vector2(s.Position.X + s.Rectangle.X + s.Rectangle.Width / 2, s.Position.Y + s.Rectangle.Y + s.Rectangle.Height / 2);

            // Vector from B to A
            Vector2 n = centreA - centreB;

            // Closest point on B to center of A
            Vector2 closest = n;

            // Clamp point to edges of the AABB
            closest.X = MathHelper.Clamp(closest.X, -halfB.X, halfB.X);
            closest.Y = MathHelper.Clamp(closest.Y, -halfB.Y, halfB.Y);

            bool inside = false;

            // Circle is inside the AABB, so we need to clamp the circle's center
            // to the closest edge
            if (n == closest)
            {
                inside = true;

                // Find closest axis
                if (Math.Abs(n.X) > Math.Abs(n.Y))
                {
                    // Clamp to closest extent
                    closest.X = closest.X > 0 ? halfB.X : -halfB.X;
                }
                // y axis is shorter
                else
                {
                    // Clamp to closest extent
                    closest.Y = closest.Y > 0 ? halfB.Y : -halfB.Y;
                }
            }

            Vector2 normal = n - closest;
            float d = normal.LengthSquared();
            float r = parent.Rectangle.Width < parent.Rectangle.Height ? parent.Rectangle.Width / 2.0f : parent.Rectangle.Height / 2.0f;

            // Early out of the radius is shorter than distance to closest point and
            // Circle not inside the AABB
            if (d > r * r && !inside)
                return false;

            // AABBs have collided, now compute manifold
            manifold.Collidee = s;

            // Avoided sqrt until we needed
            d = (float)Math.Sqrt(d);

            // Collision normal needs to be flipped to point outside if circle was
            // inside the AABB
            if (inside)
            {
                manifold.Normal = Vector2.Normalize(normal);
                manifold.Penetration = r - d;
            }
            else
            {
                manifold.Normal = -Vector2.Normalize(normal);
                manifold.Penetration = r - d;
            }
            return true;
        }

        private bool IsTouchingAABBAABB(GameObject parent, GameObject s, out CollisionManifold manifold)
        {
            manifold = new CollisionManifold();

            Vector2 halfA = new Vector2(parent.Rectangle.Width / 2.0f, parent.Rectangle.Height / 2.0f);
            Vector2 halfB = new Vector2(s.Rectangle.Width / 2.0f, s.Rectangle.Height / 2.0f);

            //find the centre of each sprite's hit area (using .Center() will give a loss of precision)
            Vector2 centreA = new Vector2(parent.Position.X + parent.Rectangle.X + halfA.X, parent.Position.Y + parent.Rectangle.Y + halfA.Y);
            Vector2 centreB = new Vector2(s.Position.X + s.Rectangle.X + halfB.X, s.Position.Y + s.Rectangle.Y + halfB.Y);

            //Not collided
            if (parent.Rectangle.Right < s.Rectangle.Left || parent.Rectangle.Left > s.Rectangle.Right) return false;
            if (parent.Rectangle.Bottom < s.Rectangle.Top || parent.Rectangle.Top > s.Rectangle.Bottom) return false;

            // AABBs have collided, now compute manifold
            manifold.Collidee = s;

            float distance = Vector2.Distance(centreA, centreB);

            if (distance != 0)
            {
                // If distance between AABBs is not zero

                // Distance is difference between radius and distance
                float xOverlapAB = (halfA.X + halfB.X) - (centreA.X - centreB.X);
                float xOverlapBA = (halfA.X + halfB.X) - (centreB.X - centreA.X);
                float yOverlapAB = (halfA.Y + halfB.Y) - (centreA.Y - centreB.Y);
                float yOverlapBA = (halfA.Y + halfB.Y) - (centreB.Y - centreA.Y);

                manifold.Penetration = Math.Min(xOverlapAB, Math.Min(xOverlapBA, Math.Min(yOverlapAB, yOverlapBA)));

                //calculate normal vector
                if (manifold.Penetration == xOverlapAB)
                {
                    //Left of A
                    manifold.Normal = new Vector2(-1, 0);
                }
                else if (manifold.Penetration == xOverlapBA)
                {
                    //Right of A
                    manifold.Normal = new Vector2(1, 0);
                }
                else if (manifold.Penetration == yOverlapAB)
                {
                    //Top of A
                    manifold.Normal = new Vector2(0, -1);
                }
                else if (manifold.Penetration == yOverlapBA)
                {
                    //Bottom of A
                    manifold.Normal = new Vector2(0, 1);
                }
            }
            else
            {
                // If distance between circles is zero
                manifold.Normal = Vector2.UnitX;

                // Distance is difference between radius and distance
                manifold.Penetration = (float)Math.Min(parent.Rectangle.Width / 2.0, s.Rectangle.Width / 2.0);

            }

            return true;
        }

        private float CalculateTimeCollided(GameObject A, GameObject B)
        {
            return (float)(-1 / 2 * Math.Sqrt(
                Math.Pow(2 * A.Position.X * A.Velocity.X - 2 * A.Position.X * B.Velocity.X + 2 * A.Position.Y * A.Velocity.Y - 2 * A.Position.Y * B.Velocity.Y - 2 * B.Position.X * A.Velocity.X + 2 * B.Position.X * B.Velocity.X - 2 * B.Position.Y * A.Velocity.Y + 2 * B.Position.Y * B.Velocity.Y, 2) -
                4 * (A.Velocity.X * A.Velocity.X - 2 * A.Velocity.X * B.Velocity.X + B.Velocity.X * B.Velocity.X + A.Velocity.Y * A.Velocity.Y - 2 * A.Velocity.Y * B.Velocity.Y + B.Velocity.Y * B.Velocity.Y) *
                (A.Position.X * A.Position.X - 2 * A.Position.X * B.Position.X + A.Position.Y * A.Position.Y - 2 * A.Position.Y * B.Position.Y + B.Position.X * B.Position.X + B.Position.Y * B.Position.Y - A.Rectangle.X / 2 - 2 * A.Rectangle.X / 2 * B.Rectangle.X / 2 - B.Rectangle.X / 2 * B.Rectangle.X / 2)) -
                A.Position.X * A.Velocity.X + A.Position.X * B.Velocity.X - A.Position.Y * A.Velocity.Y + A.Position.Y * B.Velocity.Y + B.Position.X * A.Velocity.X - B.Position.X * B.Velocity.X + B.Position.Y * A.Velocity.Y - B.Position.Y * B.Velocity.Y) /
                (A.Velocity.X * A.Velocity.X - 2 * A.Velocity.X * B.Velocity.X + B.Velocity.X * B.Velocity.X + A.Velocity.Y * A.Velocity.Y - 2 * A.Velocity.Y * B.Velocity.Y + B.Velocity.Y * B.Velocity.Y);
        }

        #endregion

        #region ClassicCollision
        public bool IsTouching(GameObject g, GameObject parent)
        {
            return IsTouchingLeft(g, parent) ||
                IsTouchingTop(g, parent) ||
                IsTouchingRight(g, parent) ||
                IsTouchingBottom(g, parent);
        }

        public bool IsTouchingLeft(GameObject g, GameObject parent)
        {
            return parent.Rectangle.Right > g.Rectangle.Left &&
                         parent.Rectangle.Left < g.Rectangle.Left &&
                         parent.Rectangle.Bottom > g.Rectangle.Top &&
                         parent.Rectangle.Top < g.Rectangle.Bottom;
        }

        public bool IsTouchingRight(GameObject g, GameObject parent)
        {
            return parent.Rectangle.Right > g.Rectangle.Right &&
                         parent.Rectangle.Left < g.Rectangle.Right &&
                         parent.Rectangle.Bottom > g.Rectangle.Top &&
                         parent.Rectangle.Top < g.Rectangle.Bottom;
        }

        public bool IsTouchingTop(GameObject g, GameObject parent)
        {
            return parent.Rectangle.Right > g.Rectangle.Left &&
                         parent.Rectangle.Left < g.Rectangle.Right &&
                         parent.Rectangle.Bottom > g.Rectangle.Top &&
                         parent.Rectangle.Top < g.Rectangle.Top;
        }

        public bool IsTouchingBottom(GameObject g, GameObject parent)
        {
            return parent.Rectangle.Right > g.Rectangle.Left &&
                         parent.Rectangle.Left < g.Rectangle.Right &&
                         parent.Rectangle.Bottom > g.Rectangle.Bottom &&
                         parent.Rectangle.Top < g.Rectangle.Bottom;
        }
        #endregion
    }
}
