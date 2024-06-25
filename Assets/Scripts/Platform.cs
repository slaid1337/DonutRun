using UnityEngine;

namespace DonutRun 
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Vector3 _movingOffset = Vector3.zero;

        [SerializeField] [Range(0f, 15f)] private float _movingSpeed = 1f;

        private Vector3 _startPosition;
        private bool _isMovingBack = false;

        void Start () 
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            if (_movingOffset != Vector3.zero) 
            {
                Vector3 move = _startPosition + _movingOffset;
                if (transform.position.normalized == move.normalized || transform.position.normalized == _startPosition.normalized)
                    _isMovingBack = !_isMovingBack;
                
                if (!_isMovingBack)
                    transform.position = Vector3.MoveTowards(transform.position, move, _movingSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, _startPosition, _movingSpeed * Time.deltaTime);

                # if UNITY_EDITOR
                    Debug.DrawLine(_startPosition, _startPosition + _movingOffset, Color.blue);
                # endif
            }
        }
    }
}
