using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.LootBoxes
{
	public class LootBoxOpenView : MonoBehaviour
	{
		public event Action OnPlayerOpenChest;

		[Header("Components")]
		[SerializeField] private Animator _animator;
		[SerializeField] private Transform _chestTransform;

		[Header("Show Chest Data")]
		[SerializeField] private float _chestTopY;
		[SerializeField] private float _chestDownY;
		[SerializeField] private float _chestHideDownY = -15f;
		[SerializeField] private Ease _easeStart;
		[SerializeField] private LootBoxType _lootBoxType;
		
		private readonly int _idle = Animator.StringToHash("Idle");
		private readonly int _lightningValue = Animator.StringToHash("LightningValue");
		private readonly int _openAnim3 = Animator.StringToHash("Chest_open_three");
		private bool _chestIsShown;
		private Sequence _mySequence;

		public LootBoxType LootBoxType => _lootBoxType;

		public void Show(Action doOnCompleteShow)
		{
			ShowSequence(doOnCompleteShow);
		}

		public void PlayerTap()
		{
			if (_chestIsShown == false)
				return;

			_animator.CrossFade(_openAnim3, .2f, 0);
			_mySequence.Kill();
			_mySequence = DOTween.Sequence();
			_mySequence.SetUpdate(true);
			_mySequence.SetLink(gameObject);
			_mySequence.AppendInterval(0.12f);
			_mySequence.AppendCallback(() => OnPlayerOpenChest?.Invoke());
			_mySequence.Insert(1.0f, _chestTransform.DOLocalMoveY(_chestHideDownY, 1.75f).SetUpdate(true));
			_mySequence.AppendCallback(() => _chestIsShown = false);
		}

		public void Hide()
		{
			_animator.SetFloat(_lightningValue, 0.0f);
		}

		private void ShowSequence(Action doOnCompleteShow)
		{
			_chestTransform.localScale = Vector3.zero;
			_chestTransform.DOLocalMoveY(_chestDownY, 0f);
			_chestIsShown = false;
			_mySequence.Kill();
			_mySequence = DOTween.Sequence();
			_mySequence.SetUpdate(true);
			_mySequence.SetLink(gameObject);
			_mySequence.Append(_chestTransform.DOLocalMoveY(_chestTopY, 0));
			_mySequence.Append(_chestTransform.DOScale(Vector3.one * 0.8f, 0.25f).SetEase(_easeStart));
			_mySequence.Append(_chestTransform.DOShakeRotation(0.25f, 10f, 4));
			_mySequence.AppendCallback(() =>
			{
				_chestIsShown = true;
				doOnCompleteShow?.Invoke();
			});

			_animator.CrossFade(_idle, .2f, 0);
		}
	}
}