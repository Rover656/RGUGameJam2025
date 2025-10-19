using System;
using UnityEngine;

namespace Game.Resources
{
    [Serializable]
    public class BuildableConfiguration
    {
        public Vector3 Rotation;
        public Vector3 Scale;

        public BuildableConfiguration(Vector3 rotation, Vector3 scale)
        {
            Rotation = rotation;
            Scale = scale;
        }
    }
}