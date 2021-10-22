using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

namespace Main.Scripts
{
    public class EnemyHandler : GameHandlerComponent
    {
        private readonly GameHandler _gameHandler = null;
        
        public EnemyHandler(in GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }
        public void Init()
        {
            EntityManager entManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            NativeArray<Entity> enemyEntities = new NativeArray<Entity>(_gameHandler.enemiesToSpawn, Allocator.Temp);
            entManager.Instantiate(PrefabEntities.Entities[(int)PrefabEntity.Enemy], enemyEntities);
            
            for (int i = 0; i < enemyEntities.Length; i++)
            {
                Entity enemy = enemyEntities[i];
                
                entManager.SetComponentData(enemy, new Translation
                {
                    Value = new float3(
                        UnityEngine.Random.Range(-8f, 8f),
                        10f,
                        0f
                    )
                });
                entManager.SetComponentData(enemy, new Rotation
                {
                    Value = UnityEngine.Quaternion.Euler(0f, 0f, 180f)
                });
            }
            
            enemyEntities.Dispose();
        }

        public void Tick() { }
    }
}