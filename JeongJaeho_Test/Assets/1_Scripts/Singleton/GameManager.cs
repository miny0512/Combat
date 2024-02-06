using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public void OnGameSet(GameObject winObject)
    {
        if (winObject.CompareTag("Player"))
        {
            var player = winObject.GetComponent<PlayerController>();
            player.FrontViewCamera.Priority = 200;
            player.Stop();
            player.Rigidbody.velocity = Vector3.zero;
            player.Animator.Play(player.ANIM_VICTORY);
        }
        else
        {
            var enemy = winObject.GetComponent<EnemyAIController>();
            enemy.FrontViewCamera.Priority = 200;
            enemy.Stop();
            enemy.Rigidbody.velocity = Vector3.zero;
            enemy.Animator.Play(enemy.ANIM_VICTORY);
        }
        StartCoroutine(TimeSlow());
    }

    public void ResetTimescale()
    {
        Time.timeScale = 1.0f;
    }
    public IEnumerator TimeSlow()
    {
        float elapsedTime = 0;
        float duration = 5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0, 1, elapsedTime / duration);
            yield return null;
        }
        ShowRestartButton();
    }

    public void ShowRestartButton()
    {
        var btn = Instantiate(Resources.Load("RestartButton"));
        btn.GetComponentInChildren<Button>().onClick.AddListener(OnClickRestartButton);
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(0);
    }

}
