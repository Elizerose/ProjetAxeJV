using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

public class ItemController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlateformesData _data;
    public ColorAbilities itemColor;
    [SerializeField] private AudioClip _collectedSound;
    

    private void Awake()
    {
        TryGetComponent(out _spriteRenderer);
    }
    void Start()
    {
        _data = DatabaseManager.Instance.GetPlateformesData(itemColor);
        if (_data.ItemSprite != null) 
            GetComponent<SpriteRenderer>().sprite = _data.ItemSprite;
        //GetComponent<SpriteRenderer>().color = _data.PowerColor;
        GetComponent<Animator>().runtimeAnimatorController = _data.AnimationController;

        GetComponentInChildren<ParticleSystem>().startColor = _data.PowerColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Collected");
            HUDManager.Instance.DisplayCollectedFeedback(itemColor);
            _data.number += 1;
            GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(WaitForParticules());
        }
    }

    IEnumerator WaitForParticules()
    {
        AudioManager.Instance.PlaySFX(_collectedSound);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    
}
