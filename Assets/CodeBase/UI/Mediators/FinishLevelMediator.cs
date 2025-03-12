using System;
using CodeBase.Components.Interactions;
using CodeBase.Infrastructure;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.SimpleMessenger;
using MoneyHandler;
using UnityEngine;

namespace CodeBase.UI.Mediators
{
    public class FinishLevelMediator : MonoBehaviour, IMediator
    {
        public event Action<IMediator> OnCleanUp;
        public event Action OnClickEnd;

        [Header("Components")] [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private EndLevelView _winLevelView;
        [SerializeField] private EndLevelView _looseLevelView;
        [SerializeField] private SimpleButton _endButton;

        private Messenger _messenger;
        private MediatorFactory _mediatorFactory;
        private CurrencyService _currencyService;

        public bool IsAdditiveMediator { get; set; } = false;
        public bool IsCameraSpace { get; }
        public GameObject GameObject => gameObject;
        public void Show() => _canvasGroup.Show();
        public void Hide() => _canvasGroup.Hide();

        public void Construct(Messenger messenger, MediatorFactory mediatorFactory, CurrencyService currencyService)
        {
            _mediatorFactory = mediatorFactory;
            _messenger = messenger;
            _currencyService = currencyService;
            _looseLevelView.Hide();
            _winLevelView.Hide();
        }
        
        public void OpenLooseView(string locationName, int soft, int hard)
        {
            _winLevelView.Hide();
            _looseLevelView.Init(locationName, soft, hard);
            _looseLevelView.Show();
        }

        public void OpenWinView(string locationName, int soft, int hard)
        {
            _looseLevelView.Hide();
            _winLevelView.Init(locationName, soft, hard);
            _winLevelView.Show();
        }
        
        private void Awake() => _endButton.OnClick += ClickEnd;

        private void OnDestroy() => _endButton.OnClick -= ClickEnd;

        private void ClickEnd() => OnClickEnd?.Invoke();
    }
}