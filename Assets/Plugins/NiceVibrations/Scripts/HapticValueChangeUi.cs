using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Tools
{
    public class HapticValueChangeUi : MonoBehaviour
    {
        public event Action<HapticValueChangeUi, HapticCheatActionType> OnClick;
        
        [SerializeField] private Text _view;
        [SerializeField] private Button _addTime;
        [SerializeField] private Button _removeTime;
        [SerializeField] private Button _addPower;
        [SerializeField] private Button _removePower;
        [SerializeField] private Button _test;
        
        private void Awake()
        {
            _addTime.onClick.AddListener(()=> OnClick.Invoke(this, HapticCheatActionType.AddTime));
            _removeTime.onClick.AddListener(()=> OnClick.Invoke(this, HapticCheatActionType.RemoveTime));
            _addPower.onClick.AddListener(()=> OnClick.Invoke(this, HapticCheatActionType.AddPower));
            _removePower.onClick.AddListener(()=> OnClick.Invoke(this, HapticCheatActionType.RemovePower));
            _test.onClick.AddListener(()=> OnClick.Invoke(this, HapticCheatActionType.Test));
        }

        public void SetView(string text) => _view.text = text;
    }

    public enum HapticCheatActionType
    {
        AddTime,
        RemoveTime,
        AddPower,
        RemovePower,
        Test
    }
}