using System;
using _Game.Scripts.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.UI.Texts
{
    public class RewardedBoosterTextAmountAssigner : MonoBehaviour
    {
        #region PUBLIC VARIBLES

        [FormerlySerializedAs("inGameBoosterAdvertiseButton")] public InGameBoosterRewardedButton inGameBoosterRewardedButton;
        
        public TextMeshProUGUI textAmount;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            textAmount.text = "+"+inGameBoosterRewardedButton.inGameRewardedAdData.rewardAmount.ToString();
        }

        #endregion
    }
}