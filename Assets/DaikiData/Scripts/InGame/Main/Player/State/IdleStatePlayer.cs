using UnityEngine;

public class IdleStatePlayer : PlayerState
{

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

    }
    public override void OnFinishState()
    {

    }


   

}
