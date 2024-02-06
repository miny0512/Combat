using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpProgressBar : MonoBehaviour
{
    public Health TargetHealth;
    public Image ProgressBar;
    public TMP_Text Text;
    private void OnEnable()
    {
        TargetHealth.OnHealthChanged -= OnHealthChanged;
        TargetHealth.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        TargetHealth.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float ratio)
    {
        ProgressBar.fillAmount = ratio;
        if(Text != null)
        {
            Text.text = $"{(int)(ratio*100f)}%";
        }
    }

}
