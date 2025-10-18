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
    }
}