using System;
using Components;
using Contexts;
using TMPro;
using UnityEngine;

namespace Systems.GamePlay
{
    public class ScoreSystem : MonoBehaviour, ISystem
    {
        private UISystem _uiSystem;
        private Context _gameContext;
        private TMP_Text scoreContainer;
        protected internal ScoreComponent scoreComponent;

        public void Initialize(UISystem uiSystem, Context gameContext)
        {
            //Assign variables
            _uiSystem = uiSystem;
            _gameContext = gameContext;
            
            //Setup an Entity
            scoreComponent = gameObject.AddComponent<ScoreComponent>();
            scoreComponent.Score = 0;
            //Get the uiInstance
            var uiInstance = _uiSystem.UiInstance;
            //Set the scoreContainer to 0;
            //Debug.Log($"child: {uiInstance.transform.GetChild(0).name}");
            scoreContainer = uiInstance.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TMP_Text>();//This gets the firstChild then the firstGrandChild
            scoreContainer.text = scoreComponent.Score.ToString();
            EntitySystem.RegisterSystem(GameContexts.UI, this);
        }

        private void Update()
        {
            scoreContainer.text = scoreComponent.Score.ToString();
        }
    }
}