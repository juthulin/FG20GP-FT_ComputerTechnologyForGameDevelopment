using Unity.Entities;
using Unity.Jobs;

namespace Main.Scripts
{
    public class Sys_DealDamage : JobComponentSystem
    {
        // private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        private BeginSimulationEntityCommandBufferSystem _commandBufferSystem;
        
        protected override void OnCreate()
        {
            // _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            _commandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ecb = _commandBufferSystem.CreateCommandBuffer().ToConcurrent();
            
            JobHandle jobHandle = Entities.WithAll<Comp_TakeDamage>().ForEach((
                Entity ent,
                int entityInQueryIndex,
                ref Comp_Health healthComp
            ) =>
            {
                ecb.RemoveComponent<Comp_TakeDamage>(entityInQueryIndex, ent);

                healthComp.Health -= 1;
                if (healthComp.Health <= 0)
                {
                    ecb.AddComponent<Disabled>(entityInQueryIndex, ent);
                    // ecb.DestroyEntity(entityInQueryIndex, ent);
                }

            }).Schedule(inputDeps);
            _commandBufferSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }
    }
}