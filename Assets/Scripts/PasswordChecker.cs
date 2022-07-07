using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordChecker : MonoBehaviour
{
    public TMP_InputField InputPassword;
    public TMP_Text FinalText;

    //public Button Sound;
    public Button ClearDataBase;
    public Button Sound;
    public GameObject keyboard;

    public void Start()
    {
        //PauseUI.SetActive(false);
    }
    public void ButtonUpdate()
    {
        if (InputPassword.text.ToLower() == "ritchie")
        {
            Sound.gameObject.SetActive(true);
            ClearDataBase.gameObject.SetActive(true);
            keyboard.SetActive(false);
            FinalText.text = "Access granted";
        }
        else
        {
            Sound.gameObject.SetActive(false);
            ClearDataBase.gameObject.SetActive(false);
            FinalText.text = "You are not the Admin!";
        }
    }
    public void reset()
    {
        Sound.gameObject.SetActive(false);
        ClearDataBase.gameObject.SetActive(false);
        keyboard.SetActive(true);
        FinalText.text = "";
        InputPassword.text = "";
    }
}
