using System;
using Mirror;
using Network;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerView view;
        [SyncVar] public Color playerColor;
        [SerializeField] private GameObject cameraObject;
        
        private void Awake()
        {
            cameraObject ??= GetComponentInChildren<Camera>().gameObject;
        }

        private void Start()
        {
            if (!isLocalPlayer)
            {
                cameraObject.SetActive(false);
            }

            if (isLocalPlayer && isClientOnly)
            {
                CustomNetworkManager.AddPlayer(connectionToServer, gameObject);
            }
        }

        public void SetColor(Color color)
        {
            if (isLocalPlayer)
            {
                playerColor = color;
                ApplyColor();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            ApplyColor();
        }

        private void ApplyColor()
        {
            if (view != null)
            {
                view.SetColor(playerColor);
            }
            else
            {
                Debug.LogError("PlayerView component is missing.");
            }
        }
    }
}