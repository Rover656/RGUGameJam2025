using System.Collections.Generic;
using Game.Resources;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Components
{
    public class MachineInput : GridMonoBehaviour, IItemSink
    {
        public int BufferSize = 16;
        public Direction InputDirection;
        public SpriteRenderer CurrentContentsRenderer;
        
        private readonly Queue<Item> _inputs = new();

        void Awake()
        {
            CurrentContentsRenderer?.gameObject.SetActive(false);
        }
        
        public bool AcceptItem(Direction fromSide, Item stack)
        {
            if (fromSide != GetAbsoluteDirection(InputDirection))
            {
                return false;
            }

            if (_inputs.Count >= BufferSize)
            {
                return false;
            }
            
            _inputs.Enqueue(stack);

            if (_inputs.Count == 1 && CurrentContentsRenderer is not null)
            {
                CurrentContentsRenderer.gameObject.SetActive(true);
                CurrentContentsRenderer.sprite = _inputs.Peek().Sprite;
            }
            
            return true;
        }
        
        [CanBeNull]
        public Item Take()
        {
            if (_inputs.Count == 0)
                return null;
            
            if (CurrentContentsRenderer is not null)
            {
                CurrentContentsRenderer.gameObject.SetActive(true);
                CurrentContentsRenderer.sprite = _inputs.Peek().Sprite;
            }
            return _inputs.Dequeue();
        }
    }
}