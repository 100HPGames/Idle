using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
	[RequireComponent(typeof(Button))]
    public class SimpleButton : MonoBehaviour
    {
        public event Action OnClick;

        [SerializeField] protected Button _button;
        [SerializeField] private bool _isInteractable = true;

        private void Awake() => _button.onClick.AddListener(SendClickEvent);
        private void OnDestroy() => _button.onClick.RemoveListener(SendClickEvent);

        protected virtual void SendClickEvent()
        {
            if (_isInteractable)
                OnClick?.Invoke();
        }

        public void SetInteractable(bool state) => _isInteractable = state;
    }
}