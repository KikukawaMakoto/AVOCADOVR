/*
*   Name:菊川 誠
*   Script:SteamVR 2.0のボタン命令まとめスクリプト(HTC-viveに現在対応)
*   Day:19/04/09
*/
using UnityEngine;
using Valve.VR;

namespace MKTVRManager {
    [System.Serializable]
    public class HandSetting {
        [Header("使う腕側のコントローラー名(分かり易く任意設定する用)")]
        public string m_HandName = "Any";
        [Header("使う腕側の割り当て(任意設定)")]
        public SteamVR_Input_Sources m_HandType;
        [Header("X座標")]
        public float m_TouchPosX = 0.0f;
        [Header("Y座標")]
        public float m_TouchPosY = 0.0f;
    }
    [System.Serializable]
    public class ActionNames {
        [Header("アクションの名前(分かり易く任意設定する用)")]
        public string m_ActionName = "Any";
        [Header("アクションの割り当て(任意設定)")]
        public SteamVR_Action_Boolean m_Action;
    }

    public class SteamVRInputManager : MonoBehaviour {
        [Header("どちらの手かの取得用(全て任意割り当て)")]
        [SerializeField] HandSetting[] m_HandType;
        [Header("アクションの取得用(全て任意割り当て)")]
        [SerializeField] ActionNames[] m_Actions;

        [Header("TouchPadの座標取得用(任意割り当て)")]
        [SerializeField] SteamVR_Action_Vector2 m_actionVector2;

        //基本のアップデートでは、Touch座標の取得を行って、デバッグログを出力させています。
        void Update() {
            for (int i = 0; i < m_Actions.Length; i++) {
                for (int h = 0; h < m_HandType.Length; h++) {
                    //座標の取得
                    m_HandType[h].m_TouchPosX = m_actionVector2.GetAxis(m_HandType[h].m_HandType).x;
                    m_HandType[h].m_TouchPosY = m_actionVector2.GetAxis(m_HandType[h].m_HandType).y;
                    
                    //以下デバッグログ用処理
                    //---------------------------------------------------------------------------------------------------------------
                    //何かボタンが押されていた時
                    if (GetVRButtonDown(m_HandType[h].m_HandName, m_Actions[i].m_ActionName)) {
                        //Debug.Log(m_Actions[i].m_Action.GetShortName() + "のボタンが押された！");
                        Debug.Log(m_HandType[h].m_HandName + "の" + m_Actions[i].m_ActionName + "のボタンが押された！");
                    }
                    //何かボタンが離されていた時
                    if (GetVRButtonUp(m_HandType[h].m_HandName, m_Actions[i].m_ActionName)) {
                        //Debug.Log(m_Actions[i].m_Action.GetShortName() + "のボタンが離された！");
                        Debug.Log(m_HandType[h].m_HandName + "の" + m_Actions[i].m_ActionName + "のボタンが離された！");
                    }
                    //---------------------------------------------------------------------------------------------------------------
                }
            }

        }
        //VRコントローラーで割り当て設定したボタンと同じ名前のボタンが押されたらTrueを返す関数。
        public bool GetVRButtonDown(string handtype = "null" , string actionname = "null") {
            bool flag = false;
            //全てのアクションの名前を検索する
            for (int i = 0; i < m_Actions.Length; i++) {
                //アクションの名前が引数と一致している時、
                if (m_Actions[i].m_ActionName == actionname) {
                    //更に、ハンドタイプ(どちらの手なのか)の有無も確認して、処理を変更
                    if (m_HandType[0].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetStateDown(m_HandType[0].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                    } else if (m_HandType[1].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetStateDown(m_HandType[1].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                        //入力間違い時
                    } else {
                        //どちらも判定する
                        for (int h = 0; h < m_HandType.Length; h++) {
                            //押されているかどうかを判定
                            if (m_Actions[i].m_Action.GetStateDown(m_HandType[h].m_HandType)) {
                                //押されていればTrue
                                flag = true;
                            }
                        }

                    }

                }
            }
            //結果をフラグで渡す。
            return flag;
        }
        //VRコントローラーで割り当て設定したボタンと同じ名前のボタンが押し続けられている時、Trueを返す関数。
        public bool GetVRButton(string handtype = "null" , string actionname = "null") {
            bool flag = false;
            //全てのアクションの名前を検索する
            for (int i = 0; i < m_Actions.Length; i++) {
                //アクションの名前が引数と一致している時、
                if (m_Actions[i].m_ActionName == actionname) {
                    //更に、ハンドタイプ(どちらの手なのか)の有無も確認して、処理を変更
                    if (m_HandType[0].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetState(m_HandType[0].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                    } else if (m_HandType[1].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetState(m_HandType[1].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                        //入力間違い時
                    } else {
                        //どちらも判定する
                        for (int h = 0; h < m_HandType.Length; h++) {
                            //押されているかどうかを判定
                            if (m_Actions[i].m_Action.GetState(m_HandType[h].m_HandType)) {
                                //押されていればTrue
                                flag = true;
                            }
                        }

                    }

                }
            }
            //結果をフラグで渡す。
            return flag;
        }
        //VRコントローラーで割り当て設定したボタンと同じ名前のボタンを離した時、Trueを返す関数。
        public bool GetVRButtonUp(string handtype = "null" , string actionname = "null") {
            bool flag = false;
            //全てのアクションの名前を検索する
            for (int i = 0; i < m_Actions.Length; i++) {
                //アクションの名前が引数と一致している時、
                if (m_Actions[i].m_ActionName == actionname) {
                    //更に、ハンドタイプ(どちらの手なのか)の有無も確認して、処理を変更
                    if (m_HandType[0].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetStateUp(m_HandType[0].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                    } else if (m_HandType[1].m_HandName == handtype) {
                        //押されているかどうかを判定
                        if (m_Actions[i].m_Action.GetStateUp(m_HandType[1].m_HandType)) {
                            //押されていればTrue
                            flag = true;
                        }
                        //入力間違い時
                    } else {
                        //どちらも判定する
                        for (int h = 0; h < m_HandType.Length; h++) {
                            //押されているかどうかを判定
                            if (m_Actions[i].m_Action.GetStateUp(m_HandType[h].m_HandType)) {
                                //押されていればTrue
                                flag = true;
                            }
                        }

                    }

                }
            }
            //結果をフラグで渡す。
            return flag;
        }

        //コントローラーのTouchPadの座標を取得できる関数
        public Vector2 GetTouchPadPos(string handtype = "null") {
            Vector2 vec2 = new Vector2(0.0f,0.0f);
            //更に、ハンドタイプ(どちらの手なのか)の有無も確認して、処理を変更
            if (m_HandType[0].m_HandName == handtype) {
                vec2 = new Vector2(m_HandType[0].m_TouchPosX, m_HandType[0].m_TouchPosY);
            } else if (m_HandType[1].m_HandName == handtype) {
                vec2 = new Vector2(m_HandType[1].m_TouchPosX, m_HandType[1].m_TouchPosY);
                //入力間違い時
            } else {
                //特に何もしない
            }
            return vec2;
        }
    }
}
