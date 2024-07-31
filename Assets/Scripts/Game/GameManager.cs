using System;
using System.Collections;
using Game.Controller;
using Game.Server;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        [Header("Network Info")]
        [SerializeField] private ServerManager _serverManager;
        
        [Header("Player Info")]
        [SerializeField] private PlayerController _controller;

        [SerializeField] private MovementHandler _movementHandler;
        [SerializeField] private Ball _ball;
        

        [Header("GameUI")]
        [SerializeField] private TextMeshProUGUI _countDownTMP;

        private bool Restart;
        
        private void Awake()
        {
            _instance = this;
            CompleteConnectServer();
        }


        public void CompleteConnectServer()
        {
            _serverManager.SendMessage("Someone has been connected.");
            
            Init();
            
        }
        
        private void Init()
        {
            _controller.Init();
            _ball.Init(this);
            
            var isWaitPlayer = PhotonNetwork.CurrentRoom.PlayerCount == 2;
            var notice = isWaitPlayer ? "게임이 곧 시작됩니다." : "플레이어 대기중";
            _serverManager.SendMessage(notice);

            if (isWaitPlayer)
            {
                _serverManager.SendGameStart();
            }
        }

        public void GameStart()
        {
            StartCoroutine(nameof(GameSeq));
        }

        private IEnumerator GameSeq()
        {
            int playCnt = 10;
            while (playCnt > 0)
            {
                yield return GameStartTimer();
                
                _ball.StartMove();
                
                Restart = false;
                yield return new WaitUntil(() => Restart);
                
                _ball.StopMove();
                
                yield return new WaitForSeconds(3f);
                
                playCnt--;
            }
        }

        private IEnumerator GameStartTimer()
        {
            for (int i = 3; i > 0; i--)
            {
                _countDownTMP.SetText($"{i}");
                yield return new WaitForSeconds(1f);
            }
            _countDownTMP.SetText($"시작!");
        }
        
        public void Lose(PlayerInfo loser)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            var winner = _movementHandler.GetOtherPlayerInfo(loser);
            _serverManager.SendMessage($" {winner.Name} + 1점");
            Restart = true;
        }
    }
}