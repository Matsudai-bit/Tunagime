using UnityEngine;

public class PickUpStatePlayer : PlayerState
{
    private int m_pickUpAnimationHash;

    public PickUpStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        owner.GetAnimator().SetTrigger("PickUp"); // 持ち上げるオブジェクトを拾うアニメーションをトリガー

        m_pickUpAnimationHash = Animator.StringToHash("Normal.PickUp"); // アニメーションのハッシュを取得

    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // 現在のアニメーションのハッシュを取得
        int currentHash = owner.GetAnimator().GetCurrentAnimatorStateInfo(0).fullPathHash;

        // アニメーションの終了を待つ
        if (currentHash == m_pickUpAnimationHash &&
            owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // アニメーションが終了したら持ち上げるオブジェクトを拾う処理を実行
            PickUp();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

            // 持ち上げるオブジェクトを持っている場合、アニメーションレイヤーを有効化
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
        var carryingObject = gameObj.GetComponent<Carryable>(); // 持ち上げるオブジェクトを取得
        if (carryingObject == null)
        {
            Debug.LogError("Carryable component not found on the object.");
            return; // 持ち上げるオブジェクトが見つからない場合は処理を中断
        }
        // 持ち上げるオブジェクトのグリッド位置を取得
        var stageBlock = carryingObject.stageBlock;

        // グリッドから持ち上げるオブジェクトを削除
        map.GetStageGridData().RemoveGridData(stageBlock.GetGridPos()); 

        // デバッグログに持ち上げるオブジェクトの名前を出力
        Debug.Log(gameObj.name); 

        // 持ち上げるオブジェクトを非アクティブにする
        stageBlock.SetActive(false);

        // 持ち上げる
        carryingObject.OnPickUp();

        // プレイヤーに持ち上げるオブジェクトをセット
        owner.SetCarryingObject(carryingObject); 
    }


}
