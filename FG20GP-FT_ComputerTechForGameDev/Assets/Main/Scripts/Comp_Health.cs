using Unity.Entities;

namespace Main.Scripts
{
    [GenerateAuthoringComponent]
    public struct Comp_Health : IComponentData
    {
        public int Health;
    }
}