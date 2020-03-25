using System;
using UnityEngine;

namespace LOD_avatar.scripts.Manager
{
    public class AvatarLodManager : MonoBehaviour
    {
        #region Singelton

        public static AvatarLodManager instance;
        [SerializeField] private LODFadeMode globalFadeMode;
        [SerializeField] private int lodUpperBound;
        [SerializeField] private float[] lodQuality;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        #endregion


        public LODFadeMode GetFadeMode()
        {
            return globalFadeMode;
        }


        public int GetLodUpperBound()
        {
            return lodUpperBound;
        }

        public float GetSpecificLodValue(int lodLevel)
        {
//            throw new NotImplementedException();
            return lodQuality[lodLevel];

        }
    }
}