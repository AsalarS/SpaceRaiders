using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetBehavior : MonoBehaviour
{
    public float hoverScaleFactor = 1.7f;
    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnMouseOver()
    {
        // Scale the object up
        transform.localScale = originalScale * hoverScaleFactor;
    }

    void OnMouseExit()
    {
        // Scale the object back to its original size
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        // Write a message to the command line (console)
        Debug.Log("Planet has been clicked!" + gameObject.name);
        if (gameObject.name == "Earth") {
            SceneManager.LoadScene(gameObject.name);
        }
    }
}
