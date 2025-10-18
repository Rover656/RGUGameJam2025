using System.Collections.Generic;
using Game.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Components
{
    public class ItemGenerator : GridMonoBehaviour
    {
        public List<Item> Items;
        public float SecondsPerItem;

        public Direction OutputDirection;
        
        private float _generationCounter;
        private Queue<Item> _generated = new();

        private void Update()
        {
            _generationCounter += Time.deltaTime;
            
            if (_generationCounter >= SecondsPerItem)
            {
                _generationCounter -= SecondsPerItem;
                _generated.Enqueue(Items[Random.Range(0, Items.Count)]);
            }

            if (_generated.Count == 0) return;
            
            var itemToSend = _generated.Peek();
            if (itemToSend is null) return;
            
            var neighbour = WorldGrid.GetNeighbor(gameObject, GetAbsoluteDirection(OutputDirection));
            var nextSink = neighbour?.GetComponent<IItemSink>();

            if (nextSink is null) return;
            if (nextSink.AcceptItem(GetAbsoluteDirection(OutputDirection).Opposite(), itemToSend))
            {
                _generated.Dequeue();
            }
        }
    }
}