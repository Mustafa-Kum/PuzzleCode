using System;
using UnityEngine;
using TMPro;
using _Game.Scripts.ScriptableObjects.Saveable;

namespace _Game.Scripts.UI.Buttons
{
    public class MainMenuLevel : MonoBehaviour
    {
        [SerializeField] private MainMenuLevelDataProvider levelDataProvider;
        [SerializeField] private PlayerSavableData playerVariables;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private int[] tutorialLevels = new int[] { 6, 11 };

        private void OnEnable()
        {
            InitializeLevel();
            UpdateLevelText();
        }

        private void InitializeLevel()
        {
            int childIndex = transform.GetSiblingIndex();

            int level = playerVariables.LevelIndex + childIndex; 
    
            levelDataProvider.levelIndex = level;
    
            SetLevelImageActiveStatus(level);
        }


        private void SetLevelImageActiveStatus(int level)
        {
            bool isTutorialLevel = Array.IndexOf(tutorialLevels, level) >= 0;
            levelDataProvider.tutorialImage.SetActive(isTutorialLevel);

            bool isHardLevel = level % 9 == 0;
            levelDataProvider.hardLevelImage.SetActive(isHardLevel);

            //bool isMediumLevel = (level % 4 == 0) && !isHardLevel;
            //levelDataProvider.mediumLevelImage.SetActive(isMediumLevel);
            
            if (isTutorialLevel)
            {
                levelDataProvider.hardLevelImage.gameObject.SetActive(false);
                //levelDataProvider.mediumLevelImage.gameObject.SetActive(false);
            }
            
            if (isHardLevel)
            {
                //levelDataProvider.tutorialImage.gameObject.SetActive(false);
                //levelDataProvider.mediumLevelImage.gameObject.SetActive(false);
            }
        }

        private void UpdateLevelText()
        {
            levelText.text = $"Level {levelDataProvider.levelIndex + 1}";
        }
    }
}