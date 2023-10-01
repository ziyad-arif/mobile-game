using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip shoot;
    public AudioClip enemyDeath;
    public AudioClip collectXP;
    public AudioClip cannotShoot;
    [HideInInspector] public List<AudioSource> audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>().ToList();
    }
    public void Shoot()
    {
        audioSources[1].PlayOneShot(shoot);
    }
    public void CannotShoot()
    {
        audioSources[2].PlayOneShot(cannotShoot);
    }
    public void EnemyDeath()
    {
        audioSources[3].PlayOneShot(enemyDeath);
    }
    public void CollectXP()
    {
        audioSources[4].PlayOneShot(collectXP);
    }
}
