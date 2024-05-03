using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities
{
	internal abstract class MovingEntity : EntityBase
	{
        internal Vector2 taregtCenterLocation;
		//internal int speed = 1;
		internal abstract float Speed { get; }

		internal override void OnCreate(EntitySystem entitySystem)
		{
			base.OnCreate(entitySystem);
		}

		internal override void OnUpdate()
		{
			if (taregtCenterLocation != CenterLocation)
			{
				Move();
				foreach (EntityBase entity in EntitySystem.GetEntities())
				{
					if(EntitySystem.EntityExist(entity)) CheckColision(entity);
				}
			}
        }

		private void Move()
		{
			Vector2 vector2 = taregtCenterLocation - CenterLocation;

			float multX = vector2.y == 0f ? 1f : Mathf.Clamp01(Math.Abs( vector2.x / (float)vector2.y));
			float multY = vector2.x == 0f ? 1f : Mathf.Clamp01(Math.Abs( vector2.y / (float)vector2.x));

			Vector2 move = new();

			if (taregtCenterLocation.x > CenterLocation.x)
			{
				move.x = (Speed * multX) >= vector2.x ? vector2.x : Speed * multX * Math.Sign(vector2.x); // * UIManager.uiRatio.x
            }
            else
			{
				move.x = (Speed * multX) <= vector2.x ? vector2.x : Speed * multX * Math.Sign(vector2.x); // * UIManager.uiRatio.x
            }
			if (taregtCenterLocation.y > CenterLocation.y)
			{

				move.y = (Speed * multY) >= vector2.y ? vector2.y : Speed * multY * Math.Sign(vector2.y); // * UIManager.uiRatio.y
            }
			else
			{
				move.y = (Speed * multY) <= vector2.y ? vector2.y : Speed * multY * Math.Sign(vector2.y); // * UIManager.uiRatio.y
            }

			CenterLocation += move;
        }

        internal virtual void CheckColision(EntityBase entityBase)
        {

            if (entityBase is null || !EntitySystem.EntityExist(entityBase) || entityBase.mainPanel is null || mainPanel is null) return;
            if (
                (
                    (mainPanel.Left >= entityBase.mainPanel.Left && mainPanel.Left <= entityBase.mainPanel.Right) ||
                    (mainPanel.Right >= entityBase.mainPanel.Left && mainPanel.Right <= entityBase.mainPanel.Right)
                ) && (
                    (mainPanel.Top >= entityBase.mainPanel.Top && mainPanel.Top <= entityBase.mainPanel.Bottom) ||
                    (mainPanel.Bottom >= entityBase.mainPanel.Top && mainPanel.Bottom <= entityBase.mainPanel.Bottom)
                )
            )
            {
                OnCollide(entityBase);
                entityBase.OnCollide(this);
            };
        }

    }
}
