using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMessage : UIScreenBase
{
    const int SHOWTIME = 10;
    TMP_Text _messageText;
    WaitForSeconds _delay = new WaitForSeconds(SHOWTIME);

    protected override void Awake()
    {
        base.Awake();
        _messageText = transform.Find("Text (TMP) - Message").GetComponent<TMP_Text>();
    }

    public void SetMessage(string message)
    {
        _messageText.text = message;
        StartCoroutine(OFFMessage());
    }

    IEnumerator OFFMessage()
    {
        yield return _delay;
        _messageText.text = string.Empty;
    }
}
