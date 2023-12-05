using UnityEngine;

public class WheelData
{
    public WheelData(WheelType type, Transform wheelTrm, ParticleSystem skidParticle)
    {
        Type = type;
        WheelTrm = wheelTrm;
        SkidParticle = skidParticle;
    }

    public ParticleSystem SkidParticle;
    public WheelType Type;
    public Transform WheelTrm;
}

public enum WheelType
{
    Front = 0, 
    Back = 1
}


