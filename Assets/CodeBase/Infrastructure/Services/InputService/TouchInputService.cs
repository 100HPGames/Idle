using System;
using CodeBase.Tools;
using CodeBase.Tools.SimpleMessenger;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.InputService
{
	public class TouchInputService : MonoBehaviour, IService
	{
		public event Action OnTouchedEnd;

		[DI] private Messenger _messenger;

		private Vector3 _touchUpValue;
		private Vector3 _touchDownValue;
		private float _touchEndTimeValue;
		private float _touchStartTimeValue;
		private bool _isLockedInput = false;

		public Vector3 TouchUpValue => _touchUpValue;
		public Vector3 TouchDownValue => _touchDownValue;
		public float TouchEndTimeValue => _touchEndTimeValue;
		public float TouchStartTimeValue => _touchStartTimeValue;
		public TouchPhase Phase { get; private set; }
		public Vector3 TouchMoveValue { get; private set; }
		
		private void Awake() => ResetSwipeTiming();

		private void Update()
		{
			if (_isLockedInput)
				return;

#if UNITY_EDITOR || PLATFORM_STANDALONE_OSX
			if (Input.GetMouseButtonDown(0))
			{
				Began();
				return;
			}

			if (Input.GetMouseButton(0))
			{
				Moved();
				return;
			}

			if (Input.GetMouseButtonUp(0))
			{
				Ended();
			}

#elif !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			if (Input.touchCount < 1) 
				return;

			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Began();
				return;
			}

			if ((Input.GetTouch(0).phase == TouchPhase.Moved ||
			     Input.GetTouch(0).phase == TouchPhase.Stationary))
			{
				Moved();
				return;
			}

			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended) Ended();
#endif
		}
		
		private void ResetSwipeTiming()
		{
			_touchStartTimeValue = Time.time;
			_touchEndTimeValue = Time.time;
		}

		private void Began()
		{
			_touchDownValue = Input.mousePosition;
			_touchStartTimeValue = Time.time;
			Phase = TouchPhase.Began;
		}

		private void Moved()
		{
			Phase = TouchPhase.Moved;
			TouchMoveValue = Input.mousePosition;
		}

		private void Ended()
		{
			Phase = TouchPhase.Ended;
			_touchUpValue = Input.mousePosition;
			_touchEndTimeValue = Time.time;
			OnTouchedEnd?.Invoke();
		}
	}
}