using Unity.Entities;
using UnityEngine;

namespace rabbit.Assets.Scripts
{
    [ConverterVersion("df", 2)]
    public class RabbitConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Material sleepMaterial;
        public Material warningMaterial;
        public Material runMaterial;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var materials = new RabbitMaterial
            {
                sleepMaterial = sleepMaterial,
                warningMaterial = warningMaterial,
                runMaterial = runMaterial
            };

            dstManager.AddComponent<Rabbit>(entity);
            dstManager.AddSharedComponentData(entity, materials);
        }
    }
}