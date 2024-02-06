using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private RectTransform _knob;
    [SerializeField] private RectTransform _bg;
    [SerializeField, Readonly] private float _range;
    [SerializeField, Readonly] private Vector3 _moveDirection;
    public static event Action<Vector3> OnMove;
    private Vector2 _pivotPosition;
    private float _bgWidth;
    private float _bgHeight;

    private void Awake()
    {
         var bg = GetComponent<RectTransform>();
        _range = _bg.rect.width * .5f;
        _bgWidth = bg.rect.width * .5f;
        _bgHeight = bg.rect.height * .5f;

        _knob.gameObject.SetActive(false);
        _bg.gameObject.SetActive(false);
    }


    public void OnDrag(PointerEventData eventData)
    {
        _knob.localPosition = _pivotPosition + Vector2.ClampMagnitude((eventData.position/_mainCanvas.scaleFactor) - _pivotPosition, _range);
        var dir = ((Vector2)_knob.localPosition - _pivotPosition).normalized;
        var lerp = Mathf.Lerp(0, 1, Vector2.Distance(_knob.localPosition, _pivotPosition) / _range);
        _moveDirection = new Vector3(dir.x, 0, dir.y) * lerp;
        OnMove?.Invoke(_moveDirection);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _knob.gameObject.SetActive(true);
        _bg.gameObject.SetActive(true);
        _pivotPosition = eventData.position / _mainCanvas.scaleFactor;
        _bg.localPosition = _pivotPosition;
        _knob.localPosition = _pivotPosition;
        _moveDirection = Vector3.zero;
        OnMove?.Invoke(_moveDirection);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _knob.gameObject.SetActive(false);
        _bg.gameObject.SetActive(false);
        _moveDirection=Vector3.zero;
        OnMove?.Invoke(_moveDirection);
    }
}
