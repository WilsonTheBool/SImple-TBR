using UnityEngine;
using System.Collections;

public class RangedCharacterSoundController : CharacterSoundController
{

    public AudioClip attackRanged;
    public override void Start()
    {
        base.Start();
        (owner as RangedCharacter).AfterRangeAttack += RangedCharacterSoundController_BeforeRangeAttack;
    }

    private void RangedCharacterSoundController_BeforeRangeAttack(object sender, AttackEventArgs e)
    {
        audioSource.PlayOneShot(attackRanged);
    }
}
