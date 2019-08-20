
using UnityEngine;
using UnityEngine.UI;

public class Parameta : MonoBehaviour {
    [Header("体力")]
    [SerializeField] float m_HP = 100.0f;
    //最大HP
    private float m_MaxHP;
    //復活時に回復できるHP
    private float m_HeelHP;
    //死亡フラグ
    private bool m_DeadFlag = false;
    [Header("攻撃")]
    [SerializeField] float m_ATK = 10.0f;
    [Header("防御力")]
    [SerializeField] float m_DEF = 5.0f;
    [Header("移動力")]
    [SerializeField] float m_Speed = 1.0f;
    [Header("死亡回数")]
    [SerializeField] int m_DeadCnt = 0;
    [Header("HPバー※無ければ処理されない")]
    [SerializeField] Slider m_HPBar;
    [Header("Player統括オブジェクト※なければリスポーンされない")]
    [SerializeField] Transform m_PlayerHeadObj;
    void Start() {
        m_HeelHP = m_HP / 2;
        m_MaxHP = m_HP;
    }
    void Update () {
        //最大HPより現在HPが多ければ
        if (m_HP > m_MaxHP) {
            //補正する。
            m_HP = m_MaxHP;
        }

        //HPバーが存在する時。
        if (m_HPBar) {
            //HPの値を送信する。
            m_HPBar.value = m_HP / m_MaxHP;
        }
	}
    //HPにアクセスし、外部からダメージや回復を行える関数
    public void HPAccess(float access,bool damage) {
        //Damageフラグがオンの場合、ダメージAccessと判定
        if (damage) {
            //もし、値から防御値を引いて0以上になる場合
            if ((m_DEF - access) < 0.0f) {
                //値を設定する。
                access = access - m_DEF;
            //0以上にならない場合、
            } else {
                //強制的に0にする。
                access = 0.0f;
            }
            //最終的に割り出された値を引く。
            m_HP -= access;
            //HPが0以下になった時、死亡フラグが立っていなければ
            if (m_HP <= 0.0f && !m_DeadFlag) {
                //死亡回数を増やす。
                m_DeadCnt++;
                //死亡フラグを立てる。
                m_DeadFlag = true;
            }
            //ダメージでない場合、
        } else {
            //値を足す。
            m_HP += access;
            //最大HPより現在HPが多ければ
            if ( m_HP > m_MaxHP ) {
                //補正する。
                m_HP = m_MaxHP;
            }
        }
    }
    //リスポーン地点があればリスポーンを行い、無ければ復活だけさせる事の可能な関数
    public void RevivalAccess(GameObject ResPos = null) {
        //リスポーン地点が存在する時、
        if (ResPos && m_PlayerHeadObj) {
            //リスポーンさせる。
            m_PlayerHeadObj.position = ResPos.transform.position;
            m_PlayerHeadObj.rotation = ResPos.transform.rotation;
        }
        //Playerを復活させ、
        m_HP = m_HeelHP;
        //死亡フラグをオフにする。
        m_DeadFlag = false;
    }
    //死亡フラグ取得用
    public bool GetDeadFlag() {
        return m_DeadFlag;
    }
    //攻撃力取得用
    public float GetATK() {
        return m_ATK;
    }
}
