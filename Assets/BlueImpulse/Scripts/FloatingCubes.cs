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
            m_imd[i].rot_speed1 = Random.Range(0.5f, 1.5f);
            m_imd[i].rot_speed2 = Random.Range(0.5f, 1.5f);
            m_instances[i].scale = 0.8f + R(0.3f);
            //m_instances[i].translation = new Vector3(R());
        }

        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();
    }
}
