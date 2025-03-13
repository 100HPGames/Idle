using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services.MediatoerService;
using CodeBase.Tools.CurrencyHandler;
using CodeBase.Tools.Helpers;
using CodeBase.UI.FlyService;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Mediators
{
	public class CurrencyMediator : MonoBehaviour, IMediator, IDisposable
	{
		public event Action<IMediator> OnCleanUp;

		[Header("Components")]
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Transform _currencyViewElementParent;
		[SerializeField] private List<CurrencyData> _showResourceData;
		[SerializeField] private CurrencyView _currencyViewElementPrefab;

		[Header("Settings")]
		[SerializeField] private bool _shrink;
		[SerializeField] private bool _autoHide;
		[SerializeField] private bool _resourceFly;
		[SerializeField] private float _showDuration = 0.25f;
		[SerializeField] private float _flyDuration = 0.6f;

		private int _addedTestValue = 1;
		private Sequence _flySequence;
		private CurrencyService _currencyService;
		private FlyItemService _flyItemService;
		private readonly Dictionary<CurrencyData, CurrencyView> _currencyViewElements = new();

		public GameObject GameObject => gameObject;
		public bool IsAdditiveMediator { get; set; } = true;
		public bool IsCameraSpace { get; }

		public void Construct(CurrencyService currencyService, FlyItemService flyItemService)
		{
			_flyItemService = flyItemService;
			_currencyService = currencyService;

			ClearResourceViewElements();
			InitResourcesToShow();

			_currencyService.OnValueWithPayloadAdded += AddResourceCount;
			_currencyService.OnValueSet += SetResourceCount;
			_currencyService.OnValueSubtracted += SubtractResource;
		}

		public void EnterBattle()
		{
			if (TryGetElement(CurrencyType.Soft, out var currencySoftView))
				currencySoftView.Hide();

			if (TryGetElement(CurrencyType.Hard, out var currencyShardsView))
				currencyShardsView.Hide();
		}

		public void ExitBattle()
		{
			if (TryGetElement(CurrencyType.Soft, out var currencySoftView))
				currencySoftView.Show();

			if (TryGetElement(CurrencyType.Hard, out var currencyShardsView))
				currencyShardsView.Show();
		}

		private void InitResourcesToShow()
		{
			_currencyViewElements.Clear();
			foreach (var currencyData in _showResourceData)
			{
				var elementPrefab = _currencyViewElementPrefab;
				var element = Instantiate(elementPrefab, _currencyViewElementParent);
				element.Init(currencyData, _autoHide, _shrink, _currencyService);
				element.name = $"{currencyData.Type.ToString()} View Element";
				_currencyViewElements.Add(currencyData, element);
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				var currencyTypeEnums = Enum.GetValues(typeof(CurrencyType));
				var currencyTypeList = currencyTypeEnums.Cast<CurrencyType>().ToList();

				foreach (var currencyType in currencyTypeList)
					_currencyService.AddResource(currencyType, _addedTestValue, true, Input.mousePosition);
			}

			if (Input.GetKeyDown(KeyCode.L))
				_addedTestValue *= 10;
		}
#endif

		private void ClearResourceViewElements(bool immediate = false)
		{
			_currencyViewElementParent.DestroyChildren(immediate);
			_currencyViewElements.Clear();
		}

		private void AddResourceCount(CurrencyType type, int value, Vector3 screenPosition, bool fly, bool flyLeft)
		{
			Debug.Log("Update " + type + " Currency");

			if (TryGetElement(type, out CurrencyView element))
			{
				element.SetCanvasShow(true);
				element.SetFly(fly);
				element.UpdateResourceUiView(value);

				if (_resourceFly || fly)
				{
					_flyItemService.FlyItem(GetResourceData(type).ResourceIcon,
					                        screenPosition,
					                        element.ResourceIcon.transform.position,
					                        value: value,
					                        flyDuration: _flyDuration,
					                        action: () =>
					                        {
						                        element.SetFly(false);
						                        element.UpdateView();
					                        });
				}
			}
		}

		private void SubtractResource(CurrencyType type, int value)
		{
			if (TryGetElement(type, out var element))
				element.SubtractResource(value);
		}

		private void SetResourceCount(CurrencyType type, int value)
		{
			if (TryGetElement(type, out var element))
			{
				element.SetResourceCount(value);
			}
		}

		private bool TryGetElement(CurrencyType type, out CurrencyView element)
		{
			var resourceData = GetResourceData(type);

			if (resourceData != default)
			{
				if (_currencyViewElements.TryGetValue(resourceData, out var viewElement))
				{
					element = viewElement;
				}
				else
				{
					var elementPrefab = _currencyViewElementPrefab;
					element = Instantiate(elementPrefab, _currencyViewElementParent);
					element.Init(resourceData, _autoHide, _shrink, _currencyService);
					element.name = $"{type.ToString()} View Element";
					_currencyViewElements.Add(resourceData, element);
				}

				return true;
			}

			element = null;
			return false;
		}

		private CurrencyData GetResourceData(CurrencyType type) => _showResourceData.Find(x => x.Type == type);

		public void Show() => _canvasGroup.Show(_showDuration);
		public void Hide() => _canvasGroup.Hide(_showDuration);

		public void Dispose()
		{
			_currencyService.OnValueWithPayloadAdded -= AddResourceCount;
			_currencyService.OnValueSet -= SetResourceCount;
			_currencyService.OnValueSubtracted -= SubtractResource;

			OnCleanUp?.Invoke(this);

			if (this != null && !Equals(null))
				Destroy(gameObject);
		}
	}

	public enum CurrencyType
	{
		Soft = 0,
		Hard = 1,

		//Spell Recipes
		WaterFallRecipe = 100,
		FireBallRecipe = 101,
		WaterBubblesRecipe = 102,
		FirestormRecipe = 103,
		SelfHealingRecipe = 104,
		PoisonSpellRecipe = 105,
		DispelDebuffRecipe = 106,
		MirrorSpellRecipe = 107,
		MassHealingRecipe = 108,
		StoneStunRecipe = 109,
		ArmorBuffSpellRecipe = 110,
		DamageReturnSpellRecipe = 111,
		TemporaryHealthSpellRecipe = 112,
		BombSpellRecipe = 113,
		InvisibilitySpellRecipe = 114,
		DurationCurseSpellRecipe = 115,
		ArmorPearceSpellRecipe = 116,
		ChannelFireStormRecipe = 117,
		ChannelMeteoriteRecipe = 118,
		EarthQuakeRecipe = 119,
	}

	public enum Rarity
	{
		Common = 1,
		Rare = 2,
		Epic = 3,
		Legendary = 4,
	}
}