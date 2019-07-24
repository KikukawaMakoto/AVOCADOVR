/*
*   Name:菊川 誠
*   Script:菊川の作成した疑似VRIK設定管理用クラス(別途拡張済みIKスクリプトが必要です)
*   Day:19/06/04 
*/
using UnityEngine;

namespace MKTVRManager {
    public class VRIKManager : MonoBehaviour {
        //以下毎度設定必須
        [Header("モデルの頭ボーン格納用")]
        [SerializeField] Transform m_Head;
        [Header("アバターモデル格納用")]
        [SerializeField] Transform m_AvaterModel;
        [Header("移動させる時の距離")]
        [SerializeField] float m_MoveDistance = 0.5f;
        [Header("ラープを利用する場合の補間速度")]
        [SerializeField] float m_Speed = 50.0f;

        //以下Prefabにできる物
        [Header("CameraRig格納用")]
        [SerializeField] Transform m_CameraRig;
        [Header("カメラの回転用オブジェクト格納用")]
        [SerializeField] Transform m_CameraRot;
        [Header("メインカメラ格納用")]
        [SerializeField] Transform m_CameraPos;
        [Header("足の座標を管理しているオブジェクト格納用")]
        [SerializeField] Transform m_FootPosSet;
        [Header("左足が向かうべき目的座標格納用")]
        [SerializeField] Transform m_FootLDestination;
        [Header("右足が向かうべき目的座標格納用")]
        [SerializeField] Transform m_FootRDestination;
        [Header("必ず一致させる両足の座標格納用")]
        [SerializeField] Transform m_ALLFootYPos;
        [Header("左足の同期用オブジェクト格納用")]
        [SerializeField] Transform m_FootL;
        [Header("右足の同期用オブジェクト格納用")]
        [SerializeField] Transform m_FootR;
        void Update() {
            //最初に、カメラにモデル自身が追従するように設定を施す。
            m_AvaterModel.position = m_CameraRig.position - (m_Head.position - m_AvaterModel.position);
            m_AvaterModel.rotation = Quaternion.Slerp(m_AvaterModel.rotation, m_CameraRig.rotation, 0.8f * Time.deltaTime);
            //次に、無条件でカメラの回転値を回転用オブジェクトに同期します。
            m_CameraRot.rotation = m_CameraPos.rotation;
            //次に、足の座標を管理しているオブジェクトにカメラの座標とモデルの回転値を同期します。
            m_FootPosSet.position = m_CameraPos.position;
            m_FootPosSet.rotation = m_AvaterModel.rotation;
            //次に、両足目的座標のオブジェクトのY座標を固定させます。
            m_FootLDestination.position = new Vector3(m_FootLDestination.position.x, m_ALLFootYPos.position.y, m_FootLDestination.position.z);
            m_FootRDestination.position = new Vector3(m_FootRDestination.position.x, m_ALLFootYPos.position.y, m_FootRDestination.position.z);
            //最後に両足の座標チェック型同期用関数を呼び、処理終了。
            FootPosCheck(m_FootL, m_FootLDestination, m_MoveDistance);
            FootPosCheck(m_FootR, m_FootRDestination, (m_MoveDistance + 0.1f));
        }
        //両足の座標チェック型同期用関数
        private void FootPosCheck(Transform footpos, Transform footdistancepos, float movedistance) {
            //自身と指定する検知座標の距離を取得
            float tmpDis = Vector3.Distance(footpos.position, footdistancepos.position);
            if (tmpDis > movedistance) {
                //footpos.position = footdistancepos.position;
                footpos.position = Vector3.Lerp(footpos.position, footdistancepos.position, m_Speed * Time.deltaTime);
            }
            //footpos.rotation = footdistancepos.rotation;
            footpos.rotation = Quaternion.Slerp(footpos.rotation, footdistancepos.rotation, m_Speed * Time.deltaTime);
        }
        //両足の即自同期用関数
        public void FootPosSet() {
            m_FootL.position = m_FootLDestination.position;
            m_FootL.rotation = m_FootLDestination.rotation;
            m_FootR.position = m_FootRDestination.position;
            m_FootR.rotation = m_FootRDestination.rotation;
        }
    }
}