using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public CharacterStats characterStats;
    public LastPlayerController lastPlayerController;
    public Text healthText;
    public Slider healthSlider;


    void Start()
    {
        healthSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
