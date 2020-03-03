using UnityEngine;
using UnityEngine.UI;

public class LoadingFillIcon : MonoBehaviour
{
    [SerializeField]
    float speed = .2f;

    Image img;
    int from = 0, to = 1;
    float t = 0f;

    void Awake()
    {
        img = GetComponent<Image>();
    }
	
    void Update()
    {
        t += speed * Time.deltaTime;
        img.fillAmount = Mathf.Lerp(from, to, t);

        if((int)t == 1)
        {
            t = 0f;
            from = from == 0 ? 1 : 0;
            to = to == 0 ? 1 : 0;
            img.fillClockwise = !img.fillClockwise;
        }
    }
}