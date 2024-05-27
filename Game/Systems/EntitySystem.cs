using K8055Velleman.Game.Entities;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;

namespace K8055Velleman.Game.Systems
{
	internal class EntitySystem : SystemBase
	{
		private readonly List<EntityBase> _entities = [];
		private readonly List<EntityBase> _entitiesToDelete = [];
		internal GameUI GameUI { get; set; } = null;

		internal override void OnCreate()
		{
			base.OnCreate();
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();
			_entitiesToDelete.AddRange(_entities);
			_entities.Clear();
		}

		internal override void OnUpdate() 
		{ 
			List<EntityBase> temp = new(_entities);
			foreach (var entity in temp)
			{
				if ( _entities.Contains(entity) && entity.enabled ) entity.OnUpdate();
			}
			DestroyEntities();

        }

		/// <summary>
		/// Create an entity from the input type.
		/// </summary>
		/// <typeparam name="T">The type of the entity you want to create.</typeparam>
		/// <returns>The entity of T type</returns>
		internal T CreateEntity<T>() where T : EntityBase, new()
		{
			T entity = new();
			entity.OnCreate(this);
			_entities.Add(entity);
			return entity;
		}

        /// <summary>
        /// Create an entity from the input type.
        /// </summary>
        /// <typeparam name="T">The type you are sure it's going to be.</typeparam>
        /// <param name="type">The type you expect to have.</param>
        /// <returns>The entity of the type T but that are in fact of type.</returns>
        /// <exception cref="Exception">The input type is not an EntityBase or the Type type is different from the input type.</exception>
        internal T CreateEntity<T>(Type type) where T : EntityBase
        {
			object o = Activator.CreateInstance(type);
			if (o is not EntityBase baseEntity || baseEntity is not T entity || entity.GetType() != type) throw new Exception("The input type is not an EntityBase or the type type is different from the input type.");
			entity.OnCreate(this);
            _entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Destroy the given entity.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        internal void DestroyEntity(EntityBase entity)
		{
			if(!_entities.Contains(entity) || _entitiesToDelete.Contains(entity)) return;
			_entities.Remove(entity);
			_entitiesToDelete.Add(entity);
			return;
		}

		private void DestroyEntities()
		{
			foreach (EntityBase entity in _entitiesToDelete)
			{
				entity.OnDestroy();
			}
			_entitiesToDelete.Clear();
        }

        /// <summary>
        /// Get a List<T> of all the entity of the input type.
        /// </summary>
        /// <typeparam name="T">The the type of the entity you want.</typeparam>
        /// <returns>The List<T> that contains all the entity of T type.</returns>
        internal List<T> GetEntitiesByType<T>() where T : EntityBase
        {
			List<T> entityBases = [];

			foreach (var entity in _entities)
			{
				if (entity is T Tentity) entityBases.Add(Tentity);
			}

			return entityBases;

		}

        /// <summary>
        /// Check if the entity still exists.
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns>True if the entity exists.</returns>
        internal bool EntityExists(EntityBase entity)
		{
			return _entities.Contains(entity);
		}

        /// <summary>
        /// Get a new List<EntityBase> that contains all the entity.
        /// </summary>
        /// <returns>A new List<EntityBase> that contains all the entity.</returns>
        internal List<EntityBase> GetEntities()
		{
			return new(_entities);
		}

    }
}
