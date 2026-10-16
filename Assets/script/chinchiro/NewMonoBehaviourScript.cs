
using UnityEngine;

public class ChinchiroDie : MonoBehaviour
{
    [System.Serializable]
    public class Face
    {
        [Range(1, 6)]
        public int value = 1;

        [Tooltip("‚±‚Ì–Ú‚ª‚ ‚é–Ê‚ÌŠOŒü‚«•ûŒü")]
        public Vector3 localNormal = Vector3.up;
    }

    [Header("Še–Ê‚Ì”Ô†‚ÆŒü‚«")]
    [SerializeField]
    private Face[] faces = new Face[6]
    {
        new Face { value = 1, localNormal = Vector3.up },
        new Face { value = 6, localNormal = Vector3.down },
        new Face { value = 2, localNormal = Vector3.forward },
        new Face { value = 5, localNormal = Vector3.back },
        new Face { value = 3, localNormal = Vector3.right },
        new Face { value = 4, localNormal = Vector3.left }
    };

    public int GetTopFaceValue()
    {
        int result = 1;
        float highestDot = -2f;

        foreach (Face face in faces)
        {
            Vector3 worldNormal =
                transform.TransformDirection(face.localNormal.normalized);

            float dot = Vector3.Dot(worldNormal, Vector3.up);

            if (dot > highestDot)
            {
                highestDot = dot;
                result = face.value;
            }
        }

        return result;
    }
}
