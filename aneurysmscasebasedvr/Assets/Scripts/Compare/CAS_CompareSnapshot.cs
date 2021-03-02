using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_CompareSnapshot : MonoBehaviour
    {
        public RawImage rawImage;

        public void SetRawImage(RenderTexture targetTexture)
        {
            rawImage.texture = targetTexture;
        }
    }
}