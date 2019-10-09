using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;


    private BoundsCheck bndCheck;

    //This is a property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck != null && !bndCheck.offDown)
        {
            //Check to make sure it's gone off the bottom of the screen
            if (pos.y < bndCheck.camHeight - bndCheck.radius)
            {
                //We're off the bottom, to destroy this GameObject
                Destroy(gameObject);
            }
        }
    }
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.tag == "Projectile Hero")
        {
            Destroy(otherGO);
            Destroy(gameObject);
        }
        else
        {
            print("Enemy Hit by non-Projectile Hero: " + otherGO.name);
        }
    }
}
