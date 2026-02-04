using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject missileLauncherPrefab;
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;
    private GameController myGameController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGameController = Object.FindFirstObjectByType<GameController>();
        cursorHotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

        // If not assigned in inspector, try to find it
        if (missileLauncherPrefab == null)
        {
            missileLauncherPrefab = GameObject.Find("MissileLauncher"); // Replace with your launcher's name

            if (missileLauncherPrefab == null)
            {
                Debug.LogError("Missile Launcher not found! Please assign it in the inspector.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame && myGameController.currentMissilesLoadedInLauncher > 0)
        {


            // Get mouse world position
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            // Spawn missile
            GameObject missile = Instantiate(
                missilePrefab,
                missileLauncherPrefab.transform.position,
                Quaternion.identity
            );
            myGameController.currentMissilesLoadedInLauncher--;
            myGameController.UpdateMissilesInLauncherText();
        }
    }
}
