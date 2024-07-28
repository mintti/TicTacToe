using System;
using System.Collections;
using Game.Controller;
using Game.Server;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
        [SerializeField] private Ball _ball;
        
        
        private void Awake()
        {
            _instance = this;
        }


        public void CompleteConnectServer()
        {
            _serverManager.SendMessage("Someone has been connected.");
            Init();
        }
        
        private void Init()
        {
            _controller.Init();
            _ball.Init();
        }
    }
}