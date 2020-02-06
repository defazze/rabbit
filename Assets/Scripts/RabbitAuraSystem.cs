using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace rabbit.Assets.Scripts
{
    public unsafe class RabbitAuraSystem : JobComponentSystem
    {

        private BuildPhysicsWorld _buildPhysicsWorldSystem;
        private StepPhysicsWorld _stepPhysicsWorldSystem;
        private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
        struct TriggerAuraJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<Wolf> wolfGroup;
            [ReadOnly] public ComponentDataFromEntity<Rabbit> rabbitGroup;
            [ReadOnly] public ComponentDataFromEntity<PhysicsCollider> colliderGroup;
            public EntityCommandBuffer commandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                var entityA = triggerEvent.Entities.EntityA;
                var entityB = triggerEvent.Entities.EntityB;

                var rabbitEntity = rabbitGroup.Exists(entityA) ? entityA : rabbitGroup.Exists(entityB) ? entityB : Entity.Null;
                var wolfEntity = wolfGroup.Exists(entityA) ? entityA : wolfGroup.Exists(entityB) ? entityB : Entity.Null;

                if (rabbitEntity == Entity.Null || wolfEntity == Entity.Null)
                {
                    return;
                }

                var rabbitColliderKey = rabbitEntity == entityA ? triggerEvent.ColliderKeys.ColliderKeyA : triggerEvent.ColliderKeys.ColliderKeyB;
                colliderGroup[rabbitEntity].Value.Value.GetChild(ref rabbitColliderKey, out var childCollider);
                var belongsTo = childCollider.Collider->Filter.BelongsTo;

                var rabbit = rabbitGroup[rabbitEntity];
                if (IsLayerExist(belongsTo, (int)CategoryNames.InternalAura))
                {
                    rabbit.State = RabbitState.Run;
                    commandBuffer.SetComponent(rabbitEntity, rabbit);
                }
                else if (IsLayerExist(belongsTo, (int)CategoryNames.ExternalAura))
                {
                    rabbit.State = RabbitState.Warning;
                    commandBuffer.SetComponent(rabbitEntity, rabbit);
                }
            }

            private bool IsLayerExist(uint belongsTo, int layer)
            {
                return (belongsTo & ((uint)1 << layer)) != 0;
            }
        }

        protected override void OnCreate()
        {
            _buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
            _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var wolfGroup = GetComponentDataFromEntity<Wolf>();
            var rabbitGroup = GetComponentDataFromEntity<Rabbit>();
            var colliderGroup = GetComponentDataFromEntity<PhysicsCollider>();

            var jobHandle = new TriggerAuraJob
            {
                wolfGroup = wolfGroup,
                rabbitGroup = rabbitGroup,
                colliderGroup = colliderGroup,
                commandBuffer = _commandBufferSystem.CreateCommandBuffer()
            }.Schedule(_stepPhysicsWorldSystem.Simulation,
                    ref _buildPhysicsWorldSystem.PhysicsWorld, inputDeps);

            jobHandle.Complete();
            return jobHandle;
        }
    }
}