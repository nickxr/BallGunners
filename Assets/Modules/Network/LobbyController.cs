using UI;
using UnityEngine;
using VContainer;

namespace Network
{
    public class LobbyController
    {
        private LobbyUI _lobbyUI;
        private CustomNetworkManager _networkManager;

        [Inject]
        public void Initialize(LobbyUI lobbyUI, CustomNetworkManager networkManager)
        {
            _lobbyUI = lobbyUI;
            _lobbyUI.SetPlayerColorEvent += SetColorFromUI;
            _networkManager = networkManager;
            _networkManager.DisconnectedEvent += HandleClientDisconnect;
        }
        
        private void SetColorFromUI(Color color)
        {
            _networkManager.SetLocalColor(color);
        }

        private void HandleClientDisconnect()
        {
            _lobbyUI.UpdateStatus("Disconnected from server");
        }

        ~LobbyController()
        {
            _lobbyUI.SetPlayerColorEvent -= SetColorFromUI;
            _networkManager.DisconnectedEvent -= HandleClientDisconnect;
        }
    }
}