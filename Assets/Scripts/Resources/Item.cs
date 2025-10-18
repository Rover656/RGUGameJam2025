using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public string Description;
        public Sprite Sprite;
        public int CompletedValue;
    }
}