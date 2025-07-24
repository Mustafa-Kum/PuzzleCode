using System;
using UnityEngine;

namespace _Game.Scripts.UI.Buttons
{
    [Serializable]
    public class MainMenuLevelDataProvider
    {
        public GameObject tutorialImage;
        public GameObject hardLevelImage;
        //public GameObject mediumLevelImage;
        public int levelIndex;
        
        public MainMenuLevelDataProvider(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }
    }
}