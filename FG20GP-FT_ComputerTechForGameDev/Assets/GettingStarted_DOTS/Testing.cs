using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using Random = UnityEngine.Random;

namespace GettingStarted_DOTS
{
    public class Testing : MonoBehaviour
    {
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;

        private void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            EntityArchetype testArchetype = entityManager.CreateArchetype(
                typeof(TestLevelComponent),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(MoveSpeedComponent)
            );
            NativeArray<Entity> entArray = new NativeArray<Entity>(10_000, Allocator.Temp);
            entityManager.CreateEntity(testArchetype, entArray);

            for (int i = 0; i < entArray.Length; i++)
            {
                Entity ent = entArray[i];
                
                entityManager.SetComponentData(ent, new TestLevelComponent
                {
                    level = Random.Range(0f, 20f)
                });
                entityManager.SetComponentData(ent, new MoveSpeedComponent
                {
                    moveSpeed = Random.Range(1f, 3f)
                });
                entityManager.SetComponentData(ent, new Translation
                {
                    Value = new float3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f)
                });

                entityManager.SetSharedComponentData(ent, new RenderMesh
                {
                    mesh = this.mesh,
                    material = this.material
                });
            }

            entArray.Dispose();
        }
    }
}