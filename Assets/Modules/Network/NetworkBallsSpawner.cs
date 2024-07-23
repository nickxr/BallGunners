using System.Collections.Generic;
using Ball;
using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkBallsSpawner : NetworkBehaviour
    {
        private List<GameObject> _activeBalls;
        
        private void Start()
        {
            _activeBalls = new List<GameObject>();
        }

        [Command]
        public void CmdFire(Vector3 position, Quaternion rotation, Vector3 force)
        {
            if (_activeBalls.Count >= BallPool.singleton.maxBalls)
            {
                DestroyBall(_activeBalls[0]);
            }

            GameObject ball = BallPool.singleton.Get(position, rotation);
            if (ball != null)
            {
                ball.GetComponent<BallController>().Launch(force);  // Add force for shooting
                NetworkServer.Spawn(ball);
                _activeBalls.Add(ball);
                RpcSyncBall(ball);
            }
            else
            {
                Debug.LogWarning("Could not fire ball because max limit has been reached.");
            }
        }

        [ClientRpc]
        private void RpcSyncBall(GameObject ball)
        {
            if (isServer)
                return;
        }
        
        [Server]
        public void DestroyBall(GameObject ball)
        {
            if (ball != null)
            {
                NetworkServer.UnSpawn(ball);
                BallPool.singleton.Return(ball);
                _activeBalls.Remove(ball);
            }
        }
    }
}