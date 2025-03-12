using System;
using CodeBase.Tools;
using CodeBase.Tools.SimpleMessenger;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.InputService
{
	public class TouchInputService : MonoBehaviour, IService
	{
		public event Action OnTouchedEnd;

		[DI] private Messenger _messenger;

		private TouchPhase _touchPhase;
		private Vector3 _touchUpValue;
		private Vector3 _touchDownValue;
		private Vector3 _touchMoveValue;

		private bool _isLockedInput;
		private float _touchEndTimeValue;
		private float _touchStartTimeValue;
		public Vector3 TouchUpValue => _touchUpValue;
		public Vector3 TouchDownValue => _touchDownValue;
		public float TouchEndTimeValue => _touchEndTimeValue;
		public float TouchStartTimeValue => _touchStartTimeValue;
		
		public void Init(IProtoSystems systems)
		{
			_isLockedInput = false;
		}
		
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
			_touchPhase = TouchPhase.Began;
		}

		private void Moved()
		{
			_touchPhase = TouchPhase.Moved;
			_touchMoveValue = Input.mousePosition;
		}

		private void Ended()
		{
			_touchPhase = TouchPhase.Ended;
			_touchUpValue = Input.mousePosition;
			_touchEndTimeValue = Time.time;
			OnTouchedEnd?.Invoke();
		}

		public void Destroy()
		{
			
		}
	}
}