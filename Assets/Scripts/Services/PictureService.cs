using UnityEngine;
namespace Services{
    public class PictureService
    {
        public static Texture2D PickImage(int maxImgSize)
        {
            Texture2D texture = null;
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
            {
                Debug.Log( "Image path: " + path );
                if( path != null )
                {
                    // Create Texture from selected image
                    texture = NativeGallery.LoadImageAtPath(path, maxImgSize ,false);
                    if( texture == null )
                    {
                        Debug.Log( "Couldn't load texture from " + path );
                        return;
                    }
                    Debug.Log("height: " + texture.height);
                    Debug.Log("width: " + texture.width);
                    
                }
            } );
            return texture;
        }
    }
}
