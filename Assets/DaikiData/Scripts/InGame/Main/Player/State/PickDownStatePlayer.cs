using UnityEngine;

public class PickDownStatePlayer : PlayerState
{
    public PickDownStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        //owner.GetAnimator().SetTrigger("PickDown"); // 綿毛ボールを拾うアニメーションをトリガー


        
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // アニメーションの終了を待つ
        //if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
            // アニメーションが終了したら綿毛ボールを拾う処理を実行
            PickDown();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


            // 綿毛ボールを持っている場合、アニメーションレイヤーを有効化
            owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 0);
        //}

     
    }
    /// <summary>
    /// 歩行状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
     

    }

    /// <summary>
    /// 歩行状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {

    }

    private void PickDown()
    {

        var map = MapData.GetInstance; // マップを取得

        // 最も近いグリッド位置の取得
        var closestPos = map.GetClosestGridPos(owner.transform.position);
        owner.GetFluffBall().UpdatePosition(closestPos); // 綿毛ボールの位置を更新

        owner.GetFluffBall().SetActive(true); // 綿毛ボールをアクティブにする

        // グリッドデータに綿毛ボールを配置
        map.GetStageGridData().TryPlaceTileObject(closestPos, owner.GetFluffBall().GetComponent<GameObject>());
        owner.SetFluffBall(null); // 綿毛ボールを解放
    }


}
