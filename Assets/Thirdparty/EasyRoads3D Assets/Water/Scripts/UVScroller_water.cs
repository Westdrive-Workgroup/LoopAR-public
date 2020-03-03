using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller_water : MonoBehaviour {

	private Material mat;
	public float scrollSpeed = 0.02f;
	Transform lightDir;

	void Start () {
		mat = gameObject.GetComponent<MeshRenderer>().material;
		Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
		foreach (Light l in lights) {
			if (l.type == LightType.Directional) {
				lightDir = l.transform;
				break;
			}
		}
	}

	void Update () {
		var offset = Time.time * scrollSpeed;
		mat.SetTextureOffset ("_MainTex", new Vector2(offset*0.5f, offset*1));
		mat.SetTextureOffset ("_HeightTex", new Vector2(offset/2, offset));
		mat.SetTextureOffset ("_FoamTex", new Vector2(offset/4, offset*1));
		if(lightDir)
			mat.SetVector("_WorldLightDir", lightDir.forward);
		else
			mat.SetVector("_WorldLightDir", new Vector3(0.7f,0.7f,0.0f));
	}
}
