using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class ColorPowerController : MonoBehaviour
{
    // Singleton
    private static ColorPowerController _instance;
    public static ColorPowerController Instance => _instance;

    private Transform colorsList;

    public bool isInColorChoice = false;
    

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Si on appuie sur E on active la roue de couleurs
        if (Input.GetKeyDown(KeyCode.E))
        {
            InvokeColorPalette();
        }
    }



    private void InvokeColorPalette()
    {
        // fermeture des choix
        if (isInColorChoice) 
        {
            isInColorChoice = false;
        }
        else
        {
            // parcourir les transform colors
            foreach (Transform color in colorsList)
            {

            }


            // activation de la roue
            isInColorChoice = true;

            // --- Choix des couleurs

            // couleur de gauche
            if (Input.GetKeyDown(KeyCode.Q))
            { 

            }
            // couleur de droite
            else if (Input.GetKeyDown(KeyCode.D))
            {

            }


            
        }
        
    }
}
