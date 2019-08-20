/*
*   Name:菊川 誠
*   Script:仕様に合わせた敵の基本移動処理クラス
*   Day:19/07/01 
*/
using UnityEngine;
using UnityEngine.AI;

//待機
//特殊待機
//走る
//突進(攻撃)

public class EnemyStateManager2 : MonoBehaviour {
    [Header("プレイヤーを見つけているかいないかのフラグ")]
    [SerializeField] bool m_PlayerLookFlag;
    [Header("プレイヤーを見続ける時間")]
    [SerializeField] float m_PlayerLookTime = 3.5f;

    [Header("プレイヤーに与えるダメージ量")]
    [SerializeField] float m_ATK = 10.0f;
    [Header("現在のターゲットのプレイヤー")]
    [SerializeField] Transform m_Player;
    [Header("爆死パーティクル※あれば入れる")]
    [SerializeField] GameObject m_BomEffect;
    //待機時の自動回転の種類
    [SerializeField] int m_RotType;
    //辺りを見回す種類を変更するタイミング
    private float m_LookTime = 5.0f;
    //接近禁止距離
    private float m_MoveStopDis = 7.0f;
    //攻撃距離
    private float m_AttackDis = 2.5f;
    //自分のAnimator格納用
    private Animator m_MyAnim;
    void Update() {
        //自分のAnimatorが無い時
        if (!m_MyAnim) {
            //自分のアニメータを差し込む
            m_MyAnim = GetComponent<Animator>();
        }
        //もし、プレイヤーを見つけている時
        if (m_PlayerLookFlag) {
            //自身と取得したオブジェクトの距離を取得
            float tmpDis = Vector3.Distance(transform.position, m_Player.transform.position);
            //一定時間はプレイヤーを目視し続ける。
            if (m_PlayerLookTime > 0.0f) {
                //もし、プレイヤーが離れすぎていた時
                if (tmpDis > m_MoveStopDis) {
                    //NaviMeshを利用し、プレイヤーの位置へ向き
                    /*gameObject.AddComponent<NavMeshAgent>();
                    GetComponent<NavMeshAgent>().SetDestination(m_Player.position);
                    GetComponent<NavMeshAgent>().speed = 0.0f;*/
                    //走るAnimationを再生させ移動させる。
                    //プレイヤーの方へ向け、
                    transform.LookAt(m_Player.transform);
                    m_MyAnim.SetBool("Move", true);
                    m_MyAnim.SetBool("LookIdle", false);
                    m_MyAnim.SetBool("AttackMove", false);
                    //離れすぎてない時は目視を続ける
                } else {
                    //対象の位置の方向を向く
                    transform.LookAt(m_Player.transform);
                    //特殊待機Animationを実行※未作成
                    m_MyAnim.SetBool("Move", false);
                    m_MyAnim.SetBool("LookIdle", true);
                    m_MyAnim.SetBool("AttackMove", false);
                    //見続ける時間は減っていく…
                    m_PlayerLookTime -= Time.deltaTime;
                    //もし、プレイヤーが近づきすぎた時
                    if (tmpDis < m_AttackDis) {
                        //プレイヤーの方へ向け、
                        transform.LookAt(m_Player.transform);
                        //強制的に見続ける事をやめさせる。
                        m_PlayerLookTime = 0.0f;
                    }
                }
            //もし、プレイヤーが近づきすぎるか、一定時間たった時、
            } else {
                //プレイヤーめがけて突進する(ここに突進Animation)。
                m_MyAnim.SetBool("Move", false);
                m_MyAnim.SetBool("LookIdle", false);
                m_MyAnim.SetBool("AttackMove", true);
            }
        //プレイヤーを見つけていない時
        } else {
            //Animationを待機状態に
            m_MyAnim.SetBool("Move", false);
            m_MyAnim.SetBool("LookIdle", false);
            m_MyAnim.SetBool("AttackMove", false);
            //辺りを見回す種類を変更するタイミング時間を減らしていく
            m_LookTime -= Time.deltaTime;
            //辺りを見回す種類を変更するタイミングが来たら
            if (m_LookTime <= 0.0f) {
                //ランダムに、自動回転の種類を変更
                m_RotType = Random.Range(0, 3);
                //辺りを見回す種類を変更するタイミング時間を初期値に戻す。
                m_LookTime = 5.0f;
            }
            //ターゲット位置用の疑似座標を作成
            Vector3 target = transform.position;

            //待機時の自動回転の種類に合わせて処理を変更
            switch (m_RotType) {
                case 0:
                    target += new Vector3(0.0f, 0.0f, 5.0f);
                    break;
                case 1:
                    target += new Vector3(0.0f, 0.0f, -5.0f);
                    break;
                case 2:
                    target += new Vector3(5.0f, 0.0f, 0.0f);
                    break;
                case 3:
                    target += new Vector3(-5.0f, 0.0f, 0.0f);
                    break;
            }
            //正規化用の座標変数を作成
            Vector3 targetPositon = target;
            // 高さがずれていると体ごと上下を向いてしまうので便宜的に高さを統一
            if (transform.position.y != target.y) {
                targetPositon = new Vector3(target.x, transform.position.y, target.z);
            }
            //ターゲットのとの計算を行い、
            Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
            //ターゲットの方へ向く
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.5f);
        }
    }
    //外部から使用可能な戦闘態勢変異関数
    public void SetBattlePosture(Transform obj) {
        m_PlayerLookFlag = true;
        m_Player = obj;
    }
    //突進後、壁かプレイヤーに当たれば爆死(プレイヤーにはダメージ)。
    void OnCollisionEnter (Collision other) {
        //突進中に
        if (m_PlayerLookTime <= 0.0f) {
            //壁か、プレイヤーに当たった時は
            if (other.gameObject.layer == LayerMask.NameToLayer("Kabe")|| other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                //パーティクルがあれば出す
                if (m_BomEffect) {
                    GameObject Effect = (GameObject)Instantiate(m_BomEffect,transform.position, transform.rotation);
                }
                //対象にパラメータScriptがあれば、※未作成
                if (other.gameObject.GetComponent<Parameta>()) {
                    //対象のHPを減らす。
                    other.gameObject.GetComponent<Parameta>().HPAccess(m_ATK,true);
                }
                //この敵は殺す。
                Destroy(gameObject);
            }
        }
    }
}
