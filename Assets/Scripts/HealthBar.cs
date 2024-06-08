using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float initialHealth;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public EnemyAi enemy;   // reference EnemyAI class  monsterHealth
    public FistDamage fistDMG;
    private float lerpSpeed = 0.05f;
    
    
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAi>();
        fistDMG = GameObject.FindGameObjectWithTag("GameController").GetComponent<FistDamage>();
        initialHealth = enemy.MonsterHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != enemy.MonsterHealth)
        {
            UpdateHealthUI();
        }

        // if collision detection with fist detected, take damage based on the int by fists
        if (fistDMG.IsFistColliding()) {
            takeDamage(fistDMG.damageAmount);
        }

        if (healthSlider.value != easeHealthSlider.value){
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, initialHealth, lerpSpeed);
        }
    }

    void takeDamage(int damage){
        // calls the TakeDamage from EnemyAi class
        enemy.TakeDamage(damage);
        // update the Health on UI
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthSlider.maxValue = initialHealth;

        // display MonsterHealth from EnemyAi class, should update when automatically when taken damage
        healthSlider.value = enemy.MonsterHealth;
    }
}
