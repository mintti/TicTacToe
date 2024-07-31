using System;
using Game.Controller;
using Game.Server;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Player
    {
        public PlayerInfo Info;
        public PlayerMovement Movement;
    }
    /// <summary>
    /// 플레이어들의 조작을 제어
    /// </summary>
    public class MovementHandler : MonoBehaviour
    {
        private PlayerInfo _curPlayer;
        [SerializeField] private Player _player;
        [SerializeField] private Player _otherPlayer;
        
        [SerializeField] private PhotonView     _pv;

        private int            _playerIndex;
        private MovementFormat _format;


        public void Init()
        {
            var playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            _curPlayer = _player.Info;
            
            _curPlayer.Name = $"P0{playerCnt}";
            _otherPlayer.Info.Name = $"P0{(playerCnt == 1 ? 2 : 1)}";
        }
        
        public void SendData(EMoveType eMove)
        {
            _format = new MovementFormat()
            {
                Name = _curPlayer.Name, // 현재 클라이언트의 정보를 읽기?
                EMoveType =  eMove
            };
            var json = JsonUtility.ToJson(_format);
            
            _pv.RPC(nameof(ReceiveData), RpcTarget.All, json);
        }
    
        [PunRPC]
        public void ReceiveData(string data)
        {
            var dataForm = JsonUtility.FromJson<MovementFormat>(data);
            
            if(_player == null)            throw new Exception("Player is null");
            else if (_player.Info == null) throw new Exception("PlayerInfo is null");
            
            Player p = dataForm.Name == _player.Info.Name? _player : _otherPlayer;
            
            Debug.Log($"{dataForm.Name} {_player.Info.Name}");
            
            if (p.Info == _curPlayer)
            {
                p.Movement.SetMove((EMoveType)((int)dataForm.EMoveType));
            }
            else
            {
                _otherPlayer.Movement.SetMove((EMoveType)((int)dataForm.EMoveType * -1));
            }
        }

        public PlayerInfo GetOtherPlayerInfo(PlayerInfo info)
        {
            Player winner = _player.Info == info ? _otherPlayer : _player;
            return winner.Info;
        }
    }
}