using System.Collections.Generic;
using UnityEngine;

namespace DR.Gameplay.Level.Grid
{
    public class Grid : MonoBehaviour
    {
        [System.Serializable]
        private class TileColumn
        {
            public List<Tile> Tiles;

            public TileColumn()
            {
                Tiles = new List<Tile>();
            }
        }

        [SerializeField]
        private Vector2Int m_gridSize = new Vector2Int(5, 8);
        [SerializeField]
        private Vector2 m_tileSize = new Vector2(2f, 2f);

        [SerializeField]
        private List<Tile> m_tilesPrefabs = null;

        [SerializeField, HideInInspector]
        private List<TileColumn> m_tiles = new List<TileColumn>();

        public void GenerateLevel()
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                for (int x = 0; x < m_tiles.Count; ++x)
                {
                    for (int y = 0; y < m_tiles[x].Tiles.Count; ++y)
                    {
                        if(m_tiles[x].Tiles[y] != null
                            && m_tiles[x].Tiles[y].gameObject != null)
                            DestroyImmediate(m_tiles[x].Tiles[y].gameObject);
                    }
                }
                m_tiles.Clear();


                for (int x = 0; x < m_gridSize.x; ++x)
                {
                    m_tiles.Add(new TileColumn());
                    for (int y = 0; y < m_gridSize.y; ++y)
                    {
                        Tile newTile = UnityEditor.PrefabUtility.InstantiatePrefab(m_tilesPrefabs[Random.Range(0, m_tilesPrefabs.Count)], transform) as Tile;
                        m_tiles[x].Tiles.Add(newTile);
                        newTile.transform.localPosition = x * m_tileSize.x * Vector3.right + y * m_tileSize.y * Vector3.back;
                    }
                }

                UnityEditor.EditorUtility.SetDirty(this);
            }
            else
#endif
            {
                Debug.LogError("You can only generate the level in the editor in edit mode!");
            }
        }

        public Tile TryToGetTile(Vector2Int a_gamePosition)
        {
            if (a_gamePosition.x < 0 || a_gamePosition.y < 0) return null;
            if (a_gamePosition.x >= m_gridSize.x || a_gamePosition.y >= m_gridSize.y) return null;

            return m_tiles[a_gamePosition.x].Tiles[a_gamePosition.y];
        }

    }
}