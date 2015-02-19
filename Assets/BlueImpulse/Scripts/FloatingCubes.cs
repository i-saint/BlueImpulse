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
        public float speed;
    }
    IMD[] m_imd;


    public override void OnEnable()
    {
        const int num = 64;
        m_instances = new BatchCubeRenderer.InstanceData[num];
        m_imd = new IMD[num];
        for (int i = 0; i < num; ++i )
        {

        }

        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();
    }
}
