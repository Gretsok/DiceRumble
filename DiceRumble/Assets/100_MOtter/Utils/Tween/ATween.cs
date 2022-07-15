using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Tween
{
    [System.Serializable]
    public abstract class ATween : MonoBehaviour
    {
        #region Callbacks
        [SerializeField]
        private UnityEvent m_onStarted = null;
        [SerializeField]
        private UnityEvent m_onFinish = null;

        public Action<ATween> m_onTweenStarted = null;
        public Action<ATween> m_onTweenFinish = null;
        #endregion

        #region Infos
        private bool m_isPlaying = false;
        public bool IsPlaying => m_isPlaying;
        #endregion

        #region Basic Params
        protected Transform m_target = null;
        [SerializeField]
        private EStartingType m_onStartingType = EStartingType.OnStart;
        [SerializeField]
        protected float m_tweenDuration = 1f;
        [SerializeField]
        protected ELoopType m_loopType = ELoopType.Once;
        [SerializeField]
        protected AnimationCurve m_tweenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public EStartingType OnStartingType => m_onStartingType;
        public float TweenDuration => m_tweenDuration;
        public ELoopType LoopType => m_loopType;
        #endregion

        private ATween[] m_attachedTweens = null;
        private List<Coroutine> m_coroutines = new List<Coroutine>();

        private void Awake()
        {
            Init();
            if (m_onStartingType == EStartingType.OnAwake)
            {
                StartTween();
            }
        }

        protected virtual void Start()
        {
            Init();
            if (m_onStartingType == EStartingType.OnStart)
            {
                StartTween();
            }
        }

        private void OnEnable()
        {
            Init();
            if (m_onStartingType == EStartingType.OnEnabled)
            {
                StartTween();
            }
        }

        private bool m_isInit = false;
        private void Init()
        {
            m_attachedTweens = gameObject.GetComponents<ATween>();
            m_target = transform;
            m_isInit = true;
        }

        public void StartTween(bool forward = true)
        {
            if(!m_isPlaying)
            {
                if (!m_isInit)
                {
                    Init();
                }
                m_isPlaying = true;
                m_onStarted?.Invoke();
                m_onTweenStarted?.Invoke(this);
                switch(m_loopType)
                {
                    case ELoopType.Once:
                        m_coroutines.Add(TweenManager.Instance.StartCoroutine(OnceRoutine()));
                        break;
                    case ELoopType.Loop:
                        m_coroutines.Add(TweenManager.Instance.StartCoroutine(LoopRoutine()));
                        break;
                    case ELoopType.BackAndForth:
                        m_coroutines.Add(TweenManager.Instance.StartCoroutine(BackAndForthRoutine()));
                        break;
                    case ELoopType.PingPong:
                        m_coroutines.Add(TweenManager.Instance.StartCoroutine(PingPongRoutine()));
                        break;
                }
            }
        }

        public virtual void StartAllAttachedTweens()
        {
            for (int i = 0; i < m_attachedTweens.Length; ++i)
            {
                m_attachedTweens[i].StartTween();
            }
        }

        protected virtual void Stop()
        {
            for(int i = m_coroutines.Count - 1; i >= 0; --i)
            {
                if(m_coroutines[i] != null)
                    TweenManager.Instance.StopCoroutine(m_coroutines[i]);
            }
            m_coroutines.Clear();
            m_isPlaying = false;
            m_onFinish?.Invoke();
            m_onTweenFinish?.Invoke(this);
        }

        public virtual void StopAllAttachedTweens()
        {
            if(m_attachedTweens == null)
            {
                return;
            }
            for(int i = 0; i < m_attachedTweens.Length; ++i)
            {
                m_attachedTweens[i].Stop();
            }
        }

        private IEnumerator OnceRoutine()
        {
            var routine = TweenManager.Instance.StartCoroutine(ForwardRoutine());
            m_coroutines.Add(routine);
            yield return routine;
            Stop();
        }
        
        private IEnumerator LoopRoutine()
        {
            while(m_isPlaying)
            {
                var routine = TweenManager.Instance.StartCoroutine(ForwardRoutine());
                m_coroutines.Add(routine);
                yield return routine;
            }
        }

        private IEnumerator BackAndForthRoutine()
        {
            var routine = TweenManager.Instance.StartCoroutine(ForwardRoutine());
            m_coroutines.Add(routine);
            yield return routine;
            routine = TweenManager.Instance.StartCoroutine(BackwardRoutine());
            m_coroutines.Add(routine);
            yield return routine;
            Stop();
        }

        private IEnumerator PingPongRoutine()
        {
            while (m_isPlaying)
            {
                var routine = TweenManager.Instance.StartCoroutine(ForwardRoutine());
                m_coroutines.Add(routine);
                yield return routine;
                routine = TweenManager.Instance.StartCoroutine(BackwardRoutine());
                m_coroutines.Add(routine);
                yield return routine;
            }
        }

        private IEnumerator ForwardRoutine()
        {
            float startingTime = Time.time;
            while (Time.time - startingTime < m_tweenDuration)
            {
                ManageTween(Mathf.Clamp01(m_tweenCurve.Evaluate(Mathf.Clamp01((Time.time - startingTime) / m_tweenDuration))));
                yield return null;
            }
        }

        private IEnumerator BackwardRoutine()
        {
            float startingTime = Time.time;
            while (Time.time - startingTime < m_tweenDuration)
            {
                ManageTween(1 - Mathf.Clamp01(m_tweenCurve.Evaluate(Mathf.Clamp01((Time.time - startingTime) / m_tweenDuration))));
                yield return null;
            }
        }


        protected virtual void SetStartingValues()
        {

        }

        protected virtual void SetFinalValues()
        {

        }

        public virtual void ResetValues()
        {
            SetStartingValues();
        }

        protected virtual void ManageTween(float interpolationValue)
        {

        }

        public void RegisterNewRoutine(IEnumerator a_routine)
        {

        }

        private void OnDestroy()
        {
            Stop();
        }

    }

    public enum ELoopType
    {
        Once,
        Loop,
        BackAndForth,
        PingPong
    }

    public enum EStartingType
    {
        OnAwake,
        OnStart,
        OnEnabled,
        ByCode
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ATween), true)]
    public class ATweenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Start tween !"))
            {
                (target as ATween).StartTween();
            }

            if (GUILayout.Button("Play all attached tweens !"))
            {
                (target as ATween).StartAllAttachedTweens();
            }

            if (GUILayout.Button("Stop all attached tweens !"))
            {
                (target as ATween).StopAllAttachedTweens();
            }

            if(GUILayout.Button("Reset !"))
            {
                (target as ATween).ResetValues();
            }
        }
    }
#endif
}