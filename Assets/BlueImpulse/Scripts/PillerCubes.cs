using UnityEngine;
using System.Collections;

public class PillerCubes : CubeRoutine
{
    public struct IMD
    {
        public float speed;
    }
    IMD[] m_imd;

    public override void OnEnable()
    {
        const int cubes_par_piller = 256;
        m_instances = new BatchCubeRenderer.InstanceData[cubes_par_piller * 3];
        m_imd = new IMD[cubes_par_piller * 3];

        const float r = 8.0f;
        Vector3[] base_pos = new Vector3[3] {
            new Vector3(Mathf.Cos(  0.0f*Mathf.Deg2Rad), 0.0f, Mathf.Sin(  0.0f*Mathf.Deg2Rad))*r,
            new Vector3(Mathf.Cos(120.0f*Mathf.Deg2Rad), 0.0f, Mathf.Sin(120.0f*Mathf.Deg2Rad))*r,
            new Vector3(Mathf.Cos(240.0f*Mathf.Deg2Rad), 0.0f, Mathf.Sin(240.0f*Mathf.Deg2Rad))*r,
        };

        for (int pi = 0; pi < 3; ++pi)
        {
            for (int ci = 0; ci < cubes_par_piller; ++ci)
            {
                int i = cubes_par_piller * pi + ci;
                const float rxz = 0.75f;
                const float ry = 5.0f;
                m_instances[i].translation = base_pos[pi] + new Vector3(Random.Range(-rxz, rxz), Random.Range(-ry, ry), Random.Range(-rxz, rxz));
                m_instances[i].scale = Random.Range(0.4f, 0.9f);
                m_imd[i].speed = Random.Range(0.5f, 1.0f);
            }
        }
        base.OnEnable();
    }

    public override void Update()
    {
        for (int i = 0; i < m_instances.Length; ++i )
        {
            float y = m_instances[i].translation.y;
            y += Time.deltaTime * m_imd[i].speed;
            if (y > 5.0f)
            {
                y -= 10.0f;
            }
            m_instances[i].translation.y = y;
        }
        base.Update();
    }
}
