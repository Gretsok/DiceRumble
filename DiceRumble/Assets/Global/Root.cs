using MOtter.LevelData;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField]
    private LevelData m_firstLevelData = null;
    // Start is called before the first frame update
    void Start()
    {
        m_firstLevelData.LoadLevel();   
    }
}
