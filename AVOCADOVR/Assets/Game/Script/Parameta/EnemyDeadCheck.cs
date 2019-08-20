using UnityEngine;

public class EnemyDeadCheck : MonoBehaviour {
    [Header("エフェクトがあれば出す")]
    [SerializeField] GameObject m_Effect;
	void Update () {
        if (GetComponent<Parameta>().GetDeadFlag()) {
            if (m_Effect) {
                GameObject Effect = (GameObject)Instantiate(m_Effect, transform.position, transform.rotation);
            }
            Destroy(gameObject);
            DestroyImmediate(gameObject);
        }
    }
}
