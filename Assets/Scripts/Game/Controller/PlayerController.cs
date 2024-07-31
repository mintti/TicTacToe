using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controller
{
    public enum EController
    {
        None,
        Keyboard,
        Mouse
    }
    
    public class PlayerController : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private EController     eEController;
        [SerializeField] private MovementHandler _handler;
        
        public void Init()
        {
            IController controller = null;
            switch (eEController)
            {
                case EController.Keyboard :
                    controller = transform.AddComponent<KeyboardController>();
                    break;
                case EController.Mouse :
                    controller = transform.AddComponent<MouseController>();
                    break;
                default:
                    Debug.Log("컨트롤러 지정 필요");
                    break;
            }
            
            _handler.Init();
            controller?.Init(_handler);
        }
    }
}
