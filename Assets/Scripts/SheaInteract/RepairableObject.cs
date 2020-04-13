﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour, IInteractable, IDamageable<int> {

    public int health = 2;

    public int healthMax = 2;

    public int repairAmount = 1;

    [SerializeField] MeshRenderer mesh;
    [SerializeField] MeshFilter filter;
    [SerializeField] Mesh[] meshes;
    int currentMesh;
    //public GameObject steamParticlePrefab;
    public GameObject repairEffect;
    public ParticleSystem steamEffect;
    public ParticleSystem steamEffect2;
    public AlertUI alertUI;

    public bool takeDamageDebug = false;

    AudioSource pipeSound;

    private void Start()
    {
        pipeSound = GetComponent<AudioSource>();

        if(filter == null) { filter = GetComponent<MeshFilter>(); }
        if(mesh == null) { mesh = GetComponent<MeshRenderer>(); }
        
        if(Old_GameplayEvents.instance != null)
        {
            Old_GameplayEvents.instance.shipMaxHealth += healthMax;
            Old_GameplayEvents.instance.shipCurrenHealth += health;
        }

        
        if(alertUI != null)
        {
            alertUI.problemMax += healthMax;
            alertUI.problemCurrent += healthMax;
        }

        
        //StartCoroutine("takeDamage");
    }
    public void InteractWith()
    {
        //Todo: Set up a mechanic that take in the currently equiped tool. 
        if (health < healthMax)
        {
            repairObject(repairAmount);
            mesh.material.color -= Color.red;
            GameObject nutsAndBolts = Instantiate(repairEffect, transform.position + new Vector3(0,0.1f),Quaternion.identity);
            Destroy(nutsAndBolts.gameObject, 1);
             
           // AudioEventManager.instance.PlaySound("clang", .7f, Random.Range(.9f,1f), 0);    //play clang audio
           //ShipHealth.instance.shipCurrenHealth += repairAmount;
           // Debug.Log("Health Points : " + health);

            if(steamEffect.isPlaying)
            {
                steamEffect.Stop();
                steamEffect2.Stop();
            }

        }
    }



    private void Update()
    {
        if(takeDamageDebug)
        {
            TakeDamage(1);
            takeDamageDebug = false;
        }
    }

    public void repairObject(int repairAmount)
    {
        pipeSound.pitch = Random.Range(1.6f, 2.2f);
        pipeSound.Play();
        currentMesh -= 1;
        filter.mesh = meshes[currentMesh];
        health = health + repairAmount;
        //alertUI.problemCurrent += repairAmount;
        if (Old_GameplayEvents.instance != null)
        {
            Old_GameplayEvents.instance.shipCurrenHealth += repairAmount;
            Old_GameplayEvents.instance.AdjustUI();
        }

        // health = Mathf.Clamp(health + repairAmount, 0, healthMax);
        
    }



    public void TakeDamage(int damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            currentMesh += 1;
            filter.mesh = meshes[currentMesh];
            mesh.material.color += Color.red;
            if (alertUI != null) { alertUI.problemCurrent -= damageTaken; }
            if(Old_GameplayEvents.instance != null)
            {
                Old_GameplayEvents.instance.shipCurrenHealth -= damageTaken;
                Old_GameplayEvents.instance.AdjustUI();
            }
            //Debug.Log("Health Points : " + health);
            if (AudioEventManager.instance != null)
            {
                AudioEventManager.instance.PlaySound("pipebreak", .7f, 1, 0);
            }
            
            if(!steamEffect.isPlaying)
            {
                steamEffect.Play();
                steamEffect2.Play();
            }
            
        }

    }
   
   
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    public void TakeDamage(object explosionDamage)
    {
        throw new System.NotImplementedException();
    }
}
