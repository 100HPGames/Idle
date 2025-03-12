using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class PlayerPlankView: MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _healthTxt;
        [SerializeField] private TMP_Text _manaTxt;

        public void Init(float health, float mana)
        {
            _healthTxt.text = "Health: "+health;
            _manaTxt.text= "Mana: "+ mana;
        }
    }
}