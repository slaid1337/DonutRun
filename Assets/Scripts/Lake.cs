using UnityEngine;

public class Lake : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void FixedUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.x = _player.position.x;

        transform.position = newPos;
    }
}
