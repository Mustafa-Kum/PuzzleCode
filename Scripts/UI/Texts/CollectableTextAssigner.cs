using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.UI.Texts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CollectableCounterText : MonoBehaviour
    {
        [SerializeField]
        private CurrencyType textAssigner;

        [FormerlySerializedAs("collectableValuesSO")] [SerializeField]
        private CurrencyValuesSO currencyValuesSo;

        private TextMeshProUGUI textMesh;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
         
            UpdateText(textAssigner);
        }
        
        private void OnEnable()
        {
            //EventManager.CollectableEvents.UICollectAnimation += UpdateCounterText;
            EventManager.Resource.CollectableSpent += UpdateText;
            EventManager.Resource.CurrencyChanged += UpdateText;
        }

        private void OnDisable()
        {
            //EventManager.CollectableEvents.UICollectAnimation -= UpdateCounterText;
            EventManager.Resource.CollectableSpent -= UpdateText;
            EventManager.Resource.CurrencyChanged -= UpdateText;
        }
        
        private void UpdateText(CurrencyType currencyType)
        {
            if (currencyType == textAssigner)
            {
                int value = currencyValuesSo.GetValue(currencyType);
            
                textMesh.text = value.ToString();
            }
        }

        // private void UpdateCounterText(CollectableData collectableData)
        // {
        //     if (collectableData.Type == textAssigner)
        //     {
        //         int value = currencyValuesSo.GetValue(collectableData.Type);
        //     
        //         textMesh.text = value.ToString();
        //     }
        // }
    }
}
