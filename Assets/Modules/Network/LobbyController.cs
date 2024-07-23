using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using Player;
using UI;
using UnityEngine;
using VContainer;

namespace Network
{
    public class LobbyController : MonoBehaviour
    {
        private readonly Dictionary<NetworkConnection, GameObject> _players = new();
        private readonly Dictionary<NetworkIdentity, Color> _playerColors = new();

        private LobbyUI _lobbyUI;
        private Color _selectedColor;

        [Inject]
        public void Initialize(LobbyUI lobbyUI)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.SetPlayerColorEvent += SetPlayerColor;
        
            Debug.Log("LobbyUI injected successfully: " + (lobbyUI != null));
        }

        public Color GetColor()
        {
            return _selectedColor;
        }
        
        private async void JoinGame()
        {
            if (NetworkManager.singleton)
            {
                NetworkManager.singleton.StartClient();
                Debug.Log("StartingClient");
                _lobbyUI.UpdateStatus("Joining game...");
            }
            else
            {
                NetworkManager.singleton.StartHost();
                Debug.Log("StartingHost");
                _lobbyUI.UpdateStatus("Hosting game...");
            }
        
            await WaitForClientToConnectAsync();
            NetworkManager.singleton.ServerChangeScene("Game");
        }

        private async UniTask WaitForClientToConnectAsync()
        {
            while (!NetworkClient.isConnected)
            {
                await UniTask.Delay(100);
            } 
        }

        private void SetPlayerColor(Color color)
        {
            Debug.Log($"SetPlayerColor {color}");
            _selectedColor = color;
        }

        public void AddPlayer(NetworkConnection conn, GameObject player)
        {
            _players[conn] = player;
            NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

            _playerColors.TryAdd(identity, _selectedColor);

            if (_playerColors.TryGetValue(identity, out var color))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.SetColor(color);
                }
                else
                {
                    Debug.LogError("PlayerController component missing from the GameObject.");
                }
            }
            else
            {
                Debug.LogError("Color not found for the player.");
            }
        }

        public void RemovePlayer(NetworkConnection conn)
        {
            if (_players.ContainsKey(conn))
            {
                Destroy(_players[conn]);
                _players.Remove(conn);
            }
        }

        public void HandleClientDisconnect()
        {
            _lobbyUI.UpdateStatus("Disconnected from server");
        }
    }
}