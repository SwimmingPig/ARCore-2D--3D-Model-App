using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoButton : MonoBehaviour {
    public GameObject AndyAndroidPrefab;
    public GameObject Cube;
	public void click() {
        //Debug.Log("Photot button pressed");
        captureScreenshot();
        StartCoroutine(captureScreenshot());
        //Cube = OBJLoader.LoadOBJFile(Application.persistentDataPath + "Input.obj");
        //var andyObject = Instantiate(AndyAndroidPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0));

    }

    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        //string path = Application.persistentDataPath + "Screenshots" + Screen.width + "X" + Screen.height + "" + ".png";
        string path = Application.persistentDataPath + "Input.png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(path, imageBytes);
    }
}



// IEnumerator TakeScreenshot()
// {

//     string imageName = "screenshot.png";

//     // Take the screenshot
//     ScreenCapture.CaptureScreenshot(imageName);

//     //Wait for 4 frames
//     // for (int i = 0; i < 5; i++)
//     // {
//     //     yield return null;
//     // }

//     // // Read the data from the file
//     // byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);

//     // // Create the texture
//     // Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

//     // // Load the image
//     // screenshotTexture.LoadImage(data);

//     // // Create a sprite
//     // Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));

//     // // Set the sprite to the screenshotPreview
    // screenshotPreview.GetComponent<Image>().sprite = screenshotSprite;

// }
