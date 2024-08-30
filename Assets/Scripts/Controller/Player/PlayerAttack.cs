using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
    bool IsOnTopOfEnemy { get; }
    void Attack();
}

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _attackObject;
    [SerializeField] private float _attackRadius;

    public bool IsOnTopOfEnemy { get; private set; }

    private void FixedUpdate()
    {
        Attack();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Attack()
    {
        Collider2D collider = Physics2D.OverlapCircle(_attackObject.transform.position, _attackRadius, _enemyLayer);
        if (collider != null)
        {
            IsOnTopOfEnemy = true;
        }
        else
        {
            IsOnTopOfEnemy = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackObject.transform.position, _attackRadius);
    }
#endif
}
