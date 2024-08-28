using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAnimationStrategy
{
    void PlayAnimation(Animator animator);
}

public interface IPlayerAnimation
{
    void PlayAnimation(IPlayerAnimationStrategy animationStrategy);
}

public class RunAnimationStrategy: IPlayerAnimationStrategy
{
    private bool _value;

    public RunAnimationStrategy(bool value)
    {
        _value = value;
    }

    public void PlayAnimation(Animator animator)
    {
        animator.SetBool("Run", _value);
    }
}

public class JumpAnimationStrategy : IPlayerAnimationStrategy
{
    private bool _value;

    public JumpAnimationStrategy(bool value)
    {
        _value = value;
    }

    public void PlayAnimation(Animator animator)
    {
        animator.SetBool("Jump", _value);
    }
}

public class DoubleJumpAnimationStrategy : IPlayerAnimationStrategy
{
    public void PlayAnimation(Animator animator)
    {
        animator.SetTrigger("DoubleJump");
    }
}

public class FallAnimationStrategy : IPlayerAnimationStrategy
{
    private bool _value;

    public FallAnimationStrategy(bool value)
    {
        _value = value;
    }

    public void PlayAnimation(Animator animator)
    {

        animator.SetBool("Fall", _value);
    }
}

public class WallSlidingAnimationStrategy : IPlayerAnimationStrategy
{
    private bool _value;

    public WallSlidingAnimationStrategy(bool value)
    {
        _value = value;
    }

    public void PlayAnimation(Animator animator)
    {

        animator.SetBool("WallSliding", _value);
    }
}

public class TakeDamageAnimationStrategy : IPlayerAnimationStrategy
{
    public void PlayAnimation(Animator animator)
    {
        animator.SetTrigger("TakeDamage");
    }
}

public class AppearAnimationStrategy : IPlayerAnimationStrategy
{

    public void PlayAnimation(Animator animator)
    {
        animator.SetTrigger("Appear");
    }
}

public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    
    public void PlayAnimation(IPlayerAnimationStrategy animationStrategy)
    {
        animationStrategy.PlayAnimation(_animator);
    }
}
