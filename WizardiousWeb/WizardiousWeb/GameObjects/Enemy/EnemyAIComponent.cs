using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WizardiousWeb
{
    public class EnemyAIComponent : InputComponent
    {
        public float MaxHealth = 80f;
        public float Health;

        GameObject newBullet;

        private GameObject catapultBullet;

        public enum AIState
        {
            None,
            Attack,
            Defend,
            Wait,
            Died,
            WaitForWait
        }

        public enum Skill
        {
            None,
            Skill1,
            Skill2,
            Skill3
        }

        public AIState currentAIState;
        public Skill currentSkill;

        private bool TimerHelper;
        private double curTime;

        //Decision Helper Parameter
        public int attackTurnCount, defendTurnCount;

        public EnemyAIComponent(GameScene currentScene) : base(currentScene)
        {
            attackTurnCount = defendTurnCount = 0;
            Health = MaxHealth;
            catapultBullet = new GameObject(null, new CatapultPhysicsComponent(currentScene), new CatapultGraphicsComponent(currentScene));
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            switch (currentAIState)
            {
                case AIState.None:



                    break;

                case AIState.Wait:

                    break;

                case AIState.Attack:

                    //TODO: AI for Attack
                    //Console.WriteLine("Enemy Attack");

                    Vector2 playerPos = Vector2.Zero;

                    foreach (GameObject go in gameObjects)
                    {
                        if (go.IsActive && go.Name.Equals("Player"))
                        {
                            playerPos = go.Position;
                        }
                    }

                    //TODO: Compute Angle between Enemy and Player

                    //Temporary angle
                    float angle = new Random().Next(105, 165);

                    newBullet = catapultBullet.Clone() as GameObject;

                    switch (currentSkill)
                    {
                        case Skill.None:

                            newBullet.Name = "EnemyBullet";
                            newBullet.ShootAngle = 0; //angle;
                            newBullet.ShootPower = 0; //new Random().Next(100, 1000) * 3;
                            newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                            newBullet.Position = new Vector2(playerPos.X, 0);
                            newBullet.Reset();

                            break;

                        case Skill.Skill1:

                            break;
                    }

                    //Add new projectile into game
                    gameObjects.Add(newBullet);

                    //Enter Wait State after finish all action
                    currentAIState = AIState.WaitForWait;

                    break;

                case AIState.WaitForWait:

                    if (!newBullet.IsActive)
                    {
                        if (!TimerHelper)
                        {
                            TimerHelper = true;
                            curTime = gameTime.TotalGameTime.TotalSeconds;
                        }

                        if (gameTime.TotalGameTime.TotalSeconds - curTime >= 0.5f)
                        {
                            currentAIState = AIState.Wait;
                            TimerHelper = false;
                        }
                    }


                    break;

                case AIState.Defend:

                    //TODO: AI for Defend
                    //Console.WriteLine("Enemy Defend");

                    //Enter Wait State after finish all action
                    currentAIState = AIState.Wait;

                    break;

                case AIState.Died:

                    parent.IsActive = false;

                    break;
            }

            if (Health <= 0)
            {
                currentAIState = AIState.Died;
            }
        }

        public override void Reset()
        {
            Health = MaxHealth;
            base.Reset();
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
            if (sender.Equals(this)) return;

            if (message == 100)
            {
                Health -= Singleton.Instance.Damage;
            }
        }
    }
}
