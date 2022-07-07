﻿using UnityEngine;
using TMPro;

public class KeyboardScript : MonoBehaviour
{
    public TMP_InputField TextField;
    public GameObject EngLayoutSml, EngLayoutBig, SymbLayout;
    [SerializeField]
    private bool admin = false;

    public void alphabetFunction(string alphabet)
    {
        if (admin)
        {
            TextField.text = TextField.text + alphabet;
        }
        else if (TextField.text.Length < 3){
            TextField.text = TextField.text + alphabet;
        }

    }

    public void BackSpace()
    {
        if (TextField.text.Length > 0) TextField.text = TextField.text.Remove(TextField.text.Length - 1);
    }

    public void CloseAllLayouts()
    {
        EngLayoutSml.SetActive(false);
        EngLayoutBig.SetActive(false);
        SymbLayout.SetActive(false);
    }

    public void ShowLayout(GameObject SetLayout)
    {
        CloseAllLayouts();
        SetLayout.SetActive(true);
    }
}
