using System;
using Unity.Entities;
using UnityEngine;

namespace rabbit.Assets.Scripts
{
    public enum RabbitState
    {
        Sleep = 0,
        Warning = 1,
        Run = 2
    }

    public struct Rabbit : IComponentData
    {
        private RabbitState _state;
        public bool isDirty;
        public RabbitState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    isDirty = true;
                }
            }
        }
    }

    public struct RabbitMaterial : ISharedComponentData, IEquatable<RabbitMaterial>
    {
        public Material sleepMaterial;
        public Material warningMaterial;
        public Material runMaterial;

        public Material GetMaterial(RabbitState rabbitState)
        {
            switch (rabbitState)
            {
                case RabbitState.Run:
                    return runMaterial;
                case RabbitState.Warning:
                    return warningMaterial;
                default:
                    return sleepMaterial;
            }
        }

        public bool Equals(RabbitMaterial other)
        {
            return sleepMaterial == other.sleepMaterial &&
            warningMaterial == other.warningMaterial &&
            runMaterial == other.runMaterial;
        }

        public override int GetHashCode()
        {
            var hash1 = runMaterial ? runMaterial.GetHashCode() : 0;
            var hash2 = warningMaterial ? warningMaterial.GetHashCode() : 0;
            var hash3 = sleepMaterial ? sleepMaterial.GetHashCode() : 0;

            unchecked
            {
                const int FIRST_NUMBER = 17;
                const int SECOND_NUMBER = 23;

                int hash = FIRST_NUMBER;

                hash = hash * SECOND_NUMBER + hash1;
                hash = hash * SECOND_NUMBER + hash2;
                hash = hash * SECOND_NUMBER + hash3;
                return hash;
            }
        }
    }
}