using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Main.Scripts
{
    [GenerateAuthoringComponent]
    public struct Comp_Movement : IComponentData
    {
        [NonSerialized] public float3 movementVector;
        public float movementSpeed;
    }
}