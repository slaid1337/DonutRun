using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public virtual void OpenPanel()
    {
        _animator.Play("Open");
    }

    public virtual void ClosePanel()
    {
        _animator.Play("Close");
    }
}