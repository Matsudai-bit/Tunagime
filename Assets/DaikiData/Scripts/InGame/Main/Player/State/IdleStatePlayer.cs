using UnityEngine;

public class IdleStatePlayer : PlayerState
{

    private int m_idleAnimationHash;

    public IdleStatePlayer(Player owner) : base(owner)
    {
       
    }


    public override void OnFixedUpdateState()
    {

        // プレイヤーの移動処理
        if (owner.IsMoving())
        {
            // 歩行状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.WALK);
        }
    }

    public override void  OnStartState() 
    {

    
    }

    public override void OnUpdateState() 
    {
        // 綿毛ボールを拾う処理を試みる
        owner.TryPickUp();
        // 綿毛ボールを置く処理を試みる
        owner.TryPickDown();

        if (owner.GetFluffBall() != null)
        {
            // 編む処理を試みる
            owner.TryKnot();
        }

    }
    public override void OnFinishState()
    {

    }


   

}
