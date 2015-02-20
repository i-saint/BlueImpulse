using UnityEngine;
using System.Collections;

public class PillerCubes : CubeRoutine
{
    [System.Serializable]
    public struct IMD
    {
        public Vector3 initial_pos;
        public float speed;
    }
    public float m_time;
    float m_prev_time;
    public IMD[] m_imd;

    public override void Generate()
    {
        const int cubes_par_piller = 200;
        const int num_pillers = 8;
        m_instances = new BatchCubeRenderer.InstanceData[cubes_par_piller * num_pillers];
        m_imd = new IMD[cubes_par_piller * num_pillers];

        const float r1 = 9.0f;
        const float r2 = 16.0f;
        const float shift = 0.0f;
        Vector3[] base_pos = new Vector3[num_pillers] {
            new Vector3(Mathf.Cos((  0.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((  0.0f+shift)*Mathf.Deg2Rad))*r1,
            new Vector3(Mathf.Cos(( 90.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin(( 90.0f+shift)*Mathf.Deg2Rad))*r1,
            new Vector3(Mathf.Cos((180.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((180.0f+shift)*Mathf.Deg2Rad))*r1,
            new Vector3(Mathf.Cos((270.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((270.0f+shift)*Mathf.Deg2Rad))*r1,

            new Vector3(Mathf.Cos(( 45.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin(( 45.0f+shift)*Mathf.Deg2Rad))*r2,
            new Vector3(Mathf.Cos((135.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((135.0f+shift)*Mathf.Deg2Rad))*r2,
            new Vector3(Mathf.Cos((225.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((225.0f+shift)*Mathf.Deg2Rad))*r2,
            new Vector3(Mathf.Cos((315.0f+shift)*Mathf.Deg2Rad), 0.0f, Mathf.Sin((315.0f+shift)*Mathf.Deg2Rad))*r2,
    };

        for (int pi = 0; pi < num_pillers; ++pi)
        {
            for (int ci = 0; ci < cubes_par_piller; ++ci)
            {
                int i = cubes_par_piller * pi + ci;
                const float rxz = 0.75f;
                const float ry = 15.0f;
                Vector3 pos = base_pos[pi] + new Vector3(Random.Range(-rxz, rxz), Random.Range(-ry, ry), Random.Range(-rxz, rxz));
                m_imd[i].initial_pos = pos;
                m_imd[i].speed = Random.Range(0.25f, 1.0f);
                m_instances[i].translation = pos;
                m_instances[i].scale = Random.Range(0.5f, 1.1f);
            }
        }
    }

    public override void Update()
    {
        float dt = m_time - m_prev_time;
        m_prev_time = m_time;
        for (int i = 0; i < m_instances.Length; ++i )
        {
            float y = m_instances[i].translation.y;
            y += dt * m_imd[i].speed;
            if (y > 15.0f)
            {
                y -= 30.0f;
            }
            m_instances[i].translation.y = y;
        }
        base.Update();
    }
}
