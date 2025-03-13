using CodeBase.Components;
using CodeBase.Components.Interactions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.MonoBehaivours
{
	public class DamageView : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private TMP_Text _damageValue;

		[Header("Settings")]
		[SerializeField] private float _duration = 0.01f;
		[SerializeField] private float _weakColorValue = 1.5f;
		[SerializeField] private float _resistanceColorValue = 0.5f;

		public void SetValue(DamageDisplay damageView, bool heal)
		{
			var healSign = heal ? "+" : "-";
			_damageValue.text = healSign + damageView.Value;
		}

		public void SetResistance(Color resistance)
		{
			_damageValue.DOColor(resistance, _duration);
			_damageValue.transform.DOScale(_resistanceColorValue, _duration).From(Vector3.one);
		}

		public void SetWeak(Color weak)
		{
			_damageValue.DOColor(weak, _duration);
			_damageValue.transform.DOScale(_weakColorValue, _duration).From(Vector3.one);
		}

		public void SetDefault(Color defaultColor)
		{
			_damageValue.DOColor(defaultColor, _duration);
			_damageValue.transform.DOScale(Vector3.one, _duration);
		}
	}
}