using TMPro;
using UnityEngine;

namespace CodeBase.UI.FinishedView
{
    public class EndLevelView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _locationName;
        [SerializeField] private TMP_Text _softAmount;
        [SerializeField] private TMP_Text _shardsAmount;

        public void Init(string locationName, int softAmount, int shardsAmount)
        {
            _locationName.text = locationName;
            _softAmount.text = softAmount.ToString();
            _shardsAmount.text = shardsAmount.ToString();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}