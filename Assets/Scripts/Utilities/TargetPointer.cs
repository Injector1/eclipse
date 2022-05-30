using System.Linq;
using UnityEngine;

namespace Utilities
{
	public class TargetPointer : MonoBehaviour
	{
		[SerializeField] private Transform[] targets;
		[SerializeField] private RectTransform pointer;

		private Vector3 _startPointerSize;
		private Camera _mainCamera;
		private float _interfaceScale;
		private int _size;
	
		private void Awake()
		{
			_startPointerSize = pointer.sizeDelta;
			_mainCamera = Camera.main;
			_interfaceScale = 300;
			_size = 500;
		}

		private void LateUpdate()
		{
			var target = targets.FirstOrDefault(target => target.gameObject.activeSelf);
			if (target is null) return;
			
			var realPos = _mainCamera.WorldToScreenPoint(target.position);
			var rect = new Rect(-_size, -_size, Screen.width + _size * 2, Screen.height + _size * 2);

			var outPos = realPos;
			float direction = 1;

			pointer.gameObject.SetActive(true);

			if (rect.Contains(realPos)) pointer.gameObject.SetActive(false);
			
			if (IsBehind(target.position))
			{
				realPos = -realPos;
				outPos = new Vector3(realPos.x, 0, 0);
			
				if (_mainCamera.transform.position.y < target.position.y)
				{
					direction = -1;
					outPos.y = Screen.height;		
				}
			}

			var offset = pointer.sizeDelta.x;
			
			outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width - offset);
			outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

			RotatePointer(direction * realPos - outPos);

			pointer.sizeDelta = new Vector2(
				_startPointerSize.x / 100 * _interfaceScale,
				_startPointerSize.y / 100 * _interfaceScale
				);
			pointer.anchoredPosition = outPos;
		}
	
		private bool IsBehind(Vector3 point)
		{
			var cameraTransform = _mainCamera.transform;
			return Vector3.Dot(
				cameraTransform.TransformDirection(Vector3.forward),
				point - cameraTransform.position) < 0;
		}
	
		private void RotatePointer(Vector2 direction)
		{		
			pointer.rotation = Quaternion.AngleAxis(
				Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
		}
	}
}