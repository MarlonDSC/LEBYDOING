using UnityEngine;
using UnityEngine.UI;

public class LevelStar : MonoBehaviour
{
    public Sprite[] StarsSprites;
    Image Star;
    void Awake()
    {
        /*
        Level Star
            Star
        */
        Star = transform.GetChild(2).GetComponent<Image>();
    }

    public void SetData(string starColour)
    {
        starColour = "Star " + starColour;
        Debug.Log(starColour);
        for (int i = 0; i < StarsSprites.Length; i++)
        {
            if (starColour == StarsSprites[i].name)
            {
                Star.sprite = StarsSprites[i];
            }
        }
    }
}
