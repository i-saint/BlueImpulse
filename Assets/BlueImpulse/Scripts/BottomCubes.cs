using UnityEngine;
using System.Collections;

public class BottomCubes : CubeRoutine
{
    public AnimationCurve m_curve;

    public struct IMD
    {
        public Vector3 base_pos;
        public float random;
    }

    public override void OnEnable()
    {
        m_instances = new BatchCubeRenderer.InstanceData[15*15];
        for (int xi = 0; xi < 15; ++xi)
        {
            for (int zi = 0; zi < 15; ++zi)
            {
                int i = 15*xi+zi;
                m_instances[i].translation = new Vector3(1.1f * xi - 7.7f, Random.Range(-2.0f, 0.0f) - 0.7f, 1.1f * zi - 7.7f);
                m_instances[i].rotation = Quaternion.identity;
                m_instances[i].scale = 1.0f;
            }
        }
        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();
    }
}
