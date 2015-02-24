using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DSEffectBase : MonoBehaviour
{
    DSRenderer m_dsr;
    Camera m_cam;

    protected void ResetDSRenderer()
    {
        Transform parent = GetComponent<Transform>().parent;
        if (m_dsr == null) m_dsr = GetComponent<DSRenderer>();
        if (m_dsr == null) m_dsr = parent.GetComponent<DSRenderer>();
        if (m_cam == null) m_cam = GetComponent<Camera>();
        if (m_cam == null) m_cam = parent.GetComponent<Camera>();
    }

    public DSRenderer GetDSRenderer()
    {
        return m_dsr;
    }

    public Camera GetCamera()
    {
        return m_cam;
    }
}



