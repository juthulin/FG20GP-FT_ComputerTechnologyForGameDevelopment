using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

namespace Main.Scripts
{
    public class ProjectileHandler : GameHandlerComponent
    {
        private readonly GameHandler _gameHandler = null;
        private readonly float _timeBetweenShots;
        private readonly List<Entity> _projectilePool = new List<Entity>();
        
        private float _shotTimer = 0f;
        private EntityManager _entityManager;

        public ProjectileHandler(in GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _timeBetweenShots = gameHandler.timeBetweenShots;
        }

        public void Init()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            SpawnProjectiles();
        }

        public void Tick()
        {
            HandlePlayerShooting();
        }

        private void HandlePlayerShooting()
        {
            _shotTimer -= UnityEngine.Time.deltaTime;
            if (_shotTimer <= 0f && UnityEngine.Input.GetKey(UnityEngine.KeyCode.Space))
            {
                _shotTimer = _timeBetweenShots;
                
                Shoot();
            }
        }

        private void Shoot()
        {
            for (int i = 0; i < _projectilePool.Count; i++)
            {
                if (_entityManager.HasComponent<Disabled>(_projectilePool[i]))
                {
                    ActivateProjectile(i);
                    return;
                }
            }
        }

        private void ActivateProjectile(int index)
        {
            Entity proj = _projectilePool[index];
            float3 playerPos = float3.zero;
            if (!_gameHandler.GetPlayerPosition(ref playerPos)) return;
            
            _entityManager.SetComponentData(proj, new Translation
            {
                Value = playerPos
            });
            _entityManager.RemoveComponent<Disabled>(_projectilePool[index]);
        }

        private void SpawnProjectiles()
        {
            NativeArray<Entity> projectiles = new NativeArray<Entity>(10, Allocator.Temp);
            _entityManager.Instantiate(PrefabEntities.Entities[(int) PrefabEntity.Projectile], projectiles);
            
            for (int i = 0; i < projectiles.Length; i++)
            {
                Entity proj = projectiles[i];

                _entityManager.AddComponent<Disabled>(proj);

                _projectilePool.Add(proj);
            }

            projectiles.Dispose();
        }
    }
}