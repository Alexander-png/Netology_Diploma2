using Platformer.EditorExtentions;
using Platformer.PlayerSystem;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerEnterNotifier : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private BoxCollider _trigger;

    public Vector3 Size => _trigger.size;
    public event EventHandler OnPlayerEnteredTrigger;

    private void Awake() =>
        FindTrigger();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            OnPlayerEnteredTrigger?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FindTrigger()
    {
        if (_trigger == null)
        {
            _trigger = GetComponent<BoxCollider>();
        }
    }

#if UNITY_EDITOR
    private void OnValidate() =>
        FindTrigger();

    private void OnDrawGizmos()
    {
        FindTrigger();
        Color c = Color.magenta;
        c.a = 0.1f;
        Gizmos.color = c;
        Gizmos.DrawCube(transform.position, Size);
    }
#endif
}
