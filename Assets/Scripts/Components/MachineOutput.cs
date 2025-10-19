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

        [CanBeNull] private Recipe _currentRecipe;
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

                if (nextSink is null) return;
                if (nextSink.AcceptItem(GetAbsoluteDirection(OutputDirection).Opposite(), itemToSend))
                {
                    _outputQueue.Dequeue();
                }
            }
            
            if (_currentRecipe is not null)
            {
                _recipeCounter += Time.deltaTime;

                if (_recipeCounter < _currentRecipe.SecondsToCraft) return;
                if (_outputQueue.Count >= BufferSize) return;
                
                _outputQueue.Enqueue(_currentRecipe.Result);
                _currentRecipe = null;
            }
            else
            {
                if (_outputQueue.Count >= BufferSize) return;
                
                var nextInput = Input.Take();
                if (nextInput is null) return;
                
                _currentRecipe = RecipeCollection.Recipes.FirstOrDefault(r => r.Input == nextInput);
                _recipeCounter = 0;

                if (CurrentContentsRenderer is not null)
                {
                    if (_currentRecipe is not null)
                    {
                        CurrentContentsRenderer.gameObject.SetActive(true);
                        CurrentContentsRenderer.sprite = _currentRecipe.Result.Sprite;
                    }
                    else
                    {
                        CurrentContentsRenderer.gameObject.SetActive(false);
                    }
                }

                if (_currentRecipe is null)
                {
                    _outputQueue.Enqueue(nextInput);
                }
            }
        }
    }
}