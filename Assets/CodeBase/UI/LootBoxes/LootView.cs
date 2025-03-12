using CodeBase.Tools.StaticDataLoader;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChestUI
{
    public class LootView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [field: SerializeField] public RectTransform RectTransform { get; set; }

        public void Init(RewardDTO data, Sprite image)
        {
            _image.color = Color.white;
        }

        public void SetClearView()
        {
            _image.color = Color.clear;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}