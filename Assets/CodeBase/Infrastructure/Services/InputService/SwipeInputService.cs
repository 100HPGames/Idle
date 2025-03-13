using System;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.InputService
{
	public class SwipeInputService : MonoBehaviour, IService
	{
		public event Action<SwipeDirection> OnSwipe;

		private TouchInputService _touchInputService;

		private const float MinSwipeDist = 100f;
		private const float MaxSwipeTime = .25f;

		public void Initialize(TouchInputService touchInputService)
		{
			_touchInputService = touchInputService;
			_touchInputService.OnTouchedEnd += OnCalculateSwipe;
		}

		private void OnDestroy() => _touchInputService.OnTouchedEnd -= OnCalculateSwipe;

		private void OnCalculateSwipe()
		{
			if (SwipeDistanceCheck() == false)
				return;

			if (SwipeTimingCheck() == false)
				return;

			SwipeDirection direction;

			if (IsVerticalSwipe())
			{
				direction = _touchInputService.TouchDownValue.y - _touchInputService.TouchUpValue.y > 0
					            ? SwipeDirection.Up
					            : SwipeDirection.Down;
			}
			else
			{
				direction = _touchInputService.TouchDownValue.x - _touchInputService.TouchUpValue.x > 0
					            ? SwipeDirection.Right
					            : SwipeDirection.Left;
			}

			OnSwipe?.Invoke(direction);
		}

		private bool SwipeTimingCheck()
		{
			float timeDelta = _touchInputService.TouchEndTimeValue - _touchInputService.TouchStartTimeValue;
			return timeDelta < MaxSwipeTime;
		}

		private bool IsVerticalSwipe() => VerticalMovementDistance() > HorizontalMovementDistance();

		private bool SwipeDistanceCheck()
			=> VerticalMovementDistance() > MinSwipeDist || HorizontalMovementDistance() > MinSwipeDist;

		private float VerticalMovementDistance()
			=> Mathf.Abs(_touchInputService.TouchDownValue.y - _touchInputService.TouchUpValue.y);

		private float HorizontalMovementDistance()
			=> Mathf.Abs(_touchInputService.TouchDownValue.x - _touchInputService.TouchUpValue.x);
	}

	public enum SwipeDirection
	{
		Up,
		Down,
		Left,
		Right
	}
}