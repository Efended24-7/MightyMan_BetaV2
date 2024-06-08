using UnityEngine;
using System.Collections;


public class FistDamage : MonoBehaviour
{
    public int damageAmount;
    private bool isColliding = false;
    //float curTime = 0;
    //float nextDamage = 3;
    public float cooldown;
    float lastPunch;
   
   public bool IsFistColliding()
    {
        return isColliding;
    }

    void OnTriggerEnter(Collider eneCol)
    {
        // Check if the collider belongs to an enemy NPC
        EnemyAi enemy = eneCol.GetComponent<EnemyAi>(); //monsterHealth
       
        if ( (enemy != null) && (Time.time - lastPunch < cooldown) ){
            // Deal damage to the enemy NPC
            enemy.TakeDamage(damageAmount);
            Debug.Log("Enemy has taken damage: " + enemy);
            isColliding = true;
        }

        lastPunch = Time.time;
        /**
        if (curTime <= 0) {
            Debug.Log ("Current time is less than or equal to 0");
 


            curTime = nextDamage;
        } else {
            Debug.Log("Time reset by 3 seconds");
            curTime -= Time.deltaTime;
        }*/
    }


    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit Trigger.");
        isColliding = false;

    }


}