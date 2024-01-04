using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : MonoBehaviour
{
    public PolygonCollider2D avalancheCollider;
    public Animator animator;
    public AudioSource audioSource;
    public int avalancheIndex;

    // Start is called before the first frame update
    void Start()
    {
        // find animator and collider
        avalancheCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        if(avalancheIndex == 0)
        {
            StartCoroutine(PlaySound(16f));
        }

        if (avalancheIndex == 1)
        {
            StartCoroutine(PlaySound(45f));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // enabel the collider
    public void EnableCollider()
    {
        avalancheCollider.enabled = true;
    }

    // disable the collider
    public void DisableCollider()
    {
        avalancheCollider.enabled = false;
    }

    private IEnumerator PlaySound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.volume = 0.5f;
        audioSource.Play();
        yield return new WaitForSeconds(13f);
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= (Time.deltaTime / 3f);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 0.5f;
    }
}
