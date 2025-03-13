using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools.Helpers;
using CodeBase.Tools.StaticDataLoader;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.LootBoxes
{
    public class LootBoxMediator : MonoBehaviour, IMediator
    {
        public event Action OnOpenLootBox;
        public event Action OnCloseLootBox;
        public event Action<IMediator> OnCleanUp;

        [Header("Components")] [SerializeField]
        private Transform _3dChestParent;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<SimpleButton> _openChestButtons;
        [SerializeField] private SimpleButton _collectButton;
        [SerializeField] private TextMeshProUGUI _tapToCollectText;
        [SerializeField] private Transform _targetContainer;
        [SerializeField] private Transform _imageContainer;
        [SerializeField] private float _durationScaleButton = 0.5f;
        [SerializeField] private float _durationCanvas = 2f;
        [SerializeField] private float _durationScaleImage = 0.3f;
        [SerializeField] private LootView _lootPrefab;

        [Header("Bounce")] [SerializeField] private float _bounceDuration = 0.25f;
        [SerializeField] private float _bounceModifier = 1.25f;

        private readonly List<LootView> _lootViews = new();
        private readonly List<Transform> _targetRewardsParents = new();
        private AssetProvider _assetProvider;
        private Coroutine _spawnRewardsRoutine;
        private Coroutine _clickOpenWaitRoutine;
        private Coroutine _tapChestHelperRoutine;
        private Coroutine _tapToCollectWaitRoutine;
        private Coroutine _flyRewardsRoutine;
        private LootBoxOpenView _currentChestView;
        private List<RewardDTO> _rewardDatas;
        private Action _doOnComplete;
        private LootBoxType _lootBoxType;

        public bool IsAdditiveMediator => false;
        public bool IsCameraSpace => true;
        public GameObject GameObject => gameObject;

        public void Construct(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _collectButton.OnClick += AnimateLootBoxWindowClose;
            _openChestButtons.ForEach(b => b.OnClick += ProvidePlayerTap);
            DisableLootBoxOpenHandler();
        }
        
        private void OnDestroy()
        {
            _collectButton.OnClick -= AnimateLootBoxWindowClose;
            _openChestButtons.ForEach(b => b.OnClick -= ProvidePlayerTap);
            OnCleanUp?.Invoke(this);
        }

        public void SetRewards(List<RewardDTO> rewardDatas, LootBoxType lootBoxType, Action onComplete = default)
        {
            _lootBoxType = lootBoxType;
            _rewardDatas = rewardDatas;
            _doOnComplete = onComplete;
        }

        public void Show()
        {
            _tapChestHelperRoutine.Stop(this);
            _3dChestParent.DestroyChildren();
            _currentChestView = Instantiate(_assetProvider.GetLootBoxPrefab(_lootBoxType), _3dChestParent);
            EnableLootBoxOpenHandler();
            _currentChestView.OnPlayerOpenChest += SpawnPlayerRewards;
            _currentChestView.Show(ShowTapToOpen);
            OnOpenLootBox?.Invoke();
            _clickOpenWaitRoutine.Stop(this);
            _clickOpenWaitRoutine = StartCoroutine(WaitAndForcedClickToOpen());
            _canvasGroup.Show();
        }

        public void Hide()
        {
            _canvasGroup.Hide();
        }

        private void DisableLootBoxOpenHandler()
        {
            HideCollectButton();
            HideTapToOpen();
            _collectButton.transform.localScale = Vector3.zero;
        }

        private void ProvidePlayerTap()
        {
            _tapChestHelperRoutine.Stop(this);
            _clickOpenWaitRoutine.Stop(this);
            _openChestButtons.ForEach(b => b.gameObject.SetActive(false));
            _currentChestView.PlayerTap();
        }

        private void SpawnPlayerRewards()
        {
            _currentChestView.OnPlayerOpenChest -= SpawnPlayerRewards;
            HideTapToOpen();
            _spawnRewardsRoutine.Stop(this);
            _spawnRewardsRoutine = StartCoroutine(SpawnRewardsRoutine());
        }

        private IEnumerator SpawnRewardsRoutine()
        {
            yield return new WaitForSecondsRealtime(.25f);
            foreach (var reward in _rewardDatas)
            {
                SpawnRewardUI(reward);
                yield return default;
            }

            yield return MoveRewardsToDemonstrationPositionsRoutine();
        }

        private void SpawnRewardUI(RewardDTO data)
        {
            var lootView = Instantiate(_lootPrefab, _imageContainer);
            lootView.Init(data, default);
            lootView.DOKill();
            lootView.RectTransform.anchoredPosition = Vector2.zero;
            lootView.transform.localScale = Vector3.zero;
            _lootViews.Add(lootView);
        }

        private IEnumerator MoveRewardsToDemonstrationPositionsRoutine()
        {
            for (var i = 0; i < _lootViews.Count; i++)
            {
                _lootPrefab.SetClearView();
                var instance = Instantiate(_lootPrefab, _targetContainer);
                _targetRewardsParents.Add(instance.transform);
            }

            yield return new WaitForSecondsRealtime(0.1f);

            for (var index = 0; index < _lootViews.Count; index++)
            {
                AnimateAppearance(_lootViews[index], index);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            yield return new WaitForSecondsRealtime(0.1f);
            
            ShowCollectButton();
        }

        private void AnimateAppearance(LootView lootView, int index)
        {
            lootView.transform.DOScale(Vector3.one, _durationScaleImage).SetUpdate(true);
            lootView.transform
                .DOMove(_targetRewardsParents[index].position, _durationScaleImage)
                .SetUpdate(true);
        }

        private IEnumerator AnimateCloseLootBoxWindow()
        {
            HideCollectButton();

            for (var i = _lootViews.Count - 1; i >= 0; i--)
            {
                _lootViews[i].transform.DOScale(Vector3.zero, _durationScaleImage).SetUpdate(true);
                //_flyService.Fly
                yield return new WaitForSecondsRealtime(.1f);
            }

            yield return new WaitForSecondsRealtime(0.2f);

            _currentChestView.Hide();
            _doOnComplete?.Invoke();
            _canvasGroup.Hide(_durationCanvas);
            DisableLootBoxOpenHandler();
            _lootViews.Clear();
            _targetRewardsParents.Clear();
            _imageContainer.DestroyChildren();
            OnCloseLootBox?.Invoke();
        }

        private void ShowCollectButton()
        {
            _tapToCollectText.transform.DOKill();
            _collectButton.transform.DOKill();
            _tapToCollectText.transform.BounceLoop(_bounceDuration, _bounceModifier);
            _tapToCollectText.FadeText(1, _bounceDuration);
            _collectButton.transform
                .DOScale(Vector3.one, _durationScaleButton)
                .SetUpdate(true)
                .SetLink(gameObject)
                .OnComplete(() => { _collectButton.SetInteractable(true); });
        }

        private void HideCollectButton()
        {
            _tapToCollectText.transform.DOKill();
            _collectButton.transform.DOKill();
            _tapToCollectWaitRoutine.Stop(this);

            _tapToCollectText.transform.ResetScaleTo1(_bounceDuration);
            _tapToCollectText.FadeText(0, _bounceDuration);

            _collectButton.transform
                .DOScale(Vector3.zero, _durationScaleButton)
                .SetUpdate(true)
                .SetLink(gameObject)
                .OnStart(() => { _collectButton.SetInteractable(false); });
        }

        private void EnableLootBoxOpenHandler() => _canvasGroup.Show();

        private void ShowTapToOpen() => _openChestButtons.ForEach(b => b.gameObject.SetActive(true));

        private void HideTapToOpen() => _openChestButtons.ForEach(b => b.gameObject.SetActive(false));

        private IEnumerator WaitAndForcedClickToOpen()
        {
            yield return new WaitForSecondsRealtime(5f);
            ProvidePlayerTap();
        }
        
        private void AnimateLootBoxWindowClose()
        {
            _flyRewardsRoutine.Stop(this);
            _flyRewardsRoutine = StartCoroutine(AnimateCloseLootBoxWindow());
        }
    }
}