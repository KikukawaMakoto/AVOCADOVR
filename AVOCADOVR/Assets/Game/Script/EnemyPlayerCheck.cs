/*
*   Name:菊川 誠
*   Script:敵のプレイヤーを検知する視野角のクラス
*   Day:19/06/17 
*/
using UnityEngine;

public class EnemyPlayerCheck : MonoBehaviour {
    private EnemyStateManager m_EnemyStateManager;
    private EnemyStateManager2 m_EnemyStateManager2;
    //敵の視野角にプレイヤーが侵入した時
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            //もし、自分のEnemyStateManagerが検知できていなければ
            if (!m_EnemyStateManager) {
                //検知する
                if (transform.parent.gameObject.GetComponent<EnemyStateManager>()) {
                    m_EnemyStateManager = transform.parent.gameObject.GetComponent<EnemyStateManager>();
                    //検知したScriptに情報を渡す。
                    m_EnemyStateManager.SetBattlePosture(other.transform);
                }
            }
            //もし、自分のEnemyStateManagerが検知できていなければ
            if (!m_EnemyStateManager2) {
                //検知する
                if (transform.parent.gameObject.GetComponent<EnemyStateManager2>()) {
                    m_EnemyStateManager2 = transform.parent.gameObject.GetComponent<EnemyStateManager2>();
                    //検知したScriptに情報を渡す。
                    m_EnemyStateManager2.SetBattlePosture(other.transform);
                }
            }
        }
    }
}
