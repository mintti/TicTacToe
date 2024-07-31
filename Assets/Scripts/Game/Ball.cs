using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Controller;
using Game.Server;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float       _speed;
    [SerializeField] private Vector2     _defaultDir;
    
    private GameManager _gameManager;
    private Vector3 _initPos;

    private void Start()
    {
        _initPos = transform.position;
    }

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        if (!PhotonNetwork.IsMasterClient) _defaultDir *= -1;
    }

    public void StartMove()
    {
        transform.position = _initPos;
        _rigidbody.velocity = _defaultDir.normalized * _speed;
    }

    public void StopMove()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Wall"))
        {
            CalculateVelocity(col);
        }
        else if (col.CompareTag("DeathZone"))
        {
            var player = col.GetComponent<DeathZone>().PlayerInfo;
            _gameManager.Lose(player);
        }
    }
    
    /// <summary>
    /// 공의 반사 벡터를 계산 및 velocity에 설정
    /// </summary>
    private void CalculateVelocity(Collider2D col)
    {
        Vector2 normalDir    = GetCollisionNormal(col);
        Vector2 currentDir   = _rigidbody.velocity.normalized;
        Vector2 reflectedDir = Vector2.Reflect(currentDir, normalDir);
        
        _rigidbody.velocity  = reflectedDir * _speed;
    }
    
    /// <summary>
    /// 충돌한 객체의 접촉 지점을 기준으로 법선 벡터를 계산하는 함수
    /// 충돌 지점이 콜라이더의 어느 면과 가까운지를 기준으로 법선 벡터를 설정
    /// </summary>
    /// <param name="col">충돌한 콜라이더</param>
    /// <returns>충돌 지점의 법선 벡터</returns>
    private Vector2 GetCollisionNormal(Collider2D col)
    {
        Vector2 normal = Vector2.zero;
        Vector2 contactPoint = col.ClosestPoint(transform.position);
        
        float tolerance = Mathf.Min(col.bounds.size.x, col.bounds.size.y) * 0.1f; // 임계값을 콜라이더 크기에 비례하도록 설정
        
        if (Mathf.Abs(contactPoint.x - col.bounds.min.x) < tolerance)
        {
            normal = Vector2.right;
        }
        else if (Mathf.Abs(contactPoint.x - col.bounds.max.x) < tolerance)
        {
            normal = Vector2.left;
        }
        else if (Mathf.Abs(contactPoint.y - col.bounds.min.y) < tolerance)
        {
            normal = Vector2.up;
        }
        else if (Mathf.Abs(contactPoint.y - col.bounds.max.y) < tolerance)
        {
            normal = Vector2.down;
        }

        return normal;
    }
}
