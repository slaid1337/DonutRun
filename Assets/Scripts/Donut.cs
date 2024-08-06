using UnityEngine;

namespace DonutRun 
{
    public class Donut : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 90f;
        
        [SerializeField] private Transform _originTransform;
        [SerializeField] private Rigidbody _rigidBody;
        private Utilities.Controls _input;
        
        void Start()
        {
            _input = new Utilities.Controls();
            _input.Enable();
        }

        void FixedUpdate()
        {
            if (_input.Default.LeftClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.back * _moveSpeed * Time.deltaTime);
                // _originTransform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.Self);
            }

            if (_input.Default.RightClick.IsPressed()) 
            {
                _rigidBody.AddTorque(Vector3.forward * _moveSpeed * Time.deltaTime);
                // _originTransform.Translate(Vector3.left * _moveSpeed * Time.deltaTime, Space.Self);
            }
        }

        private void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.GetComponent<Platform>() != null)
                _originTransform.parent = collision.transform;
        }

        private void OnCollisionExit(Collision collision) 
        {
            if (collision.gameObject.GetComponent<Platform>() != null)
                _originTransform.parent = null;
        }
    }
}
