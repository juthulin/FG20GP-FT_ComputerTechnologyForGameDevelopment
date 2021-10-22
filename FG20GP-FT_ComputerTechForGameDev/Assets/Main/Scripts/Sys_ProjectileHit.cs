using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Main.Scripts
{
    public class Sys_ProjectileHit : JobComponentSystem
    {
        private struct ProjectileTrigger : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<Tag_Enemy> TagEnemyComponentDataFromEntity;
            [ReadOnly] public ComponentDataFromEntity<Tag_Projectile> TagProjectileComponentDataFromEntity;
            public EntityCommandBuffer Ecb;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entA = triggerEvent.EntityA;
                Entity entB = triggerEvent.EntityB;
                
                Entity entProjectile = Entity.Null;
                Entity entEnemy = Entity.Null;

                if (TagEnemyComponentDataFromEntity.HasComponent(entA)) entEnemy = entA;
                else if (TagEnemyComponentDataFromEntity.HasComponent(entB)) entEnemy = entB;

                if (TagProjectileComponentDataFromEntity.HasComponent(entA)) entProjectile = entA;
                else if (TagProjectileComponentDataFromEntity.HasComponent(entB)) entProjectile = entB;

                if (entEnemy != Entity.Null && entProjectile != Entity.Null)
                {
                    Ecb.AddComponent<Disabled>(entProjectile);
                    Ecb.AddComponent(entEnemy, new Comp_TakeDamage
                    {
                        Value = 1
                    });
                }
            }
        }

        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;

        protected override void OnCreate()
        {
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ecb = _commandBufferSystem.CreateCommandBuffer();
            ProjectileTrigger projectileTrigger = new ProjectileTrigger
            {
                TagEnemyComponentDataFromEntity = GetComponentDataFromEntity<Tag_Enemy>(),
                TagProjectileComponentDataFromEntity = GetComponentDataFromEntity<Tag_Projectile>(),
                Ecb = ecb
            };
            JobHandle jobHandle = projectileTrigger.Schedule(
                _stepPhysicsWorld.Simulation,
                ref _buildPhysicsWorld.PhysicsWorld,
                inputDeps
            );
            _commandBufferSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }
    }
}