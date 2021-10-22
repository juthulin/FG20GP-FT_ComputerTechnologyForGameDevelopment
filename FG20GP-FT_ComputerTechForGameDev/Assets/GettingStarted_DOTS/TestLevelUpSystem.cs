using Unity.Entities;

namespace GettingStarted_DOTS
{
    public class TestLevelUpSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref TestLevelComponent levelComponent) =>
            {
                levelComponent.level += 1f * Time.DeltaTime;
            });
        }
    }
}