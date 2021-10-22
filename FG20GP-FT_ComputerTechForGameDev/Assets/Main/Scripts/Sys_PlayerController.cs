using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

namespace Main.Scripts
{

[UpdateAfter(typeof(Sys_PlayerInput))]
    public class Sys_PlayerController : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            return Entities.WithAll<Tag_Player>().ForEach(
                (
                    ref Comp_Movement movementComp, 
                    ref Translation translation,
                    ref LocalToWorld localToWorld
                ) => {
                    
                    Vector3 movement = new Vector3(movementComp.movementVector.x, 
                        movementComp.movementVector.y, 
                        0f);
                    
                    float3 appliedMovement = (float3)movement.normalized 
                                         * movementComp.movementSpeed * deltaTime;

                    float maxX = 8.5f;
                    float maxY = 4.8f;
                    
                    if (localToWorld.Position.x > maxX)
                    {
                        translation.Value.x = maxX;
                        // appliedMovement.x = 0f;
                    }
                    if (localToWorld.Position.x < -maxX)
                    {
                        translation.Value.x = -maxX;
                        // appliedMovement.x = 0f;
                    }
                    if (localToWorld.Position.y > maxY)
                    {
                        translation.Value.y = maxY;
                        // appliedMovement.y = 0f;
                    }
                    if (localToWorld.Position.y < -maxY)
                    {
                        translation.Value.y = -maxY;
                        // appliedMovement.y = 0f;
                    }

                    translation.Value += appliedMovement;

                }).Schedule(inputDeps);
        }
    }
}