using UnityEngine;

public class AspectUtility : MonoBehaviour
{
    private float _wantedAspectRatio = 1.778f;
    static float wantedAspectRatio;
    static Camera cam;
    static Camera backgroundCam;

    void Awake()
    {
        if (!Const.IsLowAspectRatioFullScreen)
        {
            cam = GetComponent<Camera>() != null ? GetComponent<Camera>() : cam;//Camera.allCameras;
            /*if (!cam) {
                cam = Camera.main;
            }*/

            if (!cam)
            {
                Debug.LogError("No camera available" + this.gameObject.name);
                return;
            }
            wantedAspectRatio = _wantedAspectRatio;
            SetCamera();
        }        
    }

    public static void SetCamera()
    {
        if (!Const.IsLowAspectRatioFullScreen)
        {
            float currentAspectRatio = (float)Screen.width / Screen.height;
            if (currentAspectRatio >= 1.5f)
            {
                return;
            }
            // If the current aspect ratio is already approximately equal to the desired aspect ratio,
            // use a full-screen Rect (in case it was set to something else previously)

            if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f)
            {
                //for(int i = 0; i < cam.Length; i++)
                //{
                cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                //}

                if (backgroundCam)
                {
                    Destroy(backgroundCam.gameObject);
                }
                return;
            }

            // Pillarbox
            if (currentAspectRatio > wantedAspectRatio)
            {
                float inset = 1.0f - wantedAspectRatio / currentAspectRatio;
                //for(int i = 0; i < cam.Length; i++)
                cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
            }// Letterbox
            else
            {
                float inset = 1.0f - currentAspectRatio / wantedAspectRatio;
                //for(int k = 0; k < cam.Length; k++){
                cam.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
                //}
            }

            if (!backgroundCam)
            {
                // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
                backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
                backgroundCam.depth = int.MinValue;
                backgroundCam.clearFlags = CameraClearFlags.SolidColor;
                backgroundCam.backgroundColor = Color.black;
                backgroundCam.cullingMask = 0;
            }
        }
    }

    public static int screenHeight
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return 1;
            }
            else
            {
                return (int)(Screen.height * cam.rect.height);
            }
        }
    }

    public static int screenWidth
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return 1;
            }
            else
            {
                return (int)(Screen.width * cam.rect.width);
            }
        }
    }

    public static int xOffset
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return 0;
            }
            else
            {
                return (int)(Screen.width * cam.rect.x);
            }
        }
    }

    public static int yOffset
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return 0;
            }
            else
            {
                return (int)(Screen.height * cam.rect.y);
            }
        }
    }

    public static Rect screenRect
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return new Rect();
            }
            else
            {
                if (cam == null) {
                    return new Rect();
                }
                return new Rect(cam.rect.x * Screen.width, cam.rect.y * Screen.height, cam.rect.width * Screen.width, cam.rect.height * Screen.height);
            }
        }
    }

    public static Vector3 mousePosition
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.y -= (int)(cam.rect.y * Screen.height);
                mousePos.x -= (int)(cam.rect.x * Screen.width);

                return mousePos;
            }
        }
    }

    public static Vector2 guiMousePosition
    {
        get
        {
            if (Const.IsLowAspectRatioFullScreen)
            {
                return Vector2.zero;
            }
            else
            {
                Vector2 mousePos = Event.current.mousePosition;
                mousePos.y = Mathf.Clamp(mousePos.y, cam.rect.y * Screen.height, cam.rect.y * Screen.height + cam.rect.height * Screen.height);
                mousePos.x = Mathf.Clamp(mousePos.x, cam.rect.x * Screen.width, cam.rect.x * Screen.width + cam.rect.width * Screen.width);

                return mousePos;
            }
        }
    }
}