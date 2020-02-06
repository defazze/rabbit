using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace rabbit.Assets.Scripts
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class RabbitMovementSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {

            Entities.ForEach((Entity e, ref Translation translation, ref Rabbit rabbit) =>
            {
                if (rabbit.State == RabbitState.Run)
                {
                    translation.Value.x += Time.DeltaTime * 3.5f;
                }

            });
        }
    }
}