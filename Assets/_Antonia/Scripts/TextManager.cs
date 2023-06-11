using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TMP_Text ParagraphDisplay; // The TextMeshPro object that displays the text
    public List<string> Paragraphs; // A list of the paragraphs you want this script to go through
    public float ParagraphChangeDelay = 1; // Delay in seconds between changing paragraphs

    private int _paragraphPosition = 0; // Current position in the Paragraph list

    void Start()
    {
        CheckParagraphPosition();
    }

    void CheckParagraphPosition()
    {
        // Check if the current position is less than the amount of paragraphs in the list
        if (_paragraphPosition <= Paragraphs.Count - 1)
        {
            StartCoroutine(ChangeParagraph());
        }
        else
        {
            EndOfParagraphs(); // If not, end the loop
        }
    }

    IEnumerator ChangeParagraph()
    {
        // Change the currently displayed paragraph and then wait # of seconds before running the loop again
        ParagraphDisplay.text = Paragraphs[_paragraphPosition];
        _paragraphPosition++;
        yield return new WaitForSeconds(ParagraphChangeDelay);
        CheckParagraphPosition();
    }

    void EndOfParagraphs()
    {
        // Do something when the program has gone through all paragraphs
    }
}
