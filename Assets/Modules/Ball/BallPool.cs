using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class BallPool : MonoBehaviour
    { 
        // singleton for easier access from other scripts
        public static BallPool singleton;

        [Header("Settings")]
        public GameObject ballPrefab;
        private Queue<GameObject> _pool;
        public int maxBalls = 5;
        private int activeBallsCount;

        private void Start()
        {
            singleton = this;
            _pool = new Queue<GameObject>();
            activeBallsCount = 0;
        }
        
        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject ball = null;

            // find valid object in pool
            while (_pool.Count > 0)
            {
                ball = _pool.Dequeue();
                if (ball != null)
                    break;
            }

            if (ball == null)
            {
                if (activeBallsCount < maxBalls)
                {
                    ball = Instantiate(ballPrefab, position, rotation);
                    activeBallsCount++;
                }
                else
                {
                    Debug.LogWarning("Max ball limit reached. Cannot create more balls.");
                    return null;
                }
            }
            else
            {
                ball.SetActive(true);
                ball.transform.position = position;
                ball.transform.rotation = rotation;
            }

            return ball;
        }

        // Used to put object back into pool so they can b
        public void Return(GameObject ball)
        {
            if (ball != null)
            {
                ball.SetActive(false);
                _pool.Enqueue(ball);
            }
        }
    }
}