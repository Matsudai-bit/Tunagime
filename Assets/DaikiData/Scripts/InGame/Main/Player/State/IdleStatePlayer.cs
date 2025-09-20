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
        // 移動を停止
        owner.StopMove();

    }

    public override void OnUpdateState() 
    {

        // 持ち運ぶオブジェクトを拾う処理を試みる
        owner.TryPickUp();
        // 持ち運ぶオブジェクトを置く処理を試みる
        owner.TryPutDown();

        if (owner.GetCarryingObject() != null)
        {
  
            // test
            owner.TryForwardFloorSetting();

            if (owner.GetCarryingObject().stageBlock.GetBlockType() == StageBlock.BlockType.CARRIABLE_CORE)
            {
                // ブロックを押す処理を試みる
                owner.TryForwardSlotSetting();
            }
        }
        else
        {
  
            // test
            owner.TryForwardObjectSetting();
            // 編む処理を試みる
            owner.TryKnit();
            owner.TryUnknit();
            owner.TryPushBlock();


        }

    }
    public override void OnFinishState()
    {
        //owner.ResetTargetObject();
    }


   

}
