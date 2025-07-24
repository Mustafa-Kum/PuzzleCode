using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

namespace _Game.Scripts._GameLogic
{
    public class ButtonScaler : MonoBehaviour
    {
        [SerializeField] private List<Image> _allImages;
        [SerializeField] private List<TextMeshProUGUI> _allTexts;
        [SerializeField] private List<GameObject> _allGameObjects;
        [SerializeField] private ButtonImageSO _buttonImageSo;

        private Dictionary<Image, TextMeshProUGUI> _imageToTextMapping = new Dictionary<Image, TextMeshProUGUI>();
        private Image _selectedImage = null;

        private void Awake()
        {
            InitializeImageToTextMapping();
            RegisterButtonListeners();
            
            // Simulate button click for the middle element
            int middleIndex = _allImages.Count / 2;
            OnButtonClicked(middleIndex);
        }

        private void InitializeImageToTextMapping()
        {
            int count = Mathf.Min(_allImages.Count, _allTexts.Count);
            for (int i = 0; i < count; i++)
            {
                _imageToTextMapping[_allImages[i]] = _allTexts[i];
            }
        }

        private void RegisterButtonListeners()
        {
            for (int i = 0; i < _allImages.Count; i++)
            {
                int index = i;
                var button = _allImages[i].GetComponentInParent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnButtonClicked(index));
                }
            }
        }

        private void OnButtonClicked(int index)
        {
            Image clickedImage = _allImages[index];
            if (_selectedImage == clickedImage) return;

            for (int i = 0; i < _allImages.Count; i++)
            {
                bool isSelected = i == index;
                Transform imgTransform = _allImages[i].transform;
                imgTransform.DOScale(Vector3.one * (isSelected ? 1.3f : 1f), 0.1f).SetEase(Ease.Linear);
                imgTransform.DOLocalMove(Vector3.up * (isSelected ? 100f : 10f), 0.1f);

                if (_imageToTextMapping.TryGetValue(_allImages[i], out var text))
                {
                    text.gameObject.SetActive(isSelected);
                }
                
                if(isSelected && index < _allGameObjects.Count)
                {
                    Transform goTransform = _allGameObjects[index].transform;
                    goTransform.DOScale(Vector3.one * 1.3f, 0.1f).SetEase(Ease.Linear);
                    goTransform.DOLocalMove(Vector3.up * 85f, 0.1f);
                }
                else if(index < _allGameObjects.Count)
                {
                    Transform goTransform = _allGameObjects[i].transform;
                    goTransform.DOScale(Vector3.one, 0.1f);
                }

                if (isSelected)
                {
                    _selectedImage = clickedImage;
                    clickedImage.transform.parent.GetComponentInParent<Image>().sprite = _buttonImageSo.ClickedImage;
                    clickedImage.transform.parent.GetComponentInParent<Image>().color = Color.white;
                }
                else
                {
                    _allImages[i].transform.parent.GetComponentInParent<Image>().sprite = _buttonImageSo.DefaultImage;
                    _allImages[i].transform.parent.GetComponentInParent<Image>().color = new Color(28f / 255f, 46f / 255f, 122f / 255f, 1f);
                }
            }
        }
    }
}
