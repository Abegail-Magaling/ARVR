using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{

    private void OnToggleSettings(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf); // Set the settings panel active based on toggle state
    }
}
