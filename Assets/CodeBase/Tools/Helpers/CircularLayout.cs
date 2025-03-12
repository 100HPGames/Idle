using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Tools.Helpers
{
	public class CircularLayout : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Transform _center;

		[Header("Settings")]
		[SerializeField] private bool _enableRotation;
		[SerializeField] private float _radius;
		[SerializeField] private float _angleRangePerChild;
		[SerializeField] private float _startAngleShiftPerChild;

		public void ArrangeChildren(List<Transform> children)
		{
			var enabledChildren = children.Where(ch => ch.gameObject.activeInHierarchy).ToList();
			var angleStep = 360f / enabledChildren.Count;

			for (var i = 0; i < enabledChildren.Count; i++)
			{
				Transform child = enabledChildren[i];
				float angle = i * angleStep * Mathf.Deg2Rad;
				Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _radius;
				Vector3 centerLocalPosition = _center.localPosition;
				child.localPosition = centerLocalPosition + newPos;

				if (!_enableRotation)
					continue;

				Vector3 directionToCenter = (centerLocalPosition - child.localPosition).normalized;
				float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
				child.localRotation = Quaternion.Euler(0, 0, angleToCenter + 90);
			}
		}
	}
}