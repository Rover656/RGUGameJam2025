using System.Collections.Generic;
using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Recipe Collection", order = 1)]
    public class RecipeCollection : ScriptableObject
    {
        public List<Recipe> Recipes;
    }
}