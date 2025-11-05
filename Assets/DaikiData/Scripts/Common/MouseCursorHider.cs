using UnityEngine;

public class MouseCursorHider : MonoBehaviour
{
    private void Awake()
    {
        // 1. カーソルを非表示にする
        Cursor.visible = false;

        // 2. カーソルをゲームウィンドウの中央にロックする
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("マウスカーソルを非表示にし、ロックしました。");
    }

}
