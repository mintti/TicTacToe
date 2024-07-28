using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controller
{
    public enum Controller
    {
        None,
        Keyboard,
        Mouse
    }
    
    public class PlayerController : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private Controller     _eController;
        [SerializeField] private PlayerMovement _movement;

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            IController controller = null;
            switch (_eController)
            {
                case Controller.Keyboard :
                    controller = transform.AddComponent<KeyboardController>();
                    break;
                case Controller.Mouse :
                    controller = transform.AddComponent<MouseController>();
                    break;
                default:
                    Debug.Log("컨트롤러 지정 필요");
                    break;
            }

            controller?.Init(_movement);
        }
    }
}
