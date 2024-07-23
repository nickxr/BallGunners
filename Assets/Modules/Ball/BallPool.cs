using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class BallPool : MonoBehaviour
    {
        public GameObject ballPrefab;
        public int initialSize = 10;

        private Queue<GameObject> _pool;

        private void Start()
        {
            _pool = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject ball = Instantiate(ballPrefab);
                ball.SetActive(false);
            
                _pool.Enqueue(ball);
            }
        }

        public GameObject Get()
        {
            if (_pool.Count > 0)
            {
                GameObject ball = _pool.Dequeue();
                return ball;
            }
            else
            {
                GameObject ball = Instantiate(ballPrefab);
                return ball;
            }
        }

        public void Put(GameObject ball)
        {
            ball.SetActive(false);
            _pool.Enqueue(ball);
        }
    }
}