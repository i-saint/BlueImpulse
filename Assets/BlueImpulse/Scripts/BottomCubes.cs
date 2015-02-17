using UnityEngine;
using System.Collections;

public class BottomCubes : CubeRoutine
{
    public AnimationCurve m_curve;

    [System.Serializable]
    public struct IMD
    {
        public Vector3 base_pos;
        public float random;
        public float speed;
        public float down;
    }

    public IMD[] m_imd;


    public override void OnEnable()
    {
        const int w = 31;
        m_instances = new BatchCubeRenderer.InstanceData[w * w];
        m_imd = new IMD[m_instances.Length];
        for (int xi = 0; xi < w; ++xi)
        {
            for (int zi = 0; zi < w; ++zi)
            {
                int i = w * xi + zi;

                Vector3 pos = new Vector3(1.1f * xi - 16.1f, Random.Range(-2.0f, 0.0f) - 0.7f, 1.1f * zi - 16.1f);
                float d = Mathf.Sqrt(pos.x * pos.x + pos.z * pos.z);
                m_imd[i].base_pos = pos;
                m_imd[i].down = -10.0f-(5.0f - d*1.0f);
                m_imd[i].random = Random.Range(-1.0f, 1.0f);
                pos.y += m_imd[i].down + (-d * 0.1f);
                m_instances[i].translation = pos;
                m_instances[i].rotation = Quaternion.identity;
                m_instances[i].scale = 1.0f;
            }
        }
        base.OnEnable();
    }

    public override void Update()
    {
        for (int i = 0; i < m_instances.Length; ++i )
        {
            if (m_imd[i].down < 0.0)
            {
                float u = Mathf.Max(-Time.deltaTime, m_imd[i].down);
                m_imd[i].down -= u;
                m_instances[i].translation.y -= u;
            }
            m_instances[i].translation.y += Mathf.Sin(m_imd[i].random * Mathf.PI + m_instances[i].time*0.4f) * 0.002f;
        }
        base.Update();
    }
}
