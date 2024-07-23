using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public event Action DisconnectedEvent;
        private static readonly Dictionary<NetworkConnection, GameObject> Players = new();
        private static Color LocalPrefabColor { get; set; }
        
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            //maybe excessive
            NetworkServer.Spawn(conn.identity.gameObject);
            AddPlayer(conn, conn.identity.gameObject);
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
            Debug.Log("OnServerDisconnect");
            base.OnServerDisconnect(conn);
            RemovePlayer(conn);
        }

        public override void OnClientDisconnect()
        {
            Debug.Log("OnClientDisconnect");
            base.OnClientDisconnect();
            DisconnectedEvent?.Invoke();
        }

        public static void AddPlayer(NetworkConnection conn, GameObject player)
        {
            Players.TryAdd(conn, player);
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.SetColor(LocalPrefabColor); //call CmdSetColor on the server
        }

        //This logic is probably already written in Mirror
        private void RemovePlayer(NetworkConnection conn)
        {
            if (Players.ContainsKey(conn))
            {
                if (Players[conn] != null)
                    Destroy(Players[conn]);
                Players.Remove(conn);
            }
        }

        public void SetLocalColor(Color color)
        {
            LocalPrefabColor = color;
        }
    }
}