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
        public float minYRotation = -60f;  // Минимальное значение для вращения по оси Y
        public float maxYRotation = 60f;   // Максимальное значение для вращения по оси Y
        
        public float minForce = 10f; // Минимальная сила выстрела
        public float maxForce = 50f; // Максимальная сила выстрела

        public Transform shootPoint;        // Точка, из которой будет стреляться мяч
        public NetworkBallsSpawner networkBallsSpawner;
        
        [SerializeField] private bool isLocalPlayer;

        private float _currentXRotation = 0f;
        private float _currentYRotation = 0f;
        private float _initialZRotation = 90f; // Начальное значение по оси Z
        
        private float _holdStartTime;
        private bool _isHoldingFire;
        
        private void Start()
        {
            // Проверка, является ли этот объект локальным игроком
            isLocalPlayer = transform.root.GetComponent<NetworkIdentity>().isLocalPlayer;
            
            if (networkBallsSpawner == null)
            {
                Debug.LogError("NetworkGameManager not found in the scene.");
            }

            // Установка начального значения по оси Z
            _initialZRotation = transform.localEulerAngles.z;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            // Обработка вращения только для локального игрока
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
                    float fireForce = Mathf.Clamp(holdDuration * 20f, minForce, maxForce); // Пропорциональное значение силы

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
                // Вызов метода на сервере для спауна мяча
                networkBallsSpawner.CmdFire(shootPoint.position, shootPoint.rotation, shootPoint.forward * force);
            }
        }
        
        private void HandleRotation()
        {
            // Получаем ввод мыши
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            // Вращаем объект вокруг оси Y (влево-вправо) с ограничением
            _currentYRotation = Mathf.Clamp(_currentYRotation + mouseX * rotationSpeed, minYRotation, maxYRotation);

            // Вращение по оси X (вверх-вниз) с ограничением
            float rotationX = mouseY * rotationSpeed;
            _currentXRotation = Mathf.Clamp(_currentXRotation + rotationX, minXRotation, maxXRotation);

            // Устанавливаем вращение по всем осям
            transform.localRotation = Quaternion.Euler(_currentXRotation, _currentYRotation, _initialZRotation);
        }
    }

}