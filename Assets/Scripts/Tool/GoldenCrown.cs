using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenCrown : Tool
{
    // name/type
    public string _name = "GoldenCrown";
    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    // local position for equip
    public Vector3 _localPosition = new Vector3(0.006f, 0.264f, 0);
    public override Vector3 LocalPosition
    {
        get { return _localPosition; }
        set { _localPosition = value; }
    }
    // throwing tool
    public bool _isThrowing = false;
    public override bool IsThrowing
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }
    // parent player pick up tool
    public bool _isPicked = false;
    public override bool IsPicked
    {
        get { return _isPicked; }
        set { _isPicked = value; }
    }
    // public Vector2 _direcion;
    public Vector2 _direcion;
    public override Vector2 Direction
    {
        get { return _direcion; }
        set { _direcion = value; }
    }

    //sound effect
    public AudioSource audioSource;
    public AudioClip invincibleAudioClip;
    private bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPicked)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                StartCoroutine(PlaySound());
            }
        }
        if(!_isPicked)
        {
            audioSource.Stop();
            audioSource.volume = 0.6f;
        }
    }

    private IEnumerator PlaySound()
    {
        audioSource.volume = 0.6f;
        audioSource.Play();
        yield return new WaitForSeconds(4.5f);
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= (Time.deltaTime / 1.5f);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0.6f;
    }

}
