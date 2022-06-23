using UnityEngine;

// Controller for the side strip effect
public class SideStripEffect : MonoBehaviour
{
    [Tooltip("Attach the object at the left or right screen corner defined by the camera object")]
    public bool AttachToTopLeftScreenCorner = true;

    [Tooltip("Maximal factor by which the particle size can grow due to rescaling")]
    public float MaxParticleSizeGrowFactor = 1f;

    [Tooltip("The title UI element to attach the object to")]
    public RectTransform TitleRectTransform;

    [Tooltip("Rotation speed of the strip")]
    public float Omega;

    // design time horizontal scale
    private float designTimeHorizontalScale;

    // design time size of the particles
    private float designTimeParticleStartSize;

    // particle system to be scaled
    private ParticleSystem targetParticleSystem;

    // the target camera
    private Camera targetCamera;

    // current rotation angle
    private float alpha = 0f;

    private void OnEnable()
    {
        designTimeHorizontalScale = transform.localScale.x;
    }

    // Use this for initialization
    private void Start()
    {
        targetParticleSystem = GetComponent<ParticleSystem>();
        designTimeParticleStartSize = targetParticleSystem.startSize;
        targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        // get rects in world coordinates
        WorldSpaceRect cameraRect = targetCamera.GetComponent<Camera>().GetCameraWorldspaceRect();
        WorldSpaceRect titleRect = TitleRectTransform.GetComponent<RectTransform>().GetWorldSpaceRect();

        // compute new object coordinates
        float newHorzScale = GetWidth(AttachToTopLeftScreenCorner);
        Vector2 newPosition = AttachToTopLeftScreenCorner ?
                              new Vector2(cameraRect.Left + (0.5f * newHorzScale), cameraRect.Top) :
                              new Vector2(titleRect.Right + (0.5f * newHorzScale), cameraRect.Bottom);
        float maxParticleStartSize = designTimeParticleStartSize * MaxParticleSizeGrowFactor;
        float newParticleStartSize = designTimeParticleStartSize * (newHorzScale / designTimeHorizontalScale);
        newParticleStartSize = Mathf.Clamp(newParticleStartSize, 0f, maxParticleStartSize);

        // set new object coordinates
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        transform.localScale = new Vector3(newHorzScale, transform.localScale.y, transform.localScale.z);
        targetParticleSystem.startSize = newParticleStartSize;

        // rotate
        alpha += Time.deltaTime * Omega; 
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, alpha, transform.eulerAngles.z);
    }

    private float GetWidth(bool attachToTopLeftScreenCorner)
    {
        WorldSpaceRect cameraRect = targetCamera.GetComponent<Camera>().GetCameraWorldspaceRect();
        WorldSpaceRect titleRect = TitleRectTransform.GetComponent<RectTransform>().GetWorldSpaceRect();

        float width = attachToTopLeftScreenCorner ?
                      titleRect.Left - cameraRect.Left :
                      cameraRect.Right - titleRect.Right;

        return width;
    }
}