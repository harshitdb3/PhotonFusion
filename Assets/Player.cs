using Fusion;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _cc;
    public bool isBeingAttacked = false;
    public bool shouldAttack = true;
    Player engagedPlayer = null;
    [SerializeField]
    Animator anim;
    [SerializeField]
    NetworkMecanimAnimator Netanim;
    Hitbox hitbox;

    private LayerMask layer;

    public bool isSpawned;

    [HideInInspector]
    [Networked(OnChanged = nameof(OnHealthChanged))]
    public float Health { get; private set; }

    private static void OnHealthChanged(Changed<Player> playerInfo)
    {

    }

    private Collider[] _hits;

    private void Awake()
    {
    
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void Spawned()
    {
 
        base.Spawned();     
        Health = 100;
        isSpawned = true;
    }

    public override void FixedUpdateNetwork()
    {
  
        if (shouldAttack == false) return;
        if (GetInput(out NetworkInputData data))
        {
            if(data.hasAttacked1 && shouldAttack)
            {
                Netanim.SetTrigger("Attack01",true);



            }
            if(data.hasAttacked2 && shouldAttack)
            {
                Netanim.SetTrigger("Attack02",true);

            }

            if(Health == 0)
            {
                Netanim.SetTrigger("Died", true);
                shouldAttack = false;
            }
        }
  
    //var count = Runner.GetPhysicsScene().OverlapSphere(transform.position, 1, _hits,
    //layer, QueryTriggerInteraction.UseGlobal);

    //    if (count > 0)
    //    {
    //        Debug.Log("hit");
          
    //    }

        
    }

    private void OnTriggerEnter(Collider other)
    {
      //  if (Object.HasInputAuthority == false) return;
        if (other.transform.GetComponentInParent<Player>() != this &&  other.gameObject.tag == "Sword")
        {

            Debug.Log("Hit");
            Health -= 10;
        }
        // Take Damage

        // if Helth == 0 , Destroy
    }
}