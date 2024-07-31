using Game.Controller;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        [ReadOnly, SerializeField] private float _moveX;
        [SerializeField]           private float _speed;

        private bool            _canMoveLeft = true;
        private bool            _canMoveRight = true;


        public void SetMove(EMoveType eMove)
        {
            _moveX = (int)eMove;
        }

        private void FixedUpdate()
        {
            ExecuteMove();
        }
        
        /// <summary>
        /// 이동 가능한 경우, 이동
        /// </summary>
        private void ExecuteMove()
        {
            if ((_moveX < 0 && !_canMoveLeft) || (_moveX > 0 && !_canMoveRight))
            {
                return;
            }
            
            transform.position += new Vector3(_moveX * _speed * Time.deltaTime, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
            {
                SetMoveDirection(other, false);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
            {
                SetMoveDirection(other, true);
            }
        }

        private void SetMoveDirection(Collider2D other, bool setValue)
        {
            var position         = transform.position;
            Vector2 contactPoint = other.ClosestPoint(position);
            Vector2 direction    = (Vector2)position - contactPoint;

            if (direction.x > 0)
            {
                _canMoveLeft = setValue;
            }
            else if (direction.x < 0)
            {
                _canMoveRight = setValue;
            }
        }
    }
}
