using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.FlyService
{
	public class FlyItemView : MonoBehaviour
	{
		[SerializeField] private Image _image;

		public void SetSprite(Sprite resourceIcon) => _image.sprite = resourceIcon;
	}
}