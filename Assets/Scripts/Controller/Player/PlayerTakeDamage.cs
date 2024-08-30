using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public interface IPlayerDamageable
{
    int CurrentHealth { get; }
    bool IsDead { get; }
    void Damage(int damageAmount);
    void Die();
}

public class PlayerTakeDamage : MonoBehaviour, IPlayerDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _respawnDelayTime;
    [SerializeField] private Vector2 _respawnPosition;
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _angleInDegrees;

    private PlayerInput _playerInput;
    private Player _player;
    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rigidbody2D;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        CurrentHealth = _maxHealth;
        _respawnPosition = transform.position;
    }

    /// <summary>
    /// Handle the player take damage, reduce health and respawn the player after a certain delay
    /// </summary>
    /// <param name="currentHealth">Player's current health</param>
    /// <param name="amount">The amount of health the player loses</param>
    public void Damage(int amount)
    {
        CurrentHealth -= amount;
        StartCoroutine(Respawn(_respawnDelayTime));
    }

    /// <summary>
    /// Handle the player death, top the game and show the game over screen
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Calculate the knockback force and apply it to the player
    /// </summary>
    private void KnockBack()
    {
        // Calculate the vector direction of the knockback force by using the angle in degrees
        float x = Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad);
        float y = Mathf.Sin(_angleInDegrees * Mathf.Deg2Rad);
        Vector2 forceDirection = new Vector2(x, y).normalized;

        _rigidbody2D.AddForce(forceDirection * _knockBackForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Coroutine handles player respawning after taking damage.
    /// </summary>
    /// <param name="time">Respawn delay time</param>
    /// <returns></returns>
    private IEnumerator Respawn(float time)
    {
        _boxCollider2D.enabled = false;
        IsDead = true;
        _playerInput.DisableInput();
        KnockBack();

        yield return new WaitForSeconds(time / 2);
        _rigidbody2D.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(time);
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _boxCollider2D.enabled = true;
        transform.position = _respawnPosition;
        _playerInput.EnableInput();
        IsDead = false;
    }
}
