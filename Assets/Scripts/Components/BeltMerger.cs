using System;
using System.Collections.Generic;
using Game.Resources;
using JetBrains.Annotations;

namespace Game.Components
{
    public class BeltMerger : GridMonoBehaviour, IItemSink
    {
        public int BufferSize = 1;
        
        public Direction InputADirection;
        public Direction InputBDirection;
        public Direction OutputDirection;
        
        private Queue<Item> _inputQueue = new();
        private Direction? _lastReceivedDirection;

        private void Update()
        {
            if (!_inputQueue.TryPeek(out var itemToSend)) return;
            var neighbour = WorldGrid.GetNeighbor(gameObject, GetAbsoluteDirection(OutputDirection));
            var nextSink = neighbour?.GetComponent<IItemSink>();

            if (nextSink is null) return;
            if (nextSink.AcceptItem(GetAbsoluteDirection(OutputDirection).Opposite(), itemToSend))
            {
                _inputQueue.Dequeue();
            }
        }

        public bool AcceptItem(Direction fromSide, Item stack)
        {
            if (fromSide != GetAbsoluteDirection(InputADirection) &&
                fromSide != GetAbsoluteDirection(InputBDirection))
            {
                return false;
            }

            // if (_lastReceivedDirection == fromSide)
            // {
            //     return false;
            // }

            if (_inputQueue.Count >= BufferSize)
            {
                return false;
            }
            
            _inputQueue.Enqueue(stack);
            _lastReceivedDirection = fromSide;
            return true;
        }
    }
}