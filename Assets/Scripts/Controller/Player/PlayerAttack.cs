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
    [SerializeField] private GameObject _leftAttackObject;
    [SerializeField] private GameObject _rightAttackObject;
    [SerializeField] private float _attackCheckDistance;
    [SerializeField] private bool _drawGizmos;

    public bool IsOnTopOfEnemy { get; private set; }

    private void FixedUpdate()
    {
        Attack();
    }

    /// <summary>
    /// Check if the player is on top of the enemy
    /// </summary>
    public void Attack()
    {
        RaycastHit2D left = Physics2D.Raycast(_leftAttackObject.transform.position, Vector2.down, _attackCheckDistance, _enemyLayer);
        RaycastHit2D right = Physics2D.Raycast(_rightAttackObject.transform.position, Vector2.down, _attackCheckDistance, _enemyLayer);
        if (left.collider != null || right.collider != null)
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
        if (_drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_leftAttackObject.transform.position, Vector2.down * _attackCheckDistance);
            Gizmos.DrawRay(_rightAttackObject.transform.position, Vector2.down * _attackCheckDistance);
        }
    }
#endif
}
