using System;
using UnityEngine;
namespace Services{
    public class PictureService
    {
        public static void PickImage(int maxImgSize, Action<Texture2D> callback)
        {
            Texture2D texture = null;
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
            {
                Debug.Log( "Image path: " + path );
                if( path != null )
                {
                    // Create Texture from selected image
                    texture = NativeGallery.LoadImageAtPath(path, maxSize:maxImgSize ,false);
                    if( texture == null )
                    {
                        Debug.Log( "Couldn't load texture from " + path );
                        return;
                    }
                    Debug.Log("height: " + texture.height);
                    Debug.Log("width: " + texture.width);
                    callback(texture);
                }
            } );
        }








    public static void TakePicture( int maxSize,Action<Texture2D> callback )
    {
        Texture2D texture = null;
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create a Texture2D from the captured image
                 texture = NativeCamera.LoadImageAtPath( path, maxSize,false );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                callback(texture);
            }
        }, maxSize );
    }









        
    }
}
