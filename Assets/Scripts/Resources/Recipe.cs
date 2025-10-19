using System;

namespace Game.Resources
{
    [Serializable]
    public class Recipe
    {
        public Item Input;
        public Item Result;
        public float SecondsToCraft;
    }
}