using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerView view;
        [SyncVar] private Color _playerColor;
    
        public void SetColor(Color color)
        {
            _playerColor = color;
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
                view.SetColor(_playerColor);
            }
            else
            {
                Debug.LogError("PlayerView component is missing.");
            }
        }
    }
}