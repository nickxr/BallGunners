using UnityEngine;

namespace Ball
{
    public class BallController : MonoBehaviour
    {
        private BallModel _model;
        private BallView _view;

        private void Awake()
        {
            _model = new BallModel { Mass = 0.5f, Drag = 0.1f, Bounce = 0.6f }; // Good values after tests
            _view = GetComponent<BallView>();
            Initialize();
        }

        private void Initialize()
        {
            _view.SetPhysicsProperties(_model.Mass, _model.Drag, _model.Bounce);
        }

        public void Launch(Vector3 force)
        {
            _view.ApplyForce(force);
        }
    }
}