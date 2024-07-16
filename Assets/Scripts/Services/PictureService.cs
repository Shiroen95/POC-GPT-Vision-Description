
using System;
using SFB;
using UnityEngine;
namespace Services{
    public class PictureService
    {
        public static void PickImage(int maxImgSize, Action<Texture2D> callback)
        {
            Texture2D texture = null;
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            // Open file async
            var extensions = new [] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )};
            StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => {
                if(paths.Length == 0) 
                    return; 
                var path = paths[0];
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
                    var factor =  (texture.height < texture.width? texture.width: texture.height)/maxImgSize;
                    var resizedTexture = Resize(texture,texture.width/factor,texture.height/factor);
                    callback(resizedTexture);
                }
            });
            return;
    #else
            
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
    #endif
        }

        public static void TakePicture( int maxSize,Action<Texture2D> callback )
        {
            Texture2D texture = null;
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            // Open file async
            var extensions = new [] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )};
            StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => {
                if(paths.Length == 0) 
                    return; 
                var path = paths[0];
                Debug.Log( "Image path: " + path );
                if( path != null )
                {
                    // Create Texture from selected image
                    texture = NativeGallery.LoadImageAtPath(path, maxSize:maxSize ,false);
                    if( texture == null )
                    {
                        Debug.Log( "Couldn't load texture from " + path );
                        return;
                    }
                    var factor =  (texture.height < texture.width? texture.width: texture.height)/maxSize;
                    var resizedTexture = Resize(texture,texture.width/factor,texture.height/factor);
                    callback(resizedTexture);
                }
            });
            return;
    #else
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
        #endif
        }

        private static Texture2D Resize(Texture2D texture2D, int targetX,int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            return result;
        }
    }
}
