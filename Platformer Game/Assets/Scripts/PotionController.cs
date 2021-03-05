using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    public enum PotionType
    {
        Speed,
        Jump,
        Invulnerability
    }

    public PotionType potionType;
    public int potionModAmount = 0;

    //private float floatingTimer = 0f;
    //private float floatingMax = 1f;
    //private float floatingDir = 0.01f;

    /*private void FixedUpdate()
    {
        if(floatingTimer < floatingMax)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + floatingDir);
            floatingTimer += Time.fixedDeltaTime;
        }
        else
        {
            floatingDir += -1;
            floatingTimer = 0f;
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            if(potionType == PotionType.Jump)
            {
                collision.gameObject.GetComponent<PlayerMovement>().hasJumpPotion = true;
            }
            else if(potionType == PotionType.Speed)
            {
                collision.gameObject.GetComponent<PlayerMovement>().hasSpeedPotion = true;
            }
            else if(potionType == PotionType.Invulnerability)
            {
                //collision.gameObject.GetComponent<PlayerMovement>().hasInvulPotion = true;
            }

            collision.gameObject.GetComponent<PlayerMovement>().potionModAmount = potionModAmount;

            Destroy(this.gameObject);
        }
    }

   
}
