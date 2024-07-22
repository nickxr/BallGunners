using System.Collections.Generic;
using Ball;
using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkGameManager : NetworkBehaviour
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
            RpcFire(position, rotation, force);
        }

        [ClientRpc]
        private void RpcFire(Vector3 position, Quaternion rotation, Vector3 force)
        {
            // Создание мяча на всех клиентах
            SpawnBall(position, rotation, force);
        }

        private void SpawnBall(Vector3 position, Quaternion rotation, Vector3 force)
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
        }
    }
}