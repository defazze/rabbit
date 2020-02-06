using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace rabbit.Assets.Scripts
{
    public class InputSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<Wolf>().ForEach((ref Translation translation) =>
            {
                if (Input.GetKey(KeyCode.D))
                {
                    translation.Value.x += Time.DeltaTime * 3;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    translation.Value.x -= Time.DeltaTime * 3;
                }
            });

        }
    }
}