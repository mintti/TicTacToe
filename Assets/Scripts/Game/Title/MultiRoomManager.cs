using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class MultiRoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Room Info")]
        [SerializeField] private GameObject      _roomPrefab;
        [SerializeField] private Transform       _roomListTr;
        
        [Header("Multi Room List Popup")]
        [SerializeField] private GameObject      _connectShieldObj;
        [SerializeField] private TextMeshProUGUI _connectinfoTMP;
        [SerializeField] private Button          _multiRoomPopupOpenBTN;
        [SerializeField] private Button          _multiRoomPopupCloseBTN;
        [SerializeField] private Button          _admissionBTN;
        [SerializeField] private Button          _newRoomPopupOpenBTN;


        [Header("Create Room Popup")]
        [SerializeField] private GameObject      _newRoomPopupObj;
        [SerializeField] private Button          _newRoomPopupCloseBTN;
        [SerializeField] private Button          _newRoomPopupCreateBTN;
        [SerializeField] private TMP_InputField  _inputField;
        
        private List<Room> _rooms = new();


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
        
        #region Room Managing
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            // 방 목록 업데이트 시 호출되는 콜백
            _connectinfoTMP.SetText("방 목록 읽는 중..");
            
            UpdateRoomList(roomList);
            
            _connectShieldObj.SetActive(false);
        }
        
        void UpdateRoomList(List<RoomInfo> roomList)
        {
            ClearRooms();
            
            foreach (RoomInfo room in roomList)
            {
                if (room.RemovedFromList) continue; // 삭제된 방은 스킵

                string roomName = room.Name; // 기본적으로는 방 이름 사용

                // Custom Properties에서 방 제목 읽기
                if (room.CustomProperties.ContainsKey("RoomName"))
                {
                    roomName = (string)room.CustomProperties["RoomName"];
                }
                
                CreateRoomOnGame(roomName);
            }
        }
        
        /// <summary>
        /// 방 정보 및 오브젝트 초기화 
        /// </summary>
        private void ClearRooms()
        {
            for (int i = _rooms.Count - 1; i >= 0; i--)
            {
                RemoveRoom(_rooms[i]);
            }
        }

        /// <summary>
        /// 방 리스트에 방 프리팹 생성
        /// </summary>
        private void CreateRoomOnGame(string roomName)
        {
            var obj = Instantiate(_roomPrefab, _roomListTr);
            var info = obj.GetComponent<Room>();
            _rooms.Add(info);
            info.Init(this, roomName);
        }
        
        
        /// <summary>
        /// 방 생성
        /// </summary>
        private void CreateRoom()
        {
            if (CreateRoomOnServer())
            {
                // [TODO] 게임 씬으로 이동 필요
                return;
            }
            
            Debug.Log("방 만들기 실패");
            CloseNewRoomPopup();
        }
        
        /// <summary>
        /// 방을 서버에 생성
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        private bool CreateRoomOnServer()
        {
            string roomName = _inputField.text;
            
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;

            // 방 제목을 Custom Properties에 설정
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
            customProperties.Add("RoomName", roomName);
            roomOptions.CustomRoomProperties = customProperties;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "RoomName" };

            bool result = PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            return result;
        }

        private void RemoveRoom(Room info)
        {
            Destroy(info.gameObject);
            _rooms.Remove(info);
        }

        #endregion
        
        /// <summary>
        /// 방에 접속
        /// </summary>
        private void AccessRoom()
        {
            Room selectedRoom = null;

            foreach (var room in _rooms)
            {
                if (room.Selected)
                {
                    selectedRoom = room;
                    break;
                }
            }

            if (selectedRoom != null)
            {
                // [TODO] 게임 씬 이동
                return;
            }
            
            Debug.Log("방이 선택되지 않았음");
        }
        
        #region  Multi Room Popup Related
        #region 접속
        private void OpenMultiRoomPopup()
        {
            gameObject.SetActive(true);
            _admissionBTN.interactable = false;
            
            _connectShieldObj.SetActive(true);
            _connectinfoTMP.SetText("서버에 접속중..");
            
            PhotonNetwork.ConnectUsingSettings();
        }
        
        public override void OnConnectedToMaster()
        {
            // 로비에 입장
            _connectinfoTMP.SetText("로비에 접속중..");
            PhotonNetwork.JoinLobby();
        }
        
        public override void OnJoinedLobby()
        {
            _connectinfoTMP.SetText("로비 접속 완료..");
        }
        #endregion

        #region 종료

        private void CloseMultiRoomPopup()
        {
            gameObject.SetActive(false);
            _connectShieldObj.SetActive(false);
            
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
            
        }
        #endregion

        /// <summary>
        /// 방 선택 이벤트 시, 다른 방 선택을 해제
        /// </summary>
        public void OnSelectionEnable(Room room)
        {
            foreach (var info in _rooms)
            {
                if (room != info)
                    info.Selected = false;
                else
                    _admissionBTN.interactable = true;
            }
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
