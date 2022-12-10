using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ClothingBaseSwitcher : MonoBehaviour
{
    public Button negative;
    public Button positive;
    public string type;
    public int index;
	public GameObject listOfObjects;
    
    private void Start()
    {
       

        negative.onClick.AddListener(() =>
        {
            if (index > 0)

            {
                index--;
                listOfObjects.transform.GetChild(index).gameObject.SetActive(true);
                listOfObjects.transform.GetChild(index + 1).gameObject.SetActive(false);
            }
            else
            {
                index = listOfObjects.transform.childCount - 1;
                listOfObjects.transform.GetChild(index).gameObject.SetActive(true);
                listOfObjects.transform.GetChild(0).gameObject.SetActive(false);
            }
         
           
           
        });
        positive.onClick.AddListener(() =>
        {
            if (index < listOfObjects.transform.childCount - 1)
            {
                index++;
                listOfObjects.transform.GetChild(index).gameObject.SetActive(true);
                listOfObjects.transform.GetChild(index - 1).gameObject.SetActive(false);
            }
            else
            {
                index = 0;
                listOfObjects.transform.GetChild(index).gameObject.SetActive(true);
                listOfObjects.transform.GetChild(listOfObjects.transform.childCount - 1).gameObject.SetActive(false);
            }
          
          
        });
    }
}
