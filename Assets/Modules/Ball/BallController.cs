using UnityEngine;

namespace Ball
{
    public class BallController : MonoBehaviour
    {
        private BallModel model;
        private BallView view;

        private void Awake()
        {
            model = new BallModel { Mass = 0.5f, Drag = 0.1f, Bounce = 0.6f }; // Example values
            view = GetComponent<BallView>();
            Initialize();
        }

        private void Initialize()
        {
            view.Initialize(this);
            view.SetPhysicsProperties(model.Mass, model.Drag, model.Bounce);
        }

        public void Launch(Vector3 force)
        {
            view.ApplyForce(force);
        }

        public void HandleCollision(Collision collision)
        {
            // Логика обработки столкновений
        }
    }
}