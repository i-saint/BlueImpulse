using UnityEngine;
using System.Collections;

public class FloatingCubes : CubeRoutine
{
    [System.Serializable]
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
    public float m_rotation;
    float m_prev_time;
    public IMD[] m_imd;

    float R(float r = 1.0f)
    {
        return Random.Range(-r, r);
    }

    Vector3 RV()
    {
        return new Vector3(R(), R(), R()).normalized;
    }


    public override void Generate()
    {
        const int c1 = 32;
        const int c2 = 256;
        m_instances = new BatchCubeRenderer.InstanceData[c1 + c2];
        m_imd = new IMD[c1 + c2];
        for (int i = 0; i < c1; ++i)
        {
            m_imd[i].axis1 = RV();
            m_imd[i].axis2 = RV();
            m_imd[i].rot_speed1 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_imd[i].rot_speed2 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_imd[i].speed = R(10.0f);
            m_instances[i].scale = 0.5f + R(0.2f);
            m_instances[i].translation = RV() * Random.Range(2.5f, 4.0f);
            m_instances[i].translation.y += 2.0f;
        }
        for (int i = c1; i < c1+c2; ++i)
        {
            m_imd[i].axis1 = RV();
            m_imd[i].axis2 = RV();
            m_imd[i].rot_speed1 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_imd[i].rot_speed2 = Random.Range(0.25f, 1.5f) * 20.0f;
            m_imd[i].speed = R(2.5f);
            m_instances[i].scale = 1.0f + R(0.5f);
            m_instances[i].translation = RV() * (17.0f + R(5.0f));
            m_instances[i].translation.y += 5.0f;
        }
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
            m_instances[i].translation = Quaternion.AngleAxis(m_rotation * dt * m_imd[i].speed, Vector3.up) * m_instances[i].translation;
        }
        base.Update();
    }
}
