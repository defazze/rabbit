using Unity.Entities;
using Unity.Rendering;

namespace rabbit.Assets.Scripts
{
    public class ChangeMaterialSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, RabbitMaterial rabbitMaterial, ref Rabbit rabbit) =>
            {
                if (rabbit.isDirty)
                {
                    rabbit.isDirty = false;

                    var render = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                    render.material = rabbitMaterial.GetMaterial(rabbit.State);
                    EntityManager.SetSharedComponentData(entity, render);
                }
            });

        }
    }
}