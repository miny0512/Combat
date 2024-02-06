using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Image _image;
    [SerializeField] private Color _buttonNormalColor;
    [SerializeField] private Color _buttonDownColor;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _player.IsAttackButtonPressed = true;
        _image.color = _buttonDownColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.IsAttackButtonPressed = false;
        _image.color = _buttonNormalColor;
    }

}
