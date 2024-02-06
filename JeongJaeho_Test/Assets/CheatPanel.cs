using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatPanel : MonoBehaviour
{
    private Health player;
    private Health enemy;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<Health>();
        enemy = FindObjectOfType<EnemyAIController>().GetComponent<Health>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player.OnDie -= GameSet;   
        player.OnDie += GameSet;   
        enemy.OnDie -= GameSet;   
        enemy.OnDie += GameSet;   
    }

    private void OnDisable()
    {
        player.OnDie -= GameSet;
        enemy.OnDie -= GameSet;
    }

    private void GameSet(GameObject go)
    {
        Active(false);
    }

    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }

   public void Onclick_KillPlayer()
    {
        var player = FindObjectOfType<PlayerController>();
        var enemy = FindObjectOfType<EnemyAIController>();

        player.Health.TakeDamage(new DamageInfo(enemy.gameObject, 99999, Vector3.zero));
    }

    public void Onclick_KillEnemy()
    {
        var player = FindObjectOfType<PlayerController>();
        var enemy = FindObjectOfType<EnemyAIController>();

        enemy.Health.TakeDamage(new DamageInfo(player.gameObject, 99999, Vector3.zero));
    }

    public void Onclick_Restart()
    {
        SceneManager.LoadScene(0);
    }
}
