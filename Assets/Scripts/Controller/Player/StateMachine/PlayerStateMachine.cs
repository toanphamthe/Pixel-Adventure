using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }

    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerJumpState jumpState;
    public PlayerFallState fallState;
    public PlayerDoubleJumpState doubleJumpState;
    public PlayerWallSlideState wallSlideState;
    public PlayerTakeDamageState takeDamageState;

    public PlayerStateMachine(Player player)
    {
        this.idleState = new PlayerIdleState(player);
        this.walkState = new PlayerWalkState(player);
        this.jumpState = new PlayerJumpState(player);
        this.fallState = new PlayerFallState(player);
        this.doubleJumpState = new PlayerDoubleJumpState(player);
        this.wallSlideState = new PlayerWallSlideState(player);
        this.takeDamageState = new PlayerTakeDamageState(player);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();
    }

    public void Update()
    {
        CurrentState.UpdateState();
    }

    public void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }
}
