using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
	public class TargetPointer : MonoBehaviour
	{
		[SerializeField] private RectTransform stationPointer;
		[SerializeField] private RectTransform enemyPointer;
		[SerializeField] private GameObject planet;

		private GameObserver _observer;
		private Vector3 _startPointerSize;
		private Camera _mainCamera;
		private float _interfaceScale;
		private int _size;

		private Dictionary<RectTransform, GameObject[]> _targets;

		private void Awake()
		{
			_startPointerSize = stationPointer.sizeDelta;
			_mainCamera = Camera.main;
			_interfaceScale = 300;
			_size = 500;

			_observer = GameObject.Find("Utilities").GetComponent<GameObserver>();

			_targets = new Dictionary<RectTransform, GameObject[]>
			{
				{stationPointer, GameObject.FindGameObjectsWithTag("Station")},
				{enemyPointer, GameObject.FindGameObjectsWithTag("Enemy")}
			};
		}

		private void UpdateTargets()
		{
			_targets[stationPointer] = GameObject.FindGameObjectsWithTag("Station");
			_targets[enemyPointer] = GameObject.FindGameObjectsWithTag("Enemy");
		}

		private float GetDistance(Vector3 t)
		{
			var p = _observer.Player.transform.position;
			return (p.x - t.x) * (p.x - t.x) + (p.y - t.y) * (p.y - t.y);
		}
		
		private GameObject GetNearestTarget(GameObject[] targets)
		{
			if (targets.Count(target => target.activeSelf) == 0) return null;
			var distance = targets.Min(t => GetDistance(t.transform.position));
			return targets.FirstOrDefault(t => Math.Abs(GetDistance(t.transform.position) - distance) < 1);
		}

		private void LateUpdate()
		{
			UpdateTargets();
			foreach (var pointer in _targets.Keys)
			{
				var target = GetNearestTarget(_targets[pointer]);
				if (target is null)
				{
					if (pointer != stationPointer)
					{
						pointer.gameObject.SetActive(false);
						return;
					}
					else
						target = planet;
				}
			
				var realPos = _mainCamera.WorldToScreenPoint(target.transform.position);
				var rect = new Rect(-_size, -_size, Screen.width + _size * 2, Screen.height + _size * 2);

				var outPos = realPos;
				float direction = 1;

				pointer.gameObject.SetActive(true);

				if (rect.Contains(realPos)) pointer.gameObject.SetActive(false);
			
				if (IsBehind(target.transform.position))
				{
					realPos = -realPos;
					outPos = new Vector3(realPos.x, 0, 0);
			
					if (_mainCamera.transform.position.y < target.transform.position.y)
					{
						direction = -1;
						outPos.y = Screen.height;		
					}
				}

				var offset = pointer.sizeDelta.x;
			
				outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width - offset);
				outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

				RotatePointer(pointer, direction * realPos - outPos);

				pointer.sizeDelta = new Vector2(
					_startPointerSize.x / 100 * _interfaceScale,
					_startPointerSize.y / 100 * _interfaceScale
				);
				pointer.anchoredPosition = outPos;
			}
		}
	
		private bool IsBehind(Vector3 point)
		{
			var cameraTransform = _mainCamera.transform;
			return Vector3.Dot(
				cameraTransform.TransformDirection(Vector3.forward),
				point - cameraTransform.position) < 0;
		}
	
		private void RotatePointer(RectTransform p, Vector2 direction)
		{		
			p.rotation = Quaternion.AngleAxis(
				Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
		}
	}
}