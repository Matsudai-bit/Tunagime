using UnityEngine;

public class PickUpStatePlayer : PlayerState
{
    public PickUpStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        owner.GetAnimator().SetTrigger("PickUp"); // 綿毛ボールを拾うアニメーションをトリガー


        
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // アニメーションの終了を待つ
        if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // アニメーションが終了したら綿毛ボールを拾う処理を実行
            PickUp();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

            // 綿毛ボールを持っている場合、アニメーションレイヤーを有効化
            owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 1);
        }

     
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

    private void PickUp()
    {

        var map = MapData.GetInstance; // マップを取得

        var checkPos = owner.GetForwardGridPos(); // チェックするグリッド位置

        TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;


        GameObject gameObj = tileObject.gameObject;
        var fluffBall = gameObj.GetComponent<StageBlock>(); // 綿毛ボールを取得
        map.GetStageGridData().RemoveGridData(fluffBall.GetGridPos()); // グリッドから綿毛ボールを削除
        Debug.Log(gameObj.name); // デバッグログに綿毛ボールの名前を出力

        fluffBall.SetActive(false); // 綿毛ボールを非アクティブにする

        owner.SetFluffBall(fluffBall); // プレイヤーに綿毛ボールをセット
    }


}
