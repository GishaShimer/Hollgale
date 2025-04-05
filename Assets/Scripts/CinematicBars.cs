using UnityEngine;
using UnityEngine.UI;


public class CinematicBars : MonoBehaviour
{
    private RectTransform topBar, bottomBar;
    private float changeSizeAmount;
    private float targetSize;
    private bool isActive;

    private void Awake()
    {
        topBar = CreateBar("TopBar", new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0));
        bottomBar = CreateBar("BottomBar", new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0));
    }

    private RectTransform CreateBar(string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
    {
        GameObject barObject = new GameObject(name, typeof(Image));
        barObject.transform.SetParent(transform, false);
        barObject.GetComponent<Image>().color = Color.black;

        RectTransform rectTransform = barObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.sizeDelta = sizeDelta;

        return rectTransform;
    }

    private void Update()
    {
        if (isActive)
        {
            Vector2 sizeDelta = topBar.sizeDelta;
            sizeDelta.y += changeSizeAmount * Time.deltaTime;
            if (changeSizeAmount > 0)
            {
                if (sizeDelta.y >= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            else
            {
                if (sizeDelta.y <= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            topBar.sizeDelta = sizeDelta;
            bottomBar.sizeDelta = sizeDelta;
        }
    }


    public void InstantShow()
    {
        targetSize = 300f;
        topBar.sizeDelta = new Vector2(topBar.sizeDelta.x, targetSize);
        bottomBar.sizeDelta = new Vector2(bottomBar.sizeDelta.x, targetSize);
        isActive = true; 
    }

    public void Show(float time = 0.5f)
    {
        targetSize = 300f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }

    public void Hide(float time = 0.5f)
    {
        targetSize = 0f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }
}
