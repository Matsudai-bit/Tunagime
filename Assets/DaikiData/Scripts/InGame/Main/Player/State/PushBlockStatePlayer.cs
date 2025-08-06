using UnityEngine;

public class PushBlockStatePlayer : PlayerState
{
    public PushBlockStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
   
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
       

    }
    /// <summary>
    /// 歩行状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {

        // ブロックを押す処理
        Push(); 

        // 歩行状態から待機状態に遷移
        owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        
    }

    /// <summary>
    /// 歩行状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {
        // 歩行状態の終了時にアニメーションをリセット
        owner.GetAnimator().SetBool("Walk", false);
    }

    /// <summary>
    /// ブロックを押す処理
    /// </summary>
    private void Push()
    {
        // マップの取得
        var map = MapData.GetInstance;
        // レイの飛ばす距離を設定
        float rayDistance = (float)(map.GetCommonData().width) / 2.0f;

        // レイキャストを使用して、プレイヤーの前方にあるオブジェクトを検出
        RaycastHit hit;
        if (Physics.Raycast(new Ray(owner.transform.position, owner.transform.forward), out hit, rayDistance))
        {
            // レイが当たったオブジェクトがステージブロックであるかチェック
            if (hit.collider.gameObject?.GetComponent<FeltBlock>() != null)
            {
                var feltBlock = hit.collider.gameObject.GetComponent<FeltBlock>();

                // ブロックを押す処理
                feltBlock.Move(owner.GetForwardDirection());
            }
        }
    }

}
