using UnityEngine;

public class ParticleVariables : MonoBehaviour
{
    public ParticleSystem particles;
    public float ParticleSpeed = 100;
    public float ParticleCount = 100;
    // Use this for initialization
    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule ps = particles.main;
        ParticleSystem.EmissionModule ems = particles.emission;

        ps.startSpeed = ParticleSpeed;
        ems.rateOverTime = ParticleCount;





    }

    // Update is called once per frame
    void Update()
    {

    }
}
