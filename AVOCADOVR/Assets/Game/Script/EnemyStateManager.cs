/*
*   Name:菊川 誠
*   Script:敵の基本移動処理クラス
*   Day:19/06/17 
*/
using UnityEngine;

[System.Serializable]
public class EnemyAttackState {
    [Header("攻撃のステートの名前")]
    public string m_AttackStateName;
    [Header("確率判定時の最小の値")]
    public int m_Min;
    [Header("確率判定時の最大の値")]
    public int m_Max;
}

public class EnemyStateManager : MonoBehaviour {
    [Header("攻撃ステートの設定の配列※必ず設定")]
    [SerializeField] EnemyAttackState[] m_EnemyAttackState;

    [Header("戦闘態勢のフラグ")]
    [SerializeField] bool m_BattlePosture;
    [Header("現在のターゲットのプレイヤー")]
    [SerializeField] Transform m_Player;
    //止まる距離
    private float m_Dis = 1.0f;
    //自分のAnimator格納用
    private Animator m_MyAnim;
	void Update () {
        //自分のAnimatorが無い時
        if (!m_MyAnim) {
            //自分のアニメータを差し込む
            m_MyAnim = GetComponent<Animator>();
        }

        //もし、プレイヤーを見つけている時
        if (m_Player) {
            //自身と取得したオブジェクトの距離を取得
            float tmpDis = Vector3.Distance( transform.position, m_Player.transform.position);
            //プレイヤーに近すぎず、戦闘態勢のフラグが立っている時
            if ( tmpDis > m_Dis && m_BattlePosture) {
                //対象の位置の方向を向く
                transform.LookAt(m_Player.transform);
                //対象の方へ移動する。
                m_MyAnim.SetBool("Walk",true);
            //プレイヤーと近く、戦闘フラグが立ってない、もしくはそのどちらかが検知不可の時
            } else {
                //止まる
                m_MyAnim.SetBool("Walk", false);
                //もし、近すぎて止まっていた時
                if ( tmpDis <= m_Dis ) {
                    //Idleモーション中なら(ガバ判定無し正確で強すぎる())
                    /*if (!m_MyAnim.IsInTransition(0) && m_MyAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                        //対象の位置の方向を向く
                        transform.LookAt(m_Player.transform);
                    }*/
                    //ランダム変数を用意する
                    int rndnum = Random.Range(0,1000);
                    //配列の要素数分繰り返す
                    for (int i = 0; i < m_EnemyAttackState.Length; i++) {
                        //設定された攻撃ステートを確率ですべて確認する
                        SetNameAttack(m_EnemyAttackState[i].m_AttackStateName, rndnum, m_EnemyAttackState[i].m_Min, m_EnemyAttackState[i].m_Max);
                    }
                //そうではない時
                } else {
                    //全ての攻撃ステートは止まる
                    SetALLAttackOff();
                }
            }
        //見つけていない時
        } else {
            //止まる
            m_MyAnim.SetBool("Walk", false);
            //全ての攻撃ステートも止まる
            SetALLAttackOff();
        }
    }
    //全ての攻撃ステートをオフにする関数(面倒だったので用意)
    private void SetALLAttackOff() {
        //配列の要素数分繰り返す
        for (int i = 0; i < m_EnemyAttackState.Length; i++) {
            //攻撃ステートを全てオフにする。
            m_MyAnim.SetBool(m_EnemyAttackState[i].m_AttackStateName, false);
        }
    }
    //攻撃ステートの名前と確率を入れる事で、自動的に確率攻撃を行って呉れる関数
    private void SetNameAttack(string StateName,int Randam,int min,int max) {
        //もし、設定した数値から設定した数値の時は攻撃出来る
        if (Randam > min && Randam <= max) {
            m_MyAnim.SetBool(StateName, true);
        //それ以外の時は攻撃しない。
        } else {
            m_MyAnim.SetBool(StateName, false);
        }
    }

    //指定されたタグの中で最も近いものを取得する関数
    GameObject serchTag(GameObject nowObj,string tagName){
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        //string nearObjName = "";    //オブジェクト名称
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(tagName)){
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis){
                nearDis = tmpDis;
                //nearObjName = obs.name;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        //return GameObject.Find(nearObjName);
        return targetObj;
    }
    //外部から使用可能な戦闘態勢変異関数
    public void SetBattlePosture(Transform obj) {
        m_BattlePosture = true;
        m_Player = obj;
    }
}
