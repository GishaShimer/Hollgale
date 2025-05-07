using UnityEngine;

    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTextureDefault;

        [SerializeField] private Vector2 clickPosition = Vector2.zero;

        [SerializeField] private bool isCursor = true;

    void Start()
        {
        Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
        if (isCursor)
        {
            
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
    public void SetCursorTrue()
    {
        Cursor.visible = true;
    }
    public void SetCursorFalse()
    {
        Cursor.visible = false;
    }
}
