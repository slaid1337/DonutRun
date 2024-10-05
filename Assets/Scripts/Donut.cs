using System;
using Cinemachine;
using UnityEngine;

namespace DonutRun 
{
    public class Donut : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 90f;
        
        [SerializeField] private Transform _originTransform;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        private Transform _lastSave;

        private Utilities.Controls _input;

        public event Action OnNextPlatform;
        public event Action OnScoreChanged;
        
        private int _currentScore = 0;

        public int Score => _currentScore;

        void Start()
        {
            _input = new Utilities.Controls();
            _input.Enable();

            DeathPanel.Instance.OnRevive.AddListener(Revive);

            PlatformManager.Instance.OnStartSpawn.AddListener(delegate
            {
                _lastSave = PlatformManager.Instance.GetFirstPlatform();
            });
        }

        private void Death() 
        {
            //Destroy(gameObject, 2.0f);
            _virtualCamera.Follow = null;
            _virtualCamera.LookAt = null;

            //SceneManager.LoadScene(0);

            DeathPanel.Instance.Open();
        }

        public void Revive()
        {
            _virtualCamera.Follow = transform;
            _virtualCamera.LookAt = transform;

            _rigidBody.velocity = Vector3.zero;
            _rigidBody.rotation = Quaternion.identity;

            transform.position = _lastSave.position;
        }

        void FixedUpdate()
        {
            if (_input.Default.LeftClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.back * _moveSpeed * Time.fixedDeltaTime * (Mathf.Abs(Mathf.Clamp(_rigidBody.velocity.x, 1, 1.3f))));
            }

            if (_input.Default.RightClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.forward * _moveSpeed * Time.fixedDeltaTime * (Mathf.Abs( Mathf.Clamp(_rigidBody.velocity.x, -1.3f, -1))));
            }

            CheckGround();
        }

        private void CheckGround() 
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f);
            Debug.DrawRay(transform.position, Vector3.down, Color.yellow, 1.5f);

            if (hit.collider == null) 
            {
                _originTransform.parent = null;
            }
            else 
            {
                if (hit.collider.gameObject.GetComponentInChildren<Platform>() != null)
                    _originTransform.parent = hit.collider.transform;
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.name == "NextPlatformTrigger")
            {
                _currentScore += 1;

                OnNextPlatform?.Invoke();
                OnScoreChanged?.Invoke();
                
                other.gameObject.SetActive(false);

                _lastSave = other.transform;
            }
            else if (other.name == "DeathCollider") 
            {
                Death();
            }

            JumpPlace jumpPlace = null;

            if (other.TryGetComponent<JumpPlace>(out jumpPlace))
            {
                if (jumpPlace.OtherPlace != null) Jump(jumpPlace.OtherPlace.transform);
            }

            Coin coin = null;

            if (other.TryGetComponent<Coin>(out coin))
            {
                MoneyController.Instance.AddMoney(coin.Revenue);
                coin.Destroy();
            }

            if (other.tag == "BackgroundSection")
            {
                EnvironmentGenerator.Instance.UpdateEnviernment();
                other.GetComponent<Collider>().enabled = false;
            }
        }

        private void Jump(Transform place)
        {
            Vector3 jumpDirection = place.position - transform.position;

            // Расчет необходимого времени для достижения цели
            float timeToApex = Mathf.Sqrt((2 * 2) / Mathf.Abs(-9.81f));

            // Определяем расстояние до цели по горизонтали
            float horizontalDistance = jumpDirection.magnitude;

            // Определяем необходимую скорость по горизонтали
            float horizontalVelocity = horizontalDistance / timeToApex;

            // Определяем скорость по вертикали
            float verticalVelocity = Mathf.Sqrt(2 * 2 * Mathf.Abs(-9.81f));

            // Создаем вектор скорости
            Vector3 velocity = jumpDirection.normalized;
            velocity *= horizontalVelocity; // Добавляем горизонтальную скорость
            velocity.y = verticalVelocity; // Добавляем вертикальную скорость

            // Применяем силу в соответствующем направлении
            _rigidBody.velocity = velocity;
        }
    }
}
