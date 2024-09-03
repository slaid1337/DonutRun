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

        private Utilities.Controls _input;

        public event Action OnNextPlatform;
        public event Action OnScoreChanged;
        
        private int _currentScore = 0;

        public int Score => _currentScore;
        
        void Start()
        {
            _input = new Utilities.Controls();
            _input.Enable();
        }

        void Update() 
        {
            
        }

        private void Death() 
        {
            Destroy(gameObject, 2.0f);
            _virtualCamera.Follow = null;
            _virtualCamera.LookAt = null;
        }

        void FixedUpdate()
        {
            if (_input.Default.LeftClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.back * _moveSpeed * Time.deltaTime);
            }

            if (_input.Default.RightClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.forward * _moveSpeed * Time.deltaTime);
            }

            CheckGround();
        }

        private void CheckGround() 
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            Debug.DrawRay(transform.position, Vector3.down, Color.yellow);

            if (hit.collider == null) 
            {
                _originTransform.parent = null;
            }
            else 
            {
                if (hit.collider.gameObject.GetComponent<Platform>() != null)
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
            }
            else if (other.name == "DeathCollider") 
            {
                Death();
            }
        }
    }
}
