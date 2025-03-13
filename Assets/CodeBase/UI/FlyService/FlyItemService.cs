using System;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Tools;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CodeBase.UI.FlyService
{
	public class FlyItemService : IService
	{
		public event Action OnFlyFinished;

		private FlyItemView _flyItemView;
		private UiOverlayRoot _uiRoot;

		public void Initialize(AssetProvider assetProvider, UiOverlayRoot uiRoot)
		{
			_uiRoot = uiRoot;
			_flyItemView = assetProvider.GetFlyItemView();
		}

		public void FlyItem(Sprite sprite,
		                    Vector3 startPosition,
		                    Vector3 endPosition,
		                    float delayToStart = 0.25f,
		                    float flyDuration = 0.6f,
		                    Transform parent = default,
		                    float value = 1f,
		                    Action action = default,
		                    float scaleToFlyEnd = 1.2f,
		                    float scaleFlyDestroy = 1,
		                    float additionalTimer = 0.1f)

		{
			var spawnPosition = startPosition;
			var limitValue = Mathf.Clamp(value, value, 10f);
			for (var i = 0; i < limitValue; i++)
			{
				var flyResource = Object.Instantiate(_flyItemView,
				                                     spawnPosition,
				                                     Quaternion.identity,
				                                     parent == default ? _uiRoot.FlyItemParent : parent);
				flyResource.SetSprite(sprite);

				Transform flyTransform = flyResource.transform;
				flyTransform.DOKill();
				flyTransform.localScale = Vector3.zero;
				flyTransform.position = spawnPosition + (Vector3) (Random.insideUnitCircle * 60);
				float delay = Random.Range(0.0f, delayToStart);
				Sequence flySequence = DOTween.Sequence();
				flySequence.SetUpdate(true);
				flySequence.AppendInterval(delay);
				flySequence.Append(flyTransform.DOScale(1f, additionalTimer).SetEase(Ease.OutCirc));

				var directionFly = 1f;
				var transformPosition = endPosition;
				flySequence.Join(flyTransform
				                 .DOPath(CreatePath(transformPosition, spawnPosition, directionFly),
				                         flyDuration,
				                         PathType.CatmullRom)
				                 .SetEase(Ease.Linear));

				flySequence.Append(flyTransform.DOScale(scaleToFlyEnd, additionalTimer).SetEase(Ease.InCirc));
				flySequence.Append(flyTransform.DOScale(scaleFlyDestroy, additionalTimer).SetEase(Ease.OutCirc));

				var i1 = i;
				flySequence.OnComplete(() =>
				{
					Object.Destroy(flyResource.gameObject);

					if (i1 >= limitValue - 1)
					{
						OnFlyFinished?.Invoke();
						action?.Invoke();
					}
				});
			}
		}

		private Vector3[] CreatePath(Vector3 targetPosition, Vector3 spawnPosition, float directionFly)
		{
			Vector3 direction = targetPosition - spawnPosition;
			Vector3 middlePoint = Vector3.Lerp(targetPosition, spawnPosition, Random.Range(0.35f, 0.75f));
			Vector3 perpendicular = Vector3.Cross(directionFly * direction, Vector3.forward);
			Vector3 shift = middlePoint + perpendicular / 4f;

			Vector3[] path = new Vector3[3];
			path[0] = spawnPosition;
			path[1] = shift;
			path[2] = targetPosition;
			return path;
		}
	}
}