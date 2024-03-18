using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraCaptureScript : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager _arCameraManager;

    private Image _image;


    public void takePicture(){
        AsynchronousConversion();
    }
    

    void AsynchronousConversion()
    {
        // Acquire an XRCpuImage
        if (_arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            // If successful, launch an asynchronous conversion coroutine
            StartCoroutine(ConvertImageAsync(image));

            // It is safe to dispose the image before the async operation completes
            image.Dispose();
        }
    }

    IEnumerator ConvertImageAsync(XRCpuImage image)
    {
        // Create the async conversion request
        var request = image.ConvertAsync(new XRCpuImage.ConversionParams
        {
            // Use the full image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Optionally downsample by 2
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Output an RGB color image format
            outputFormat = TextureFormat.RGB24,

            // Flip across the Y axis
            transformation = XRCpuImage.Transformation.MirrorY
        });

        // Wait for the conversion to complete
        while (!request.status.IsDone())
            yield return null;

        // Check status to see if the conversion completed successfully
        if (request.status != XRCpuImage.AsyncConversionStatus.Ready)
        {
            // Something went wrong
            Debug.LogErrorFormat("Request failed with status {0}", request.status);

            // Dispose even if there is an error
            request.Dispose();
            yield break;
        }

        // Image data is ready. Let's apply it to a Texture2D
        var rawData = request.GetData<byte>();

        // Create a texture
        var texture = new Texture2D(
            request.conversionParams.outputDimensions.x,
            request.conversionParams.outputDimensions.y,
            request.conversionParams.outputFormat,
            false);

        // Copy the image data into the texture
        texture.LoadRawTextureData(rawData);
        texture.Apply();
        Debug.Log("Texture was loaded");
        _image.sprite = Sprite.Create(texture,new Rect(0, 0, image.width, image.height),Vector2.zero);
        // Dispose the request including raw data
        request.Dispose();
    }

}
