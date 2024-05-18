
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.Blog.UserInterface.WindowRooms
{
	public class RoomElementView : MonoBehaviour, IRoomElementView
	{
		[SerializeField] private TextMeshProUGUI _textName;
		[SerializeField] private Button _buttonEnter;
		public event Action OnButtonEnter;
		private void Awake()
		{
			_buttonEnter.onClick.AddListener(Enter);
		}
		public void SetData(RoomElementViewData roomElementViewData)
		{
			if (_textName) _textName.text = roomElementViewData.RoomName;
		}
		private void Enter()
		{
			OnButtonEnter?.Invoke();
		}
	}
}