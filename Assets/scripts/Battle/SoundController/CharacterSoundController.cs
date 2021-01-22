using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character), typeof(AudioSource))]
public class CharacterSoundController : MonoBehaviour
{

    protected Character owner;
    protected AudioSource audioSource;

    public AudioClip OnTakeDamage;
    public AudioClip OnDeath;
    public AudioClip OnWalk;
    public AudioClip LevelUp;

    public virtual void Start()
    {
        owner = GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();
        owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;
        owner.OnDeathStart += Owner_OnDeathStart;
        owner.OnMoveStart += Owner_OnMoveStart;
        owner.OnMoveEnd += Owner_OnMoveEnd;
        owner.LeveledUp += Owner_LeveledUp;
    }

    private void Owner_LeveledUp(object sender, System.EventArgs e)
    {
        audioSource.PlayOneShot(LevelUp);
    }

    private void Owner_OnMoveEnd(object sender, System.EventArgs e)
    {
        audioSource.loop = false;
    }

    private void Owner_OnMoveStart(object sender, System.EventArgs e)
    {
        audioSource.loop = true;
        audioSource.clip = OnWalk;
        audioSource.Play();
    }



    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void Owner_OnDeathStart(object sender, System.EventArgs e)
    {
        PlaySound(OnDeath);
    }

    private void Owner_OnAfterTakeDamage(object sender, Character.TakeDamageEventArgs e)
    {
        PlaySound(OnTakeDamage);
    }
}
