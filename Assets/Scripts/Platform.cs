using UnityEngine;

namespace DonutRun 
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Vector3 _movingVector = Vector3.zero;

        [SerializeField] [Range(0f, 15f)] private float _movingSpeed = 1f;
        [SerializeField] private bool _deafaultPosition = false;

        private Vector3 _startPosition;
        private bool _isMovingBack = false;

        public Vector3 MoveVector { get { return _movingVector; } }

        void Start () 
        {
            _startPosition = transform.position;
        }

        void FixedUpdate()
        {
            if (_movingVector != Vector3.zero && !_deafaultPosition) 
            {
                Vector3 move = _startPosition + _movingVector;
                if (transform.position.normalized == move.normalized || transform.position.normalized == _startPosition.normalized)
                    _isMovingBack = !_isMovingBack;
                
                if (!_isMovingBack)
                    transform.position = Vector3.MoveTowards(transform.position, move, _movingSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, _startPosition, _movingSpeed * Time.deltaTime);

                # if UNITY_EDITOR
                    Debug.DrawLine(_startPosition, _startPosition + _movingVector, Color.blue);
                # endif
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, _startPosition, _movingSpeed * Time.deltaTime);
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

        // public void GetPathBounds()
    }
}
