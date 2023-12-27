using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicUIControl : MonoBehaviour
{
    public Image image; // Change RawImage to Image
    public TextMeshProUGUI priceTagText;
    public TextMeshProUGUI headerText;

    // You can assign sprites to these in the Unity Editor
    public Sprite defaultImage;
    public Sprite otherImage;

    // Default price tag text
    private string defaultPriceTag = "$10.99";
    // Default price tag text
    private string defaultHeaderText = "test";

    // Use this for initialization
    void Start()
    {
        // Set the default values
        SetImage(defaultImage);
        SetPriceTag(defaultPriceTag);
        SetItemHeader(defaultHeaderText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Set the image of the Image component
    void SetImage(Sprite newImage)
    {
        if (image != null)
        {
            image.sprite = newImage;
        }
    }

    // Set the text of the Text component
    void SetPriceTag(string newText)
    {
        if (priceTagText != null)
        {
            priceTagText.text = newText;
        }
    }

    void SetItemHeader(string newText)
    {
        if (headerText != null)
        {
            headerText.text = newText;
        }
    }

}