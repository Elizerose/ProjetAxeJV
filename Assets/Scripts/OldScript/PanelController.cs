using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public Dictionary<string, Color> Colors = new Dictionary<string, Color>()
    {
        { "water", new Color(0f, 0f, 1f, 0.2f) }
    };


    public static PanelController Instance { get; private set; }

    [Header ("Game Objects")]
    [SerializeField] private GameObject filterPanel;
    [SerializeField] private GameObject SwimMode0;
    [SerializeField] private GameObject SwimMode1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeSwimmingMode(0);
    }

    public void ActiveFilter(string color)
    {
        filterPanel.GetComponent<Image>().color = Colors[color];
        filterPanel.SetActive(true);
    }

    public void DeactiveFilter()
    {
        filterPanel.SetActive(false);
    }

    public void ChangeSwimmingMode(int mode)
    {
        PlayerController.Instance.swimMode = mode;

        Color btnColor = new Color(0.4f,0.9f,1f,1f);
        GameObject CurrentBtn = SwimMode0;
        GameObject OldSwimMode = SwimMode1;

        if (mode == 0)
        {
            CurrentBtn = SwimMode0;
            OldSwimMode = SwimMode1;
        }
        else if (mode == 1)
        {
            CurrentBtn = SwimMode1;
            OldSwimMode = SwimMode0;
        }

        CurrentBtn.GetComponent<Image>().color = btnColor;
        CurrentBtn.GetComponentInChildren<Text>().color = Color.white;

        OldSwimMode.GetComponent<Image>().color = Color.white;
        OldSwimMode.GetComponentInChildren<Text>().color = Color.black;
    }
}
