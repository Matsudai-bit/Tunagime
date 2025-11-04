using UnityEngine;

public class WalkStatePlayer : PlayerState
{
    private int m_walkSoundID = -1;
    public WalkStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        // 歩行状態の開始時にアニメーションを設定
        owner.GetAnimator().SetBool("Walk", true);

        // 歩行状態の開始時にサウンドを再生
        m_walkSoundID = SoundManager.GetInstance.RequestPlaying(SoundID.SE_PLAYER_WALK, true);
        if ( m_walkSoundID == SoundManager.ERROR_SOUND_ID)
        {
            Debug.LogError("WalkStatePlayer: 歩行サウンドの再生に失敗しました。");
        }

        OnFixedUpdateState();
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
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
            // 編む処理を試みる
            owner.TryKnit();

            if (CanSlide())
            {
                // サテン床上でスライド可能ならスライド状態に遷移
                owner.GetStateMachine().RequestStateChange(PlayerStateID.SLIPPER);
            }

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

            owner.TryPushBlock();

            owner.TryUnknit();

            if (CanSlide())
            {
                // サテン床上でスライド可能ならスライド状態に遷移
                owner.GetStateMachine().RequestStateChange(PlayerStateID.SLIPPER);
            }
        }

    }
    /// <summary>
    /// 歩行状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
     
        // プレイヤーの移動処理
        if (owner.IsMoving())
        {
            // 移動処理
            owner.Move();
            owner.SavePreviousMoveVelocity(); // 前回の速度を保存
        }
        else
        {
            // 歩行状態から待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
            // 移動を停止
            owner.StopMove();
        }
    }

    /// <summary>
    /// 歩行状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {
        // 歩行状態の終了時にアニメーションをリセット
        owner.GetAnimator().SetBool("Walk", false);

        // 歩行状態の終了時にサウンドを停止
        SoundManager.GetInstance.RequestStopping(m_walkSoundID);


    }

    public bool CanSlide()
    {

        if (MapData.GetInstance.CheckInnerGridPos(owner.GetGridPosition()) == false)
        {
            return false;
        }

            var stageGridData = MapData.GetInstance.GetStageGridData(); 
        var myGridPosFloor = stageGridData.GetFloorObject(owner.GetGridPosition());

        if (myGridPosFloor)
        {
            StageBlock stageBlock = myGridPosFloor.GetComponent<StageBlock>();
            if (stageBlock && stageBlock.GetBlockType() == StageBlock.BlockType.SATIN_FLOOR)
            {
                return true; // サテン床ならスライド可能
            }
        }
        return false;
    }

    
}
