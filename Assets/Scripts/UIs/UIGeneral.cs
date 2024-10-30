using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGeneral : UIBase
{
    public TextMeshProUGUI promptText;

    public void Start()
    {
        Interaction interaction = CharacterManager.Instance.Player.GetComponent<Interaction>();
        if (interaction != null)
        {
            interaction.ActivePromptEvent += ActivePrompt;
            interaction.SetPromptTextEvent += SetPromptText;
        }
    }

    public void ActivePrompt(bool active)
    {
        promptText.gameObject.SetActive(active);
    }
    public void SetPromptText(string text)
    {
        promptText.text = text;
    }
}
