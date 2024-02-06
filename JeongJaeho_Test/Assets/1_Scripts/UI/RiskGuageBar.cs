using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiskGuageBar : MonoBehaviour
{
    public RectTransform RiskGuagebar;
    public Image Progressbar;
    public EnemyAIController enemyAIController;

    private void OnEnable()
    {
        enemyAIController.OnRiskGuageChanged -= OnRiskGuageUpdate;
        enemyAIController.OnRiskGuageChanged += OnRiskGuageUpdate;
    }

    private void OnDisable()
    {
        enemyAIController.OnRiskGuageChanged -= OnRiskGuageUpdate;
    }

    private void OnRiskGuageUpdate(float ratio)
    {
        if(ratio < 1) 
        {
            RiskGuagebar.gameObject.SetActive(true);
        }
        else
        {
            RiskGuagebar.gameObject.SetActive(false);
        }
        Progressbar.fillAmount = ratio;
    }

}
