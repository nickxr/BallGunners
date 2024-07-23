using Mirror;
using Network;
using UnityEngine;

namespace Player
{
    public class CannonController : MonoBehaviour
    {
        public float rotationSpeed = 5f;
        public float minXRotation = -30f;
        public float maxXRotation = 30f;
        public float minYRotation = -60f;
        public float maxYRotation = 60f; 
        
        public float minForce = 10f; 
        public float maxForce = 50f;

        public Transform shootPoint;
        public NetworkBallsSpawner networkBallsSpawner;
        
        [SerializeField] private bool isLocalPlayer;

        private float _currentXRotation;
        private float _currentYRotation;
        private float _initialZRotation = 90f;
        
        private float _holdStartTime;
        private bool _isHoldingFire;
        
        private void Start()
        {
            isLocalPlayer = transform.root.GetComponent<NetworkIdentity>().isLocalPlayer;
            
            if (networkBallsSpawner == null)
            {
                Debug.LogError("NetworkGameManager not found in the scene.");
            }

            _initialZRotation = transform.localEulerAngles.z;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            HandleRotation();
            HandleFireInput();
        }

        private void HandleFireInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _holdStartTime = Time.time;
                _isHoldingFire = true;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (_isHoldingFire)
                {
                    float holdDuration = Time.time - _holdStartTime;
                    float fireForce = Mathf.Clamp(holdDuration * 20f, minForce, maxForce);

                    if (isLocalPlayer)
                    {
                        Fire(fireForce);
                    }

                    _isHoldingFire = false;
                }
            }
        }

        private void Fire(float force)
        {
            if (networkBallsSpawner != null)
            {
                //Send command for spawn ball on server
                networkBallsSpawner.CmdFire(shootPoint.position, shootPoint.rotation, shootPoint.forward * force);
            }
        }
        
        private void HandleRotation()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            _currentYRotation = Mathf.Clamp(_currentYRotation + mouseX * rotationSpeed, minYRotation, maxYRotation);

            float rotationX = mouseY * rotationSpeed;
            _currentXRotation = Mathf.Clamp(_currentXRotation + rotationX, minXRotation, maxXRotation);

            transform.localRotation = Quaternion.Euler(_currentXRotation, _currentYRotation, _initialZRotation);
        }
    }

}