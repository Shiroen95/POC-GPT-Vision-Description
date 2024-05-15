using System;
using System.Buffers.Text;
using System.Collections;
using Scripts;
using TMPro;
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
    [SerializeField]
    private GameObject sendingText;


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

    public async void sendRequest (){
        sendingText.SetActive(true);
        await RESTClient.instance.sendGPT4PostRequest(Convert.ToBase64String(DataScript.image.EncodeToJPG()));
        sendingText.SetActive(false);
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
            outputDimensions = new Vector2Int(image.width, image.height),

            // Output an RGB color image format
            outputFormat = TextureFormat.RGBA32,

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
        DataScript.image = new Texture2D(
            request.conversionParams.outputDimensions.x,
            request.conversionParams.outputDimensions.y,
            request.conversionParams.outputFormat,
            false);

        // Copy the image data into the texture
        DataScript.image.LoadRawTextureData(rawData);
        DataScript.image.Apply();
        RotateImage( DataScript.image,90f);
        _image.rectTransform.sizeDelta = request.conversionParams.outputDimensions;
        _image.sprite = Sprite.Create(DataScript.image,new Rect(0, 0, DataScript.image.width, DataScript.image.height),Vector2.zero);
        // Dispose the request including raw data
        request.Dispose();
    }


    public static void RotateImage(Texture2D tex, float angleDegrees)
    {
        int width = tex.width;
        int height = tex.height;
        float halfHeight = height * 0.5f;
        float halfWidth = width * 0.5f;

        var texels = tex.GetRawTextureData<Color32>();
        var copy = System.Buffers.ArrayPool<Color32>.Shared.Rent(texels.Length);
        Unity.Collections.NativeArray<Color32>.Copy(texels, copy, texels.Length);

        float phi = Mathf.Deg2Rad * angleDegrees;
        float cosPhi = Mathf.Cos(phi);
        float sinPhi = Mathf.Sin(phi);

        int address = 0;
        for (int newY = 0; newY < height; newY++)
        {
            for (int newX = 0; newX < width; newX++)
            {
                float cX = newX - halfWidth;
                float cY = newY - halfHeight;
                int oldX = Mathf.RoundToInt(cosPhi * cX + sinPhi * cY + halfWidth);
                int oldY = Mathf.RoundToInt(-sinPhi * cX + cosPhi * cY + halfHeight);
                bool InsideImageBounds = (oldX > -1) & (oldX < width)
                                       & (oldY > -1) & (oldY < height);

                texels[address++] = InsideImageBounds ? copy[oldY * width + oldX] : default;
            }
        }

        // No need to reinitialize or SetPixels - data is already in-place.
        tex.Apply(true);

        System.Buffers.ArrayPool<Color32>.Shared.Return(copy);
    }

}
