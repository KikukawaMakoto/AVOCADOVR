/*
*   Name:菊川 誠
*   Script:菊川の作成したVRでの簡易移動の管理用クラス(別途拡張済みSteamVRInputManagerスクリプトが必要です)
*   Day:19/06/06 
*/
using UnityEngine;

namespace MKTVRManager {
    public class VRSimpleMove : MonoBehaviour {
        [Header("アバターのAnimator(あれば移動に応じてモーションする)")]
        [SerializeField] Animator m_AvatorAnim;
        [Header("座標移動に使う手の名前")]
        [SerializeField] string m_MoveHandName;
        [Header("回転移動に使う手の名前")]
        [SerializeField] string m_RotHandName;
        [Header("VRのボタン管理スクリプトを格納")]
        [SerializeField] SteamVRInputManager m_KSteamVRManager;
        [Header("回転速度")]
        [SerializeField] float m_RotSpeed = 20.0f;
        [Header("移動速度")]
        [SerializeField] float m_MoveSpeed = 0.01f;

        [Header("VRカメラのトランスフォーム格納用")]
        [SerializeField] Transform m_VRCamera;

        [Header("メインカメラ格納用")]
        [SerializeField] Transform m_MainCamera;
        [Header("プレイヤー回転格納用")]
        [SerializeField] Transform m_PlayerRot;
        [Header("ルームScale情報取得格納用")]
        [SerializeField] Transform m_RoomScaleInfo;
        private bool m_JumpFlag;
        void Update() {
            VRPlayerRot();
            VRPlayerMove();
        }
        private void VRPlayerRot() {
            //左移動
            if (Input.GetKeyDown(KeyCode.Q) || m_KSteamVRManager.GetVRButtonDown(m_RotHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).x < 0 && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).y < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).y > -0.7f) {
                //まずメインカメラの情報をプレイヤー回転格納用に渡す
                m_PlayerRot.position = m_MainCamera.position;
                //次にルームScaleの情報をルームScale情報取得格納用に渡す
                m_RoomScaleInfo.position = transform.position;
                m_RoomScaleInfo.rotation = transform.rotation;
                //プレイヤー回転格納用を回転させる。
                m_PlayerRot.Rotate(0, -m_RotSpeed, 0);
                //出来上がったプレイヤーを中心として回転した情報を本体に渡す。
                transform.position = m_RoomScaleInfo.position;
                transform.rotation = m_RoomScaleInfo.rotation;
            }
            //右移動
            if (Input.GetKeyDown(KeyCode.E) || m_KSteamVRManager.GetVRButtonDown(m_RotHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).x > 0 && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).y < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_RotHandName).y > -0.7f) {
                //まずメインカメラの情報をプレイヤー回転格納用に渡す
                m_PlayerRot.position = m_MainCamera.position;
                //次にルームScaleの情報をルームScale情報取得格納用に渡す
                m_RoomScaleInfo.position = transform.position;
                m_RoomScaleInfo.rotation = transform.rotation;
                //プレイヤー回転格納用を回転させる。
                m_PlayerRot.Rotate(0, m_RotSpeed, 0);
                //出来上がったプレイヤーを中心として回転した情報を本体に渡す。
                transform.position = m_RoomScaleInfo.position;
                transform.rotation = m_RoomScaleInfo.rotation;
            }
            //ジャンプ
            if (Input.GetKeyDown(KeyCode.Space) || m_KSteamVRManager.GetVRButtonDown(m_MoveHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y > 0 && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x > -0.7f) {
                if (GetComponent<Rigidbody>() && !m_JumpFlag) {
                    GetComponent<Rigidbody>().AddForce(0.0f,200.0f,0.0f);
                    m_JumpFlag = true;
                }
            }

        }
        private void VRPlayerMove() {
            if (m_AvatorAnim) {
                m_AvatorAnim.SetBool("Walk", false);
                m_AvatorAnim.SetBool("Back", false);
                m_AvatorAnim.SetBool("WalkL", false);
                m_AvatorAnim.SetBool("WalkR", false);
            }
            Transform trans = m_VRCamera;
            //Y軸のみ反映させて、XとZ軸は考慮しない
            trans.rotation = new Quaternion(0, trans.rotation.y, 0, trans.rotation.w);
            //前移動
            if (Input.GetKey(KeyCode.W) || m_KSteamVRManager.GetVRButton(m_MoveHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y > 0 && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x > -0.7f) {
                //transform.position += new Vector3(0.0f, 0.0f, m_MoveSpeed);
                transform.position += trans.transform.forward * m_MoveSpeed;
                if (m_AvatorAnim) {
                    m_AvatorAnim.SetBool("Walk",true);
                }
            }
            //後ろ移動
            if (Input.GetKey(KeyCode.S) || m_KSteamVRManager.GetVRButton(m_MoveHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y < 0 && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x > -0.7f) {
                //transform.position -= new Vector3(0.0f, 0.0f, m_MoveSpeed);
                transform.position -= trans.transform.forward * m_MoveSpeed;
                if (m_AvatorAnim) {
                    m_AvatorAnim.SetBool("Back", true);
                }
            }
            //左移動
            if (Input.GetKey(KeyCode.A) || m_KSteamVRManager.GetVRButton(m_MoveHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x < 0 && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y > -0.7f) {
                // transform.position -= new Vector3(m_MoveSpeed, 0.0f, 0.0f);
                transform.position -= trans.transform.right * m_MoveSpeed;
                if (m_AvatorAnim) {
                    m_AvatorAnim.SetBool("WalkL",true);
                }
            }
            //右移動
            if (Input.GetKey(KeyCode.D) || m_KSteamVRManager.GetVRButton(m_MoveHandName, "TouchPad") && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).x > 0 && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y < 0.7f && m_KSteamVRManager.GetTouchPadPos(m_MoveHandName).y > -0.7f) {
                //transform.position += new Vector3( m_MoveSpeed, 0.0f,0.0f);
                transform.position += trans.transform.right * m_MoveSpeed;
                if (m_AvatorAnim) {
                    m_AvatorAnim.SetBool("WalkR",true);
                }
            }
        }

        //何かと当たった時
        void OnCollisionEnter(Collision collision) {
            //再びjump可能になる
            m_JumpFlag = false;
        }
    }
}