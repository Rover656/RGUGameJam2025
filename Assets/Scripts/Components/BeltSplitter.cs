using System;
using System.Collections.Generic;
using Game.Resources;
using JetBrains.Annotations;

namespace Game.Components
{
    public class BeltSplitter : GridMonoBehaviour, IItemSink
    {
        public int BufferSize = 4;
        private Queue<Item> _inputQueue = new();
        
        public Direction InputDirection;
        public Direction OutputADirection;
        public Direction OutputBDirection;

        [CanBeNull] public Item OutputAFilter;

        private void Update()
        {
            
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