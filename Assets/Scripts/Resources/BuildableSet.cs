using System.Collections.Generic;
using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildableSet", order = 1)]
    public class BuildableSet : ScriptableObject
    {
        public List<Buildable> Buildables;
    }
}