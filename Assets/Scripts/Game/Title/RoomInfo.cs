using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class RoomInfo : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private Button          _selectBTN;
        [SerializeField] private TextMeshProUGUI _roomNameTMP;
        [SerializeField] private Outline         _outline;

        private MultiRoomManager _manager;
        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                _outline.enabled = _selected;

                if (_selected)
                {
                    _manager.OnSelectionEnable(this);
                }
            }
        }

        public void Init(MultiRoomManager manager, string roomName)
        {
            _manager = manager;
            
            _roomNameTMP.SetText(roomName);
            _selectBTN.onClick.AddListener(() => Selected = !_selected);
            
            Selected = false;
        }
    }
}
