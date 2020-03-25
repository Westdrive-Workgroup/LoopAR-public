using LOD_avatar.scripts.Manager;
using UnityEngine;

namespace LOD_avatar.scripts.Controller
{
    public class AvatarLodController : MonoBehaviour
    {
        private LODGroup lodg;

        //LOD[] lods = new LOD[4];

        // Start is called before the first frame update
        void Start()
        {
            lodg = gameObject.GetComponent<LODGroup>();

            LOD[] updateLods = new LOD[lodg.GetLODs().Length];
            for (int lodLevel = 0; lodLevel < lodg.GetLODs().Length; lodLevel++)
            {
                LOD loD = new LOD
                {
                    screenRelativeTransitionHeight = AvatarLodManager.instance.GetSpecificLodValue(lodLevel),
                    renderers = lodg.GetLODs()[lodLevel].renderers
                };
                updateLods[lodLevel] = loD;
            }

            lodg.SetLODs(updateLods);
        }

        // Update is called once per frame
        void Update()
        {
            lodg.fadeMode = AvatarLodManager.instance.GetFadeMode();
        }
    }
}