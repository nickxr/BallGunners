using Mirror;
using UnityEngine;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        private LobbyController _lobbyController;

        public void Initialize(LobbyController lobbyController)
        {
            Debug.Log("CustomNetworkManager Initialize");
            _lobbyController = lobbyController;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Player added.");
        }
        
        public override void OnServerSceneChanged(string sceneName)
        {
            // Ensure that we are in the Game scene before adding players
            if (sceneName == "Game")
            {
                Debug.Log($"OnServerSceneChanged {sceneName}");
                foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
                {
                    GameObject player = Instantiate(playerPrefab);
                    NetworkServer.AddPlayerForConnection(conn, player);
                    NetworkServer.Spawn(player);
                    _lobbyController.AddPlayer(conn, player);
                }
            }
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            Debug.Log("Host started, creating a new room.");
        }

        
        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("Client started, attempting to connect to a room.");
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            _lobbyController.RemovePlayer(conn);
            base.OnServerDisconnect(conn);
        }

        public override void OnClientDisconnect()
        {
            _lobbyController.HandleClientDisconnect();
            base.OnClientDisconnect();
        }
    }
}