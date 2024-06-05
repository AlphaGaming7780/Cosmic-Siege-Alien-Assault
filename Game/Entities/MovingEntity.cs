using K8055Velleman.Game.Interfaces;
using K8055Velleman.Game.Systems;
using System;

namespace K8055Velleman.Game.Entities
{
    internal abstract class MovingEntity : EntityBase
    {
        internal Vector2 TaregtCenterLocation;
        internal abstract float Speed { get; }

        internal override void OnCreate(EntitySystem entitySystem)
        {
            base.OnCreate(entitySystem);
        }

        internal override void OnUpdate()
        {
            if (TaregtCenterLocation != CenterLocation)
            {
                Move();
                foreach (EntityBase entity in EntitySystem.GetEntities())
                {
                    if(EntitySystem.EntityExists(entity)) CheckColision(entity);
                }
            }
        }

        private void Move()
        {
            Vector2 vector2 = TaregtCenterLocation - CenterLocation;

            float multX = vector2.y == 0f ? 1f : Mathf.Clamp01(Math.Abs( vector2.x / (float)vector2.y));
            float multY = vector2.x == 0f ? 1f : Mathf.Clamp01(Math.Abs( vector2.y / (float)vector2.x));

            Vector2 move = new();

            if (TaregtCenterLocation.x > CenterLocation.x)
            {
                move.x = (Speed * multX) >= vector2.x ? vector2.x : Speed * multX * Math.Sign(vector2.x); // * UIManager.uiRatio.x
            }
            else
            {
                move.x = (Speed * multX) <= vector2.x ? vector2.x : Speed * multX * Math.Sign(vector2.x); // * UIManager.uiRatio.x
            }
            if (TaregtCenterLocation.y > CenterLocation.y)
            {

                move.y = (Speed * multY) >= vector2.y ? vector2.y : Speed * multY * Math.Sign(vector2.y); // * UIManager.uiRatio.y
            }
            else
            {
                move.y = (Speed * multY) <= vector2.y ? vector2.y : Speed * multY * Math.Sign(vector2.y); // * UIManager.uiRatio.y
            }

            CenterLocation += move;
        }

        private void CheckColision(EntityBase entityBase)
        {

            if (entityBase is null || entityBase.MainPanel is null || MainPanel is null) return;
            if (
                (
                    (MainPanel.Left >= entityBase.MainPanel.Left && MainPanel.Left <= entityBase.MainPanel.Right) ||
                    (MainPanel.Right >= entityBase.MainPanel.Left && MainPanel.Right <= entityBase.MainPanel.Right)
                ) && (
                    (MainPanel.Top >= entityBase.MainPanel.Top && MainPanel.Top <= entityBase.MainPanel.Bottom) ||
                    (MainPanel.Bottom >= entityBase.MainPanel.Top && MainPanel.Bottom <= entityBase.MainPanel.Bottom)
                )
            )
            {
                OnCollide(entityBase);
                entityBase.OnCollide(this);
            };
        }
    }
}
