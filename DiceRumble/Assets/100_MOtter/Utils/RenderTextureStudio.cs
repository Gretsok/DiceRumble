using UnityEngine;

public class RenderTextureStudio : MonoBehaviour
{
    [SerializeField]
    private Transform m_spawnPoint = null;
    [SerializeField]
    private Camera m_camerea = null;

    [SerializeField]
    private float m_rotationSpeed = 90f;
    private GameObject m_objectSpawned = null;

    public void Inflate(GameObject objectToShoot, RenderTexture renderTexture)
    {
        m_camerea.targetTexture = renderTexture;
        m_objectSpawned = Instantiate(objectToShoot, m_spawnPoint.position, Quaternion.identity, m_spawnPoint);
    }

    public void DeleteObjectSpawned()
    {
        if(m_objectSpawned != null)
            Destroy(m_objectSpawned);
        m_objectSpawned = null;
    }

    private void Update()
    {
        m_spawnPoint.Rotate(Vector3.up, m_rotationSpeed * Time.deltaTime);
    }
}
