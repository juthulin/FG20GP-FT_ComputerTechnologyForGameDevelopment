using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

namespace Main.Scripts
{
    public class Sys_PlayerInput : JobComponentSystem
    {
        // public event EventHandler OnPlayerMoveRight;
        // public event EventHandler OnPlayerMoveUp;
        // public event EventHandler OnPlayerShoot;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            bool moveRightInput = Input.GetKey(KeyCode.D);
            bool moveLeftInput = Input.GetKey(KeyCode.A);
            bool moveUpInput = Input.GetKey(KeyCode.W);
            bool moveDownInput = Input.GetKey(KeyCode.S);
            // bool shootInput = Input.GetKey(KeyCode.Mouse0);

            // if (moveRightInput)
            // {
            //     OnPlayerMoveRight?.Invoke(this, EventArgs.Empty);
            // }
            //
            // if (moveUpInput)
            // {
            //     OnPlayerMoveUp?.Invoke(this, EventArgs.Empty);
            // }
            //
            // if (shootInput)
            // {
            //     OnPlayerShoot?.Invoke(this, EventArgs.Empty);
            // }

            return Entities.WithAll<Tag_Player>().ForEach((ref Comp_Movement movementComp) =>
            {
                if (moveRightInput ^ moveLeftInput) // xor
                    movementComp.movementVector.x = moveRightInput ? 1f : -1f;
                else
                    movementComp.movementVector.x = 0f;

                if (moveUpInput ^ moveDownInput) // xor
                    movementComp.movementVector.y = moveUpInput ? 1f : -1f;
                else
                    movementComp.movementVector.y = 0f;

            }).Schedule(inputDeps);
        }
    }
}