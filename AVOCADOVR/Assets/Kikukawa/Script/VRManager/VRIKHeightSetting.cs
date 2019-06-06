/*
*   Name:菊川 誠
*   Script:菊川の作成したアバターの高さを合わせることのできるクラス
*   Day:19/06/06 
*/
using UnityEngine;
namespace MKTVRManager {
    public class VRIKHeightSetting : MonoBehaviour {
        [Header("高さを変更するオブジェクト")]
        [SerializeField] Transform m_HeightObj;
        [Header("変化値")]
        [SerializeField] float m_ChangeNum = 0.02f;
        void Update() {
            //もし、上キーを入力したら
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                ChangeHeight(m_ChangeNum);
            }
            //もし、下キーを入力したら
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                ChangeHeight(-m_ChangeNum);
            }
        }
        //高さを変更する事の可能な関数
        public void ChangeHeight(float num) {
            m_HeightObj.position += new Vector3(0,num,0);
        }
    }
}
