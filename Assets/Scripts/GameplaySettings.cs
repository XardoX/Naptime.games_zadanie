using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameplaySettings : ScriptableObject
{
    public ObjectCountSetting[] objectsCountSettings;


    [System.Serializable]
    public class ObjectCountSetting
    {
        [Min(0)]
        public int count;

        [Min(0)]
        public int cameraSize;

        public Color buttonIconColor;

    }
}
