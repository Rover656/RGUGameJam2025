using System.Collections.Generic;
using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Buildable", order = 1)]
    public class Buildable : ScriptableObject
    {
        public Sprite Preview;
        public GameObject Prefab;
        public string Description;
        public int Cost;

        public List<BuildableConfiguration> Configurations = new List<BuildableConfiguration>()
        {
            new(new Vector3(0, 0, 0), new Vector3(1, 1, 1)),
            new(new Vector3(0, 0, 90), new Vector3(1, 1, 1)),
            new(new Vector3(0, 0, 180), new Vector3(1, 1, 1)),
            new(new Vector3(0, 0, 270), new Vector3(1, 1, 1)),
        };
    }
}