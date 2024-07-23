using Mirror;
using Network;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerView view;
        [SyncVar(hook = nameof(OnColorChanged))] public Color playerColor;
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

        [Command]
        public void CmdSetColor(Color color)
        {
            playerColor = color; // This will trigger the SyncVar hook on all clients
        }

        public void SetColor(Color color)
        {
            if (isLocalPlayer)
            {
                CmdSetColor(color); // Set color through a Command
            }
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
            ApplyColor();
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