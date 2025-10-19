using System;
using System.Collections.Generic;
using System.Linq;
using Game.Resources;
using UnityEngine;

namespace Game.Components
{
    public class Conveyor : GridMonoBehaviour, IItemSink
    {
        public const float SecondsPerItem = 1;
        public const int MaxItems = 4;
        
        public Direction InputDirection;
        public Direction OutputDirection;

        public GameObject ItemRendererPrefab;
        
        private List<ConveyorItem> _items = new();
        private List<GameObject> _itemRenderers = new();

        private void Start()
        {
            for (int i = 0; i < MaxItems; i++)
            {
                var renderer = Instantiate(ItemRendererPrefab);
                renderer.transform.SetParent(gameObject.transform);
                renderer.SetActive(false);
                _itemRenderers.Add(renderer);
            }
        }

        private void Update()
        {
            float progressBy = Time.deltaTime / SecondsPerItem;

            for (var i = 0; i < MaxItems; i++)
            {
                if (_items.Count > i)
                {
                    // Ensure items are equally spread.
                    float maximumProgression = (MaxItems - i) / (float)MaxItems;

                    var item = _items[i];
                    item.Progress = Mathf.Min(1, MathF.Min(maximumProgression, item.Progress + progressBy));
                
                    // Update item renderer
                    // TODO: Probably want to cache the component.
                    _itemRenderers[i].SetActive(true);
                    _itemRenderers[i].GetComponent<SpriteRenderer>().sprite = item.Item.Sprite;

                    if (item.Progress < 0.5f)
                    {
                        _itemRenderers[i].transform.localPosition = GetItemOffset(item.Progress, InputDirection.Opposite());                        
                    }
                    else
                    {
                        _itemRenderers[i].transform.localPosition = GetItemOffset(item.Progress, OutputDirection);
                    }
                }
                else
                {
                    _itemRenderers[i].SetActive(false);
                }
            }

            // Highest progress first.
            _items.Sort((a, b) => b.Progress.CompareTo(a.Progress));

            // Scan for neighbours
            var itemToSend = _items.SingleOrDefault(i => i.Progress >= 1f);
            if (itemToSend is null) return;
            
            var neighbour = WorldGrid.GetNeighbor(gameObject, GetAbsoluteDirection(OutputDirection));
            var nextSink = neighbour?.GetComponent<IItemSink>();

            if (nextSink is null) return;
            if (nextSink.AcceptItem(GetAbsoluteDirection(OutputDirection).Opposite(), itemToSend.Item))
            {
                _items.Remove(itemToSend);
            }
        }

        private Vector3 GetItemOffset(float progress, Direction targetDirection)
        {
            int xOffset = targetDirection.GetXOffset();
            int yOffset = targetDirection.GetYOffset();

            if (xOffset != 0)
            {
                return new Vector3((progress - 0.5f) * xOffset, 0, 0);
            }

            return new Vector3(0, (progress - 0.5f) * yOffset, 0);
        }

        private class ConveyorItem
        {
            public Item Item;
            public float Progress;
        }

        public bool AcceptItem(Direction fromSide, Item stack)
        {
            if (fromSide != GetAbsoluteDirection(InputDirection))
            {
                return false;
            }
            
            if (_items.Count >= MaxItems)
            {
                return false;
            }

            if (_items.Any(i => i.Progress < 1 / (float)MaxItems))
            {
                return false;
            }

            _items.Add(new ConveyorItem { Item = stack });
            return true;
        }
    }
}