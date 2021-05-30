using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Saving
{
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ConvertToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}
