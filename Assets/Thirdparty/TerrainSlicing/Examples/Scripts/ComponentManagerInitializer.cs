
namespace DynamicLoadingKit
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ComponentManagerInitializer : MonoBehaviour
    {
        [SerializeField]
        GameObject loadingCanvasToDisableAfterInitialization;
        
        public void InitializeComponentManagerGradually()
        {
            StartCoroutine(GradualInitialization());
        }

        IEnumerator<YieldInstruction> GradualInitialization()
        {
            //Should be only one componet manager in the scene so we can use FindObjectOfType to get it
            ComponentManager componentManagerToInitialize = FindObjectOfType<ComponentManager>();

            //You could load custom save data here if you had any

            //string saveData = LoadManager.GetSaveData();
            //componentManagerToInitialize.LoadSaveData(saveData);

            //This is equivalent to using StartCoroutine, but it's slightly more performant
            IEnumerator<YieldInstruction> e = componentManagerToInitialize.InitializeGradually();
            while (e.MoveNext())
                yield return e.Current;

            //You can also just use this code. It's more straightfoward, but incurs an additional cost 
            //for the StartCoroutine call

            //yield return StartCoroutine(componentManagerToInitialize.InitializeGradually());

            //disable loading canvas
            if(loadingCanvasToDisableAfterInitialization != null)
                loadingCanvasToDisableAfterInitialization.SetActive(false);
        }
    }
}