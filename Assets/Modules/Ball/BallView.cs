using UnityEngine;

namespace Ball
{
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PhysicMaterial physMat;

        private void Awake()
        {
            if (rb == null) TryGetComponent(out rb);
            if (physMat == null)
            {
                TryGetComponent(out Collider col);
                physMat = col.material;
            }
        }
        
        public void SetPhysicsProperties(float mass, float drag, float bounce)
        {
            rb.mass = mass;
            rb.drag = drag;
            physMat.bounciness = bounce;
        }

        public void ApplyForce(Vector3 force)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        private void OnDisable()
        {
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}