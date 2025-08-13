using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_InputField nameField;

    public void StartNew()
    {
        MainManager.Instance.playerName = nameField.text;
        SceneManager.LoadScene(1); // gameplay scene
    }
}
