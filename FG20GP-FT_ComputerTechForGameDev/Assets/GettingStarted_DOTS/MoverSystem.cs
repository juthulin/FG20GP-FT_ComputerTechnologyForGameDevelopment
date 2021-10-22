using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace GettingStarted_DOTS
{
    public class MoverSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveSpeedComponent speedComponent) =>
            {
                translation.Value.y += speedComponent.moveSpeed * Time.DeltaTime;
                if (translation.Value.y > 5f)
                {
                    speedComponent.moveSpeed = -math.abs(speedComponent.moveSpeed);
                }
                if (translation.Value.y < -5f)
                {
                    speedComponent.moveSpeed = +math.abs(speedComponent.moveSpeed);
                }
            });
        }
    }
}