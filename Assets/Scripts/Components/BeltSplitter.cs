using System;
using System.Collections.Generic;
using Game.Resources;
using JetBrains.Annotations;

namespace Game.Components
{
    public class BeltSplitter : GridMonoBehaviour, IItemSink
    {
        public int BufferSize = 4;
        
        public Direction InputDirection;
        public Direction OutputADirection;
        public Direction OutputBDirection;

        // [CanBeNull] public Item OutputAFilter;
        
        private Queue<Item> _inputQueue = new();
        private int _lastOutputSide;

        private void Update()
        {
            if (!_inputQueue.TryPeek(out var itemToSend)) return;
            
            if (_lastOutputSide == 0)
            {
                if (TrySend(OutputADirection, itemToSend))
                {
                    _inputQueue.Dequeue();
                    _lastOutputSide = 1;
                    return;
                }
            }
            
            if (TrySend(OutputBDirection, itemToSend))
            {
                _inputQueue.Dequeue();
                _lastOutputSide = 0;
            }
        }

        private bool TrySend(Direction direction, Item item)
        {
            var neighbour = WorldGrid.GetNeighbor(gameObject, GetAbsoluteDirection(direction));
            var nextSink = neighbour?.GetComponent<IItemSink>();

            if (nextSink is null) return false;
            return nextSink.AcceptItem(GetAbsoluteDirection(direction).Opposite(), item);
        }

        public bool AcceptItem(Direction fromSide, Item stack)
        {
            if (fromSide != GetAbsoluteDirection(InputDirection))
            {
                return false;
            }

            if (_inputQueue.Count >= BufferSize)
            {
                return false;
            }
            
            _inputQueue.Enqueue(stack);
            return true;
        }
    }
}