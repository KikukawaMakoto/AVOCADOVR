using UnityEngine;

public class UIHPWindow : MonoBehaviour {
    [Header("ターゲットとなるプレイヤー格納用")]
    [SerializeField] GameObject m_TGPlayer;
    void Start(){
        PlyerSearch();
    }
    void Update () {
        //もし、ターゲットが取得できていれば
        if (m_TGPlayer) {
            //そのターゲットへこのオブジェクトを向ける
            transform.LookAt(m_TGPlayer.transform);
        //もし、なんらかの影響で失敗したら再度サーチ。
        } else {
            PlyerSearch();
        }
    }
    private void PlyerSearch() {
        //メインカメラのみを取得
        m_TGPlayer = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
