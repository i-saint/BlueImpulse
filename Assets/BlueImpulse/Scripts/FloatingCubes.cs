using UnityEngine;
using System.Collections;

public class FloatingCubes : CubeRoutine
{
    public struct IMD
    {
        public Vector3 axis1;
        public Vector3 axis2;
        public float rot_speed1;
        public float rot_speed2;
        public float rot1;
        public float rot2;
        public float speed;
    }
    public float m_time;
    float m_prev_time;
    IMD[] m_imd;

    float R(float r = 1.0f)
    {
        return Random.Range(-r, r);
    }


    public override void OnEnable()
    {
        const int num = 64;
        m_instances = new BatchCubeRenderer.InstanceData[num];
        m_imd = new IMD[num];
        for (int i = 0; i < num; ++i )
        {
            m_imd[i].axis1 = new Vector3(R(), R(), R()).normalized;
            m_imd[i].axis2 = new Vector3(R(), R(), R()).normalized;
            m_imd[i].rot_speed1 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_imd[i].rot_speed2 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_instances[i].scale = 0.5f + R(0.2f);
            m_instances[i].translation = new Vector3(R(3.0f), R(3.0f)+3.0f, R(3.0f));
        }

        base.OnEnable();
    }

    public override void Update()
    {
        float dt = m_time - m_prev_time;
        m_prev_time = m_time;
        for (int i = 0; i < m_instances.Length; ++i)
        {
            m_imd[i].rot1 += m_imd[i].rot_speed1*dt;
            m_imd[i].rot2 += m_imd[i].rot_speed2*dt;
            Quaternion r = Quaternion.AngleAxis(m_imd[i].rot1, m_imd[i].axis1) * Quaternion.AngleAxis(m_imd[i].rot2, m_imd[i].axis2);
            m_instances[i].rotation = r;
        }
        base.Update();
    }
}
