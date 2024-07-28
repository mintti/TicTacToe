using Game.Controller;
using Game.Server;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class MovementHandler : MonoBehaviour, IPunObservable
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PhotonView     _pv;

        private int          _playerIndex;
        private MovementFormat _format;


        public void Init()
        {
        }

        public void Packing()
        {
            
        }
        
        public void SendData(EMoveType eMove)
        {
            _format = new MovementFormat()
            {
                PlayerIndex = _playerIndex, // 현재 클라이언트의 정보를 읽기?
                EMoveType =  eMove
            };
            var json = JsonUtility.ToJson(_format);
            
            _pv.RPC(nameof(ReceiveData), RpcTarget.All, json);

            // var stream = new PhotonStream(true, new object[]{});
            // var info   = new PhotonMessageInfo();
            // OnPhotonSerializeView(stream, info);
        }
    
        [PunRPC]
        public void ReceiveData(string data)
        {
            var dataForm = JsonUtility.FromJson<MovementFormat>(data);
            _movement.SetMove(dataForm.EMoveType);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 데이터 전송
                var json = JsonUtility.ToJson(_format);
                stream.SendNext(json);
                Debug.Log(("데이터 송신"));
            }
            else
            {
                // 데이터 수신
                var json = (string)stream.ReceiveNext();
                var dataForm = JsonUtility.FromJson<MovementFormat>(json);
                
                _movement.SetMove(dataForm.EMoveType);
                Debug.Log(("데이터 수신"));
            }
        }
    }
}