using System;
using System.Collections.Generic;
using System.Linq;
using Game.Resources;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Components
{
    public class MachineOutput : GridMonoBehaviour
    {
        public int BufferSize;
        public Direction OutputDirection;
        public MachineInput Input;
        public RecipeCollection RecipeCollection;
        public SpriteRenderer CurrentContentsRenderer;

        [NonSerialized] [CanBeNull] public Recipe SelectedRecipe;

        private bool _isCrafting;
        private float _recipeCounter;

        private readonly Queue<Item> _outputQueue = new();

        private void Awake()
        {
            CurrentContentsRenderer?.gameObject.SetActive(false);
        }

        private void Update()
        {
            // Push output
            if (_outputQueue.TryPeek(out var itemToSend))
            {
                var neighbour = WorldGrid.GetNeighbor(gameObject, GetAbsoluteDirection(OutputDirection));
                var nextSink = neighbour?.GetComponent<IItemSink>();

                if (nextSink is not null && nextSink.AcceptItem(GetAbsoluteDirection(OutputDirection).Opposite(), itemToSend))
                {
                    _outputQueue.Dequeue();
                }
            }

            if (SelectedRecipe == null)
            {
                return;
            }
            
            if (CurrentContentsRenderer is not null)
            {
                if (SelectedRecipe is not null)
                {
                    CurrentContentsRenderer.gameObject.SetActive(true);
                    CurrentContentsRenderer.sprite = SelectedRecipe.Result.Sprite;
                }
                else
                {
                    CurrentContentsRenderer.gameObject.SetActive(false);
                }
            }

            if (!_isCrafting)
            {
                var ingredient = Input.Take();
                if (ingredient is not null)
                {
                    _isCrafting = true;
                    _recipeCounter = 0;
                }
                
                return;
            }
            
            _recipeCounter += Time.deltaTime;

            if (_recipeCounter < SelectedRecipe.SecondsToCraft) return;
            if (_outputQueue.Count >= BufferSize) return;
                
            _outputQueue.Enqueue(SelectedRecipe.Result);
            _recipeCounter = 0;
            _isCrafting = false;
        }
    }
}