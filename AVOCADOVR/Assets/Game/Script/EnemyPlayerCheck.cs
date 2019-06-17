/*
*   Name:菊川 誠
*   Script:敵のプレイヤーを検知する視野角のクラス
*   Day:19/06/17 
*/
using UnityEngine;

public class EnemyPlayerCheck : MonoBehaviour {
    private EnemyStateManager m_EnemyStateManager;
    //敵の視野角にプレイヤーが侵入した時
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            //もし、自分のEnemyStateManagerが検知できていなければ
            if (!m_EnemyStateManager) {
                //検知する
                m_EnemyStateManager = transform.parent.gameObject.GetComponent<EnemyStateManager>();
            }
            //検知したScriptに情報を渡す。
            m_EnemyStateManager.SetBattlePosture(other.transform);
        }
    }
}
