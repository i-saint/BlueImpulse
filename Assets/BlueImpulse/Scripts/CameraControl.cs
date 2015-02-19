using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


public class CameraControl : MonoBehaviour
{
    public bool m_rotate_by_time = false;
    public float m_rotate_speed = -10.0f;
    public Transform m_camera;
    public Transform m_look_target;
    Transform m_trans;


    void Awake()
    {
        m_trans = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) { m_rotate_by_time = !m_rotate_by_time; }

        if (m_camera == null) return;

        Vector3 pos = m_camera.transform.position;
        if (m_rotate_by_time)
        {
            m_trans.Rotate(Vector3.up, Time.deltaTime * m_rotate_speed);
        }
        if (Input.GetMouseButton(0))
        {
            float ry = Input.GetAxis("Mouse X") * 3.0f;
            float rxz = Input.GetAxis("Mouse Y") * 0.25f;
            pos = Quaternion.Euler(0.0f, ry, 0) * pos;
            pos.y += rxz;
            m_camera.transform.position = pos;
        }
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel != 0.0f)
            {
                pos += pos.normalized * wheel * 4.0f;
                m_camera.transform.position = pos;
            }
        }
    }

    void LateUpdate()
    {
        m_camera.transform.LookAt(m_look_target.position);
    }
}
