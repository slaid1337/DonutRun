using UnityEngine;

namespace DonutRun 
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Vector3 _movingVector = Vector3.zero;

        [SerializeField] private float _movingSpeed;
        [SerializeField] private bool _defaultPosition = false;
        [SerializeField] private GameObject[] _prefabsMoney;

        private bool _paused;

        private Vector3 _startPosition;
        private bool _isMovingBack = false;
        public Vector3 StartPosition
        {
            get
            {
                return _startPosition;
            }
        }

        public Vector3 MoveVector { get { return _movingVector; } }

        void Start()
        {
            if (_defaultPosition)
            {
                _startPosition = transform.position;
            }
            else
            {
                _startPosition = transform.position - _movingVector;
                _movingVector *= 2;
            }

        }

        public void Pause()
        {
            _paused = true;
        }

        public void UnPause()
        {
            _paused = false;
        }

        public void SetStatic()
        {
            _defaultPosition = true;
        }

        void FixedUpdate()
        {
            if (_paused) return;

            if (_movingVector != Vector3.zero && !_defaultPosition)
            {
                Vector3 move = _startPosition + _movingVector;
                if (transform.position.normalized == move.normalized || transform.position.normalized == _startPosition.normalized)
                    _isMovingBack = !_isMovingBack;

                if (!_isMovingBack)
                    transform.position = Vector3.MoveTowards(transform.position, move, _movingSpeed * Time.fixedDeltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, _startPosition, _movingSpeed * Time.fixedDeltaTime);

#if UNITY_EDITOR
                Debug.DrawLine(_startPosition, _startPosition + _movingVector, Color.blue);
#endif
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _startPosition, _movingSpeed * Time.fixedDeltaTime);
            }
        }

        public void ChangeMovingVector(Vector3 newVector)
        {
            _movingVector = newVector;
        }

        public void ChangeMovingSpeed(float newSpeed)
        {
            _movingSpeed = newSpeed;
        }

        public void DestroyPlatform(float delay)
        {
            Destroy(gameObject, delay);
        }

        public void GenerateMoney()
        {
            GameObject prefab = _prefabsMoney[Random.Range(0, _prefabsMoney.Length)];
            Instantiate(prefab, transform);
        }

        // public void GetPathBounds()
    }
}
