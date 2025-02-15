﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f; // # seconds to show damage
    public float powerUpDropChance = 1f;    // Chance to drop a power-up
    
    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;  // All the Materials of this & its children
    public bool showingDamage = false;
    public float damageDoneTime;  // Time to stop showing damage
    public bool notifiedOfDestruction = false;

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>(); 
    }

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck != null && bndCheck.offDown)
        {
            
            
               //We're off the bottom, to destroy this GameObject
                Destroy(gameObject);
            
        }
    }
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // If this Enemy is off screen, don't damage it.
                if(!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                // Hurt this Enemy
                ShowDamage();
                //Get the damage amount from the Main WEAP_DICT.
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    // Tell the Main Singleton that this ship was destroyed
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }
    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
