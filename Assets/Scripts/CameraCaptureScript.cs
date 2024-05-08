using System;
using System.Buffers.Text;
using System.Collections;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraCaptureScript : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager _arCameraManager;
    [SerializeField]
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
        Debug.Log(image);
        // Create the async conversion request
        var request = image.ConvertAsync(new XRCpuImage.ConversionParams
        {
            // Use the full image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Optionally downsample by 2
            outputDimensions = new Vector2Int(image.width/2, image.height/2),

            // Output an RGB color image format
            outputFormat = TextureFormat.RGBA32,

            // Flip across the Y axis
            transformation = XRCpuImage.Transformation.MirrorX
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
        DataScript.image = new Texture2D(
            request.conversionParams.outputDimensions.x,
            request.conversionParams.outputDimensions.y,
            request.conversionParams.outputFormat,
            false);

        // Copy the image data into the texture
        DataScript.image.LoadRawTextureData(rawData);
        DataScript.image.Apply();
        _image.rectTransform.sizeDelta = request.conversionParams.outputDimensions;
        _image.sprite = Sprite.Create(DataScript.image,new Rect(0, 0, image.width/2, image.height/2),Vector2.zero);
        // Dispose the request including raw data
        request.Dispose();
    }

}
