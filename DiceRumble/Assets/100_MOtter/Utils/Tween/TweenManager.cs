using UnityEngine;

namespace Tween
{
    public class TweenManager : MonoBehaviour
    {
        private static TweenManager s_instance = null;
        public static TweenManager Instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new GameObject("TweenManager").AddComponent<TweenManager>();
                }
                return s_instance;
            }
        }

        private void Awake()
        {
            if(s_instance != null)
            {
                if(s_instance != this)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                s_instance = this;
            }
        }
    }
}