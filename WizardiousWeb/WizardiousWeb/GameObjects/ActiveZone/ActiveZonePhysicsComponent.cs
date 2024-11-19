using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class ActiveZonePhysicsComponent : PhysicsComponent
    {
        int countEnemy;
        bool startCount;

        public enum TurnState
        {
            None,
            Normal,
            Reset
        }

        public TurnState currentState;

        public List<GameObject> activeTurn;

        private GameScene curScene;
        private bool StateHelper;

        public ActiveZonePhysicsComponent(GameScene currentScene) : base(currentScene)
        {
            activeTurn = new List<GameObject>();
            currentState = TurnState.None;
            curScene = currentScene;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            //Control Turn Based Mechanics
            switch (currentState)
            {
                case TurnState.None:

                    countEnemy = 0;
                    activeTurn = new List<GameObject>();

                    foreach (GameObject s in gameObjects)
                    {
                        if (s.Name.Equals("Enemy") && s.IsActive && this.IsTouching(s))
                        {
                            activeTurn.Add(s);
                            countEnemy++;
                            startCount = true;
                        }

                        if (s.Name.Equals("Player") && s.IsActive && this.IsTouching(s, parent))
                        {
                            activeTurn.Add(s);
                        }
                    }

                    foreach (GameObject go in activeTurn)
                    {
                        if (go.Name.Equals("Player") && go.IsActive)
                        {
                            GameObject turnIndicator = new GameObject(null, new TurnIndicatorPhysicsComponent(curScene), new TurnIndicatorGraphicsComponent(curScene, this));
                            turnIndicator.Name = "TurnIndicator";

                            gameObjects.Add(turnIndicator);

                            currentState = TurnState.Reset;
                        }
                    }

                    break;

                case TurnState.Normal:

                    //For Enemy
                    if (activeTurn.First().Input is EnemyAIComponent)
                    {
                        EnemyAIComponent ec = activeTurn.First().Input as EnemyAIComponent;

                        if (StateHelper)
                        {
                            switch (activeTurn.First().SubName)
                            {
                                //For Special Enemy
                                case "n/a":


                                    break;

                                default:

                                    if ((ec.defendTurnCount > 1 || ec.Health > ec.MaxHealth / 2) && ec.attackTurnCount < 3)
                                    {
                                        ec.currentAIState = EnemyAIComponent.AIState.Attack;
                                        ec.attackTurnCount++;
                                        ec.defendTurnCount = 0;
                                    }
                                    else
                                    {
                                        ec.currentAIState = EnemyAIComponent.AIState.Defend;
                                        ec.defendTurnCount++;
                                        ec.attackTurnCount = 0;
                                    }

                                    break;
                            }
                            StateHelper = false;
                        }

                        if (ec.currentAIState != EnemyAIComponent.AIState.Wait)
                        {
                            //Busy Waiting
                        }
                        else
                        {
                            currentState = TurnState.Reset;
                        }
                    }

                    //For Player
                    else if (activeTurn.First().Input is PlayerInputComponent)
                    {
                        PlayerInputComponent pc = activeTurn.First().Input as PlayerInputComponent;

                        if (StateHelper)
                        {
                            pc.currentState = PlayerInputComponent.ActiveState.Attack;
                            StateHelper = false;
                        }

                        if (pc.currentState != PlayerInputComponent.ActiveState.Wait)
                        {
                            //Busy Waiting
                        }
                        else
                        {
                            currentState = TurnState.Reset;
                        }
                    }

                    activeTurn.First().Input.TurnSpeed = activeTurn.First().Input.MaxTurnSpeed;

                    break;

                case TurnState.Reset:

                    Console.WriteLine("Reset!");

                    bool isZero = false;

                    //Decrease turn speed by 1 after passing 1 turn
                    int objCount = activeTurn.Count();

                    for (int i = 0; i < objCount; ++i)
                    {
                        if (!activeTurn[i].IsActive)
                        {
                            activeTurn.RemoveAt(i--);
                            objCount--;
                            continue;
                        }

                        InputComponent ic = activeTurn[i].Input as InputComponent;

                        if (ic.TurnSpeed > 0)
                        {
                            ic.TurnSpeed--;
                        }
                        else
                        {
                            isZero = true;
                        }

                        //Console.WriteLine(activeTurn[i].Name + " " + ic.TurnSpeed);
                    }

                    if (activeTurn.Count() == 1 && activeTurn[0].Name.Equals("Player"))
                    {
                        currentState = TurnState.None;
                        parent.IsActive = false;
                        break;
                    }

                    //Re-compute and arrange into new pattern
                    List<GameObject> SortedList = activeTurn.OrderBy(o => o.Input.TurnSpeed).ToList();
                    activeTurn = SortedList;

                    StateHelper = true;
                    if (isZero && parent.IsActive) currentState = TurnState.Normal;

                    break;
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
