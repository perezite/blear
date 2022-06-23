using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CameraHelper
{
    public static float GetScreenPortraitAspectRatio()
    {
        return Screen.height / (float)Screen.width;
    }

    // get best matching sprite for given portraitAspectRatio
    public static Sprite GetBestMatchingSprite(ResponsiveSpriteEntry[] responsiveSprites, float referencePortraitAspectRatio)
    {
        List<float> aspectRatios = responsiveSprites.Select(x => x.AspectRatio).ToList();

        // choose best matching sprite
        int indexOfBestMatchingAspectRatio =
            GetIndexOfNextWiderPortraitAspectRatio(aspectRatios, referencePortraitAspectRatio);
        Sprite bestMatchingSprite = responsiveSprites[indexOfBestMatchingAspectRatio].Sprite;

        return bestMatchingSprite;
    }

    // return the index of the next wider portrait aspect ratio
    public static int GetIndexOfNextWiderPortraitAspectRatio(List<float> portraitAspectRatios, float referencePortraitAspectRatio)
    {
        List<float> sortedPortraitAspectRatios = portraitAspectRatios.OrderByDescending(x => x).ToList();
        float bestMatchingPortraitAspectRatio = sortedPortraitAspectRatios[sortedPortraitAspectRatios.Count - 1];

        // find best aspect ratio
        for (int i = 0; i < sortedPortraitAspectRatios.Count; i++)
        {
            float portraitAspectRatio = sortedPortraitAspectRatios[i];

            bool aspectRatiosMatch = Mathf.Approximately(portraitAspectRatio, referencePortraitAspectRatio)
                                     || portraitAspectRatio < referencePortraitAspectRatio;

            if (aspectRatiosMatch)
            {
                bestMatchingPortraitAspectRatio = portraitAspectRatio;
                break;
            }
        }

        return portraitAspectRatios.FindIndex(x => Mathf.Approximately(x, bestMatchingPortraitAspectRatio));
    }

    // get camera rect in world space
    public static WorldSpaceRect GetCameraWorldspaceRect(this Camera camera)
    {
        var bottomLeftScreen = new Vector3(0.0f, 0.0f, camera.nearClipPlane);
        var topRightScreen = new Vector3(1.0f, 1.0f, camera.nearClipPlane);

        var bottomLeftWorld = camera.ViewportToWorldPoint(bottomLeftScreen);
        var topRightWorld = camera.ViewportToWorldPoint(topRightScreen);

        float width = topRightWorld.x - bottomLeftWorld.x;
        float height = topRightWorld.y - bottomLeftWorld.y;
        var worldSpaceRect = new WorldSpaceRect(bottomLeftWorld.x, bottomLeftWorld.y, width, height);

        return worldSpaceRect;
    }

    public static void SetViewport(this Camera camera, float viewportHeight, float viewportVerticalPosition)
    {
        Rect viewport = camera.rect;

        viewport.height = viewportHeight;
        viewport.y = viewportVerticalPosition;

        camera.rect = viewport;
    }
}