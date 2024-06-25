using UnityEngine;

namespace DonutRun 
{
    public class Donut : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1f;

        private Utilities.Controls _input;
        
        void Start()
        {
            _input = new Utilities.Controls();
            _input.Enable();
        }

        void Update()
        {
            if (_input.Default.LeftClick.IsPressed()) 
            {
                transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
            }

            if (_input.Default.RightClick.IsPressed()) 
            {
                transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
            }

            RaycastHit hit;
            Physics.SphereCast(transform.position, transform.localScale.z, Vector3.down, out hit);
            if (hit.collider != null) 
            {
                transform.parent = hit.collider.transform;
            }
            else 
            {
                transform.parent = null;
            }
        }
    }
}
