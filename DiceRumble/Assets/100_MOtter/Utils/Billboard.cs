using System.Collections;
using UnityEngine;

namespace MOtter.Utils
{
    public class Billboard : MonoBehaviour
    {
        private enum EBillboardType
        {
            WatchingCamera,
            CopyCameraOrientation
        }
        [SerializeField]
        private EBillboardType m_billboardType = EBillboardType.CopyCameraOrientation;
        [SerializeField]
        private bool m_reverseBillboard = false;
        [SerializeField]
        private Camera m_cameraToUse = null;
        private Transform m_cameraTransform = null;

        private void Start()
        {
            SetCamera(m_cameraToUse);
        }

        public void SetCamera(Camera camera)
        {
            if(camera != null)
            {
                SetCamera(camera.transform);
            }
        }

        public void SetCamera(Transform cameraTransform)
        {
            if (cameraTransform != null)
                m_cameraTransform = cameraTransform;
            else
                StartCoroutine(WaitForMainCamera());
        }

        private IEnumerator WaitForMainCamera()
        {
            while (Camera.main == null)
                yield return null;
            m_cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if(m_cameraTransform != null)
            {
                if (m_billboardType == EBillboardType.CopyCameraOrientation)
                {
                    if (m_reverseBillboard)
                    {
                        transform.forward = m_cameraTransform.forward;
                    }
                    else
                    {
                        transform.forward = -m_cameraTransform.forward;
                    }

                }
                else if (m_billboardType == EBillboardType.WatchingCamera)
                {
                    if (m_reverseBillboard)
                    {
                        transform.LookAt(-m_cameraTransform.position);
                    }
                    else
                    {
                        transform.LookAt(m_cameraTransform.position);
                    }

                }
            }
        }
    }
}