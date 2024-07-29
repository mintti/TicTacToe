using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class MultiRoomManager : MonoBehaviour
    {
        [Header("Room Info")]
        [SerializeField] private GameObject      _roomPrefab;
        [SerializeField] private Transform       _roomListTr;
        
        [Header("Multi Room List Popup")]
        [SerializeField] private GameObject      _connectShieldObj;
        [SerializeField] private Button          _multiRoomPopupOpenBTN;
        [SerializeField] private Button          _multiRoomPopupCloseBTN;
        [SerializeField] private Button          _admissionBTN;
        [SerializeField] private Button          _newRoomPopupOpenBTN;


        [Header("Create Room Popup")]
        [SerializeField] private GameObject      _newRoomPopupObj;
        [SerializeField] private Button          _newRoomPopupCloseBTN;
        [SerializeField] private Button          _newRoomPopupCreateBTN;
        [SerializeField] private TMP_InputField  _inputField;
        
        private List<RoomInfo> _roomInfos = new();


        public void Init()
        {
            gameObject.SetActive(false);
            _newRoomPopupObj.SetActive(false);
            _connectShieldObj.SetActive(false);
            
            InitOnClickAction();
        }
        
        private void InitOnClickAction()
        {
            // Multi Room List Popup
            _multiRoomPopupOpenBTN .onClick.AddListener(OpenMultiRoomPopup );
            _multiRoomPopupCloseBTN.onClick.AddListener(CloseMultiRoomPopup);
            _newRoomPopupOpenBTN   .onClick.AddListener(OpenNewRoomPopup   );
            _admissionBTN          .onClick.AddListener(AccessRoom         );
            
            // Create Room Popup
            _newRoomPopupCloseBTN .onClick.AddListener(CloseNewRoomPopup);
            _newRoomPopupCreateBTN.onClick.AddListener(CreateRoom       );
        }

        /// <summary>
        /// 방 선택 이벤트 시, 다른 방 선택을 해제
        /// </summary>
        public void OnSelectionEnable(RoomInfo roomInfo)
        {
            foreach (var info in _roomInfos)
            {
                if(roomInfo != info)
                    info.Selected = false;
            }
        }
        
        private void CreateRoom()
        {
            string newRoomName = _inputField.text;
            
            var obj = Instantiate(_roomPrefab, _roomListTr);
            var info = obj.GetComponent<RoomInfo>();
            _roomInfos.Add(info);

            info.Init(this, newRoomName);
            CloseNewRoomPopup();
        }

        private void RemoveRoom(RoomInfo info)
        {
            _roomInfos.Remove(info);
        }

        private void AccessRoom()
        {
            
        }
        
        
        #region  Multi Room Popup
        private void OpenMultiRoomPopup()
        {
            gameObject.SetActive(true);
            
            // 접속 확인?
            // _connectShieldObj.SetActive(true);
        }

        private void CloseMultiRoomPopup()
        {
            gameObject.SetActive(false);
            _connectShieldObj.SetActive(false);
        }
        #endregion
        #region New Room Popup
        private void OpenNewRoomPopup()
        {
            _newRoomPopupObj.SetActive(true);
            _inputField.text = string.Empty;
        }

        private void CloseNewRoomPopup()
        {
            _newRoomPopupObj.SetActive(false);
        }
        #endregion
    }
}
