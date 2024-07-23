using System.Collections.Generic;
using Ball;
using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkBallsSpawner : NetworkBehaviour
    {
        public BallPool ballPool;
        public float maxBalls;

        private List<GameObject> _activeBalls;
        
        private void Start()
        {
            _activeBalls = new List<GameObject>();
        }

        [Command]
        public void CmdFire(Vector3 position, Quaternion rotation, Vector3 force)
        {
            SpawnBallOnServer(position, rotation, force);
        }

        [ClientRpc]
        private void RpcSyncBall(GameObject ball)
        {
            if (isServer)
                return;

            _activeBalls.Add(ball);

            if (_activeBalls.Count > maxBalls)
            {
                GameObject oldestBall = _activeBalls[0];
                ballPool.Put(oldestBall);
                _activeBalls.RemoveAt(0);
            }
        }

        [Server]
        private void SpawnBallOnServer(Vector3 position, Quaternion rotation, Vector3 force)
        {
            GameObject ball = ballPool.Get();
            ball.transform.position = position;
            ball.transform.rotation = rotation;
            ball.SetActive(true);
            ball.GetComponent<BallController>().Launch(force);

            NetworkServer.Spawn(ball);
            _activeBalls.Add(ball);

            if (_activeBalls.Count > maxBalls)
            {
                GameObject oldestBall = _activeBalls[0];
                NetworkServer.UnSpawn(oldestBall);
            
                ballPool.Put(oldestBall);
                _activeBalls.RemoveAt(0);
            }
            RpcSyncBall(ball);
        }
        
        [Server]
        public void HandleBallHit(GameObject ball)
        {
            if (_activeBalls.Contains(ball))
            {
                _activeBalls.Remove(ball);
                NetworkServer.UnSpawn(ball);
                ballPool.Put(ball);
            }
        }
    }
}