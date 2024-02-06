using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button Button;
    public Image CooltimeImage;
    public Image Icon;
    public SkillBase Skill;


    private void OnEnable()
    {
        Binding();
    }

    private void OnDisable()
    {
        UnBinding();
    }

    private void Update()
    {
        Button.interactable = Skill.IsReady;
    }
    private void OnSkillCooltimeChanged(float ratio)
    {
        CooltimeImage.fillAmount = ratio;
    }

    private void UseSkill()
    {
        Skill.UseSkill();
    }

    private void Binding()
    {
        Button.onClick.AddListener(UseSkill);
        Icon.sprite = Skill.Icon;
        Skill.OnSkillCooltimeChanged -= OnSkillCooltimeChanged;
        Skill.OnSkillCooltimeChanged += OnSkillCooltimeChanged;
    }

    private void UnBinding()
    {
        Button.onClick.RemoveAllListeners();
        Icon.sprite = null;
        Skill.OnSkillCooltimeChanged -= OnSkillCooltimeChanged;
    }
}
