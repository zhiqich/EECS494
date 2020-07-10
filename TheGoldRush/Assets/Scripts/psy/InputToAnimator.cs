using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToAnimator : MonoBehaviour
{
    Animator animator;
    PlayerBrain pb;
    PlayerBrain.Direction direction;
    bool moving = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        pb = GetComponent<PlayerBrain>();
    }

    public bool getPlayer = false;

    // Update is called once per frame
    void Update()
    {
        if (getPlayer)
        {
            return;
        }
        // update parameters
        direction = pb.GetDirection();
        moving = pb.GetMoving();

        bool shooting = false;

        // send parameters to animator
        animator.SetBool("Moving", moving);
        
        if (pb.character == Character.PickaxeMan){
            animator.SetBool("Attacking", GetComponent<PlayerWeapons>().pickaxe.GetComponent<Pickaxe>().brandishing);
        }
        if (pb.character == Character.Shooter){
            if (GetComponent<ShooterWeapon>().attacking){
                shooting = true;
                // animator.SetInteger("Dir", GetComponent<ShooterWeapon>().shootingDir);
                StartCoroutine(shooterAttackCool());
                GetComponent<ShooterWeapon>().attacking = false;
            }
            if (animator.GetBool("Attacking")){
                animator.SetBool("Attacking", false);
            }
        }
        animator.SetInteger("Dir", (int)direction);
        // if (! shooting){
        //     animator.SetInteger("Dir", (int)direction);
        // }
    }


    IEnumerator shooterAttackCool()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("Attacking", true);
    }
}
