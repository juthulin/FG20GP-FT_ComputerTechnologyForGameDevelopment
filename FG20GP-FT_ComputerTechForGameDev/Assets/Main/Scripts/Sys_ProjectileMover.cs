using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

namespace Main.Scripts
{
    public class Sys_ProjectileMover : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;

        protected override void OnCreate()
        {
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer.Concurrent concurrent = _commandBufferSystem.CreateCommandBuffer().ToConcurrent();
        
            float deltaTime = Time.DeltaTime;
            JobHandle jobHandle = Entities.WithAll<Tag_Projectile>().ForEach(
            (
                int entityInQueryIndex,
                Entity ent,
                ref Comp_Movement movementComp, 
                ref Translation translation
            ) => {
                    
                translation.Value += new float3(0f, 1f, 0f) 
                                    * movementComp.movementSpeed * deltaTime;

                if (translation.Value.y > 5f)
                {
                    concurrent.AddComponent<Disabled>(entityInQueryIndex, ent);
                }
                    
            }).Schedule(inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}