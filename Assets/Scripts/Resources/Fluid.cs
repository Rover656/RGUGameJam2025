using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Fluid", order = 1)]
    public class Fluid : ScriptableObject
    {
        public string description;
        public Color color;
    }
}
