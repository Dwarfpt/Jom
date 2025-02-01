using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;
    Damageable playerDamageable;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("No player in scene");
        }
        playerDamageable = player.GetComponent<Damageable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // playerDamageable = player.GetComponent<Damageable>();
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    }


    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
    // Update is called once per frame
    private void OnEnable()
    {
        playerDamageable.healhChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerDamageable.healhChanged.RemoveListener(OnPlayerHealthChanged);
    }
    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
