using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimations : MonoBehaviour
{

    private float _time = 0f;
    [SerializeField] private Anim _animationType;
    [SerializeField] private float _duration = 1f;

    [Header("Animation de couleur / opacité")]
    [SerializeField] private float _startFontSize;
    [SerializeField] private float _endFontSize;

    [Header("Animation de la taille")]
    [SerializeField] private Vector3 _startSize;
    [SerializeField] private Vector3 _endSize;

    [Header("Animation de la couleur")]
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;

    public enum Anim
    {
        none,
        size,
        fontsize,
        color
    }



    void Update()
    {
        if (this.enabled)
        {
            float ratio = _time / _duration;
            float t = Mathf.Sin(ratio * Mathf.PI);

            if (_animationType == Anim.size)
            {
                GetComponent<RectTransform>().localScale = Vector3.Lerp(_startSize, _endSize, t);
            }
            else if (_animationType == Anim.fontsize)
            {
                GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(_startFontSize, _endFontSize, t);
            }
            else if ( _animationType == Anim.color)
            {
                GetComponent<Image>().color = Color.Lerp(_startColor, _endColor, t);
            }


            _time += Time.unscaledDeltaTime;

            if (_time >= _duration)
                _time = 0f;

        }


    }

    


}
