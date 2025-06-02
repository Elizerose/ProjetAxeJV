using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrokenLightAnim : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    private float _time = 0f;
    private Light2D _light2D;

    void Start()
    {
        TryGetComponent(out _light2D);
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = _time / _duration;
        float t = EaseOutElastic(ratio);

        _light2D.intensity = Mathf.Lerp(0, 1, t);

        _time += Time.unscaledDeltaTime;

        if (_time >= _duration)
            _time = 0;
    }

    // Fonction EaseOutElastic (inspirée de celle en https://easings.net)
    float EaseOutElastic(float x)
    {
        float c4 = (2f * Mathf.PI) / 3f;

        if (x == 0f)
            return 0f;
        if (x == 1f)
            return 1f;

        return Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
    }

}
