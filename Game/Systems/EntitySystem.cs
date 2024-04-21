using K8055Velleman.Game.Entities;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Systems
{
	internal class EntitySystem : SystemBase
	{
		private readonly List<EntityBase> entities = [];
		internal GameUI GameUI { get; private set; }

		internal override void OnCreate()
		{
			base.OnCreate();
			GameUI = GameManager.GetOrCreateSystem<GameSystem>().GameUI;
			GameWindow.Resize += OnResize;

		}

		internal override void OnDestroy()
		{
			base.OnDestroy();
			GameWindow.Resize -= OnResize;
			List<EntityBase> temp = new(entities);
            entities.Clear();
            foreach (var entity in temp)
			{
				DestroyEntity(entity);
			}
		}

		internal override void OnUpdate() 
		{ 
			List<EntityBase> temp = new(entities);
			foreach (var entity in temp)
			{
				if ( entities.Contains(entity) && entity.enabled ) entity.OnUpdate();
			}
		}

		internal T CreateEntity<T>() where T : EntityBase, new()
		{
			T entity = new();
			entity.OnCreate(this);
			entities.Add(entity);
			return entity;
		}

        internal T CreateEntity<T>(Type type) where T : EntityBase
        {
			object o = Activator.CreateInstance(type);
			if (o is not EntityBase baseEntity || baseEntity is not T entity || entity.GetType() != type) throw new Exception("The input type is not an EntityBase or the type type is different from the input type.");
			entity.OnCreate(this);
            entities.Add(entity);
            return entity;
        }

        internal bool DestroyEntity(EntityBase entity)
		{
			if(!entities.Contains(entity)) return false;
			entities.Remove(entity);
			entity.OnDestroy();
			return true;
		}

		private void OnResize(object sender, EventArgs e)
		{
			foreach (EntityBase entity in entities)
			{
				entity.OnResize();
			}
        }

        internal List<T> GetEntitiesByType<T>() where T : EntityBase
        {
			List<T> entityBases = [];

			foreach (var entity in entities)
			{
				if (entity is T Tentity) entityBases.Add(Tentity);
			}

			return entityBases;

		}

		internal bool EntityExist(EntityBase entity)
		{
			return entities.Contains(entity);
		}

		internal List<EntityBase> GetEntities()
		{
			return new(entities);
		}

    }
}
