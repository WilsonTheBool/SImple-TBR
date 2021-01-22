using UnityEngine;
using System.Collections;
using System;

public class CharacterAnimationController : MonoBehaviour
{
    Character owner;

    SpriteRenderer spriteRenderer;

    Animator animator;

    Vector3 lastPos;


    public const int animatorlayerIndex = 0;

    public enum Side
    {
        left, right,
    }

    public Side side
    {
        get
        {
            if (spriteRenderer.flipX)
            {
                return Side.left;
            }
            else
            {
                return Side.right;
            }
        }
        set
        {
            if (value == Side.right)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void Start()
    {
        owner = GetComponent<Character>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        owner.OnMoveStart += StartAnim_Run;
        owner.OnMoveEnd += EndAnim_Run;
        owner.OnDeathStart += StartDeathAnim;

       //owner.OnBeforeTakeDamage += StartAnim_Block;
        owner.OnBeforeTakeDamage += LookAtTakeDamage;

        if (owner is MeleeCharacter)
        {
            //(owner as MeleeCharacter).OnBeforeAttack += StartAnim_Attack;
            (owner as MeleeCharacter).OnBeforeAttack += StartNotAnim_Attack;
        }
        if (owner is IRangeAttacker)
        {
            (owner as IRangeAttacker).BeforeRangeAttack += StartAnim_AttackRange;
        }

        lastPos = transform.position;
    }

    void LookAtTakeDamage(object obj, Character.TakeDamageEventArgs e)
    {
        if(e.attackData.owner != null)
        {
            owner.LookAt(e.attackData.owner.TilePosition);
        }
        
    }

    public void FixedUpdate()
    {
        ManageMoovingSide();
    }

    private bool isMoving
    {
        get
        {
            return animator.GetBool("Run");
        }
        
    }

    public void ManageMoovingSide()
    {
        if (isMoving)
        {
            if (lastPos.x != transform.position.x)
                if (lastPos.x < transform.position.x)
                {
                    if (side != Side.right)
                    {
                        side = Side.right;
                    }

                }
                else
                {
                    side = Side.left;
                }
            lastPos = transform.position;
        }
        
    }

    public void SwitchSide(Side sideToSwitch)
    {
        if(side != sideToSwitch)
        {
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;

            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void StartAnim_Attack(object owner, EventArgs e)
    {
        animator.SetBool("Attack", true);
    }

    private void   StartNotAnim_Attack(object owner, AttackEventArgs e)
    {

        (owner as MeleeCharacter).StartNotAnim_Attack((owner as BattleObject).TilePosition, (e.target as BattleObject).TilePosition);

        SpawnSlashAnimObj((e.target as BattleObject).TilePosition);
    }

    [SerializeField]
    private GameObject slashAnimPrefab;
    private void SpawnSlashAnimObj(Vector3Int taretPos)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, TileMath.GetZRotation(owner.TilePosition, taretPos));
        Vector3 newPos = owner.transform.position;
        newPos.y += 0.2f;

        Destroy(Instantiate(slashAnimPrefab, newPos, rotation), 0.2f);
    }

    public void StartAnim_AttackRange(object owner, EventArgs e)
    {
        animator.SetBool("Fire", true);

        StartCoroutine(FireAnimStarted());
    }

    private IEnumerator FireAnimStarted()
    {
        yield return new WaitForEndOfFrame();

        float waitTime = animator.GetCurrentAnimatorStateInfo(animatorlayerIndex).length;

        yield return new WaitForSeconds(waitTime);

        FireAnimationEnded?.Invoke();
    }

    public Action FireAnimationEnded;


    public void StartAnim_Block(object owner, EventArgs e)
    {
        animator.SetBool("Block", true);
    }

    public void StartDeathAnim(object owner, EventArgs e)
    {
        animator.SetBool("Death", true);

        StartCoroutine(OnDeath());
    }

    public float DeathDelay;

    private IEnumerator OnDeath()
    {
        while (animator.GetBool("Death"))
        {
            yield return new WaitForFixedUpdate();
        }

        owner.DeathAnimEnded();

        spriteRenderer.sortingOrder = -100;
        //gameObject.transform.Translate(UnityEngine.Random.Range(-0.2f,0.2f), UnityEngine.Random.Range(0, 0.1f), 0);
    }

    public void StartAnim_Run(object owner, EventArgs e)
    {
        animator.SetBool("Run", true);
    }

    public void EndAnim_Run(object owner, EventArgs e)
    {
        animator.SetBool("Run", false);
    }
}
