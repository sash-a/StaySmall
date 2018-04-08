using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayer
{
    private Text _text;

    public TextDisplayer(Text t)
    {
        _text = t;
    }

    public IEnumerator ShowMessage(string message, float delay)
    {
        _text.text = message;
        _text.enabled = true;
        _text.color = Color.white;
        yield return new WaitForSeconds(delay);
        _text.enabled = false;
    }
}