using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    // These three fields need to be defined in the Inspector pane
    public string name;     // The name of this part
    public float health;    // The amount of health this part has
    public string[] protectedBy;    // The other parts that protect this

    [HideInInspector]
    public GameObject go;   // The GameObject of this part
    [HideInInspector]
    public Material mat;    // The Material to show damage
}
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;

    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4;


    // Start is called before the first frame update
    void Start()
    {
        p0 = p1 = pos;

        InitMovement();

        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }
    void InitMovement()
    {
        p0 = p1;

        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtminRad = bndCheck.camHeight - bndCheck.radius;

        timeStart = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }
}
