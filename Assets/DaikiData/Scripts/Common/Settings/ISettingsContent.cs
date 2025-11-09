using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 設定画面のコンテンツの基底クラス
/// </summary>
public abstract class SettingsContent : MonoBehaviour , ISettingsContent
{
    // 初期化処理
    public abstract void Initialize();

    // 操作可能かどうか
    public abstract bool IsInteractable();

    // 操作可能にする
    public abstract void EnableInteractable();
    // 操作不可能にする
    public abstract void DisableInteractable();

    public abstract void SaveSettings();
    public abstract void LoadSettings();
    public abstract void ResetSettings();
    public abstract void OnUpdate();

    public abstract void CancelSettings();

    public abstract void OnNavigate(InputAction.CallbackContext context);

    public abstract void OnSubmit(InputAction.CallbackContext context);
}

/// <summary>
/// 設定画面のコンテンツのインターフェース
/// </summary>
public interface ISettingsContent 
{
    // 初期化処理
    public void Initialize();

    // 操作可能かどうか
    public bool IsInteractable();

    // 操作可能にする
    public void EnableInteractable();

    // 操作不可能にする
    public void DisableInteractable();

    // 設定内容を保存する
    public void SaveSettings();

    // 設定内容を読み込む
    public void LoadSettings();

    // 設定内容をリセットする
    public void ResetSettings();

    // 毎フレームの更新処理
    public void OnUpdate();

    // 設定のキャンセル処理
    public void CancelSettings();

    // ナビゲート入力の処理
    public void OnNavigate(InputAction.CallbackContext context);

    // サブミット入力の処理
    public void OnSubmit(InputAction.CallbackContext context);
}
