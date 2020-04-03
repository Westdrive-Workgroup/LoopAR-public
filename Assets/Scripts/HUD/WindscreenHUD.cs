using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindscreenHUD : MonoBehaviour
{
    public Text Speed;
    public Text MaxSpeed;
    public Text Date;
    public Text Weather;
    public GameObject NonEventAnzeigen;
    public Image SpeedGauge;
    public GameObject linkeSeite;
    public GameObject rechteSeite;
    public int speedLimit = 70;
    private float nextUpdate = 1;
    public bool Event;
    private float speed;
    public GameObject KameraHalt;
    List<GameObject> currentCollider = new List<GameObject>();
    public CarController _carController;
    public GameObject KreisFuerEvents;
    private GameObject Kreis;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        //Event start
        Event = true;
        NonEventAnzeigen.SetActive(false);
        if (currentCollider.Contains(other.gameObject) == false)
        {
            currentCollider.Add(other.gameObject);
            Kreis = Instantiate(KreisFuerEvents);
            Kreis.transform.SetParent(other.gameObject.transform, false);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        // Remove the GameObject collided with from the list.
        currentCollider.Remove(other.gameObject);
        if (other.gameObject.transform.childCount > 0)
        {
            // we have children!
            Destroy(other.transform.Find("Magical_Circle(Clone)").gameObject);
            //not any more 
        }

    }


    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = (Mathf.FloorToInt(Time.time) + 1);
            // Call your function
            UpdateEverySecond();

        }
        //Wenn kein Event
        if (currentCollider.Count == 0)
        {
            Event = false;

        }
        else
        {
            foreach (GameObject gObject in currentCollider)
            {
                Vector3 direction = gObject.transform.position - KameraHalt.transform.position;
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer and object has a child
                if (Physics.Raycast(KameraHalt.transform.position, direction, out hit, Mathf.Infinity) && (gObject.transform.childCount > 0))
                {
                    switch (hit.collider.gameObject.name)
                    {
                        case "Canvas":
                            gObject.transform.GetChild(0).gameObject.SetActive(true);
                            float dist = Vector3.Distance(gObject.transform.position, hit.point);
                            float quot =(5/dist)+1;
                            Debug.Log(quot + " dist =" + dist);
                            //set Kreis position and rotation
                            Kreis = gObject.transform.Find("Magical_Circle(Clone)").gameObject;
                            Kreis.transform.position = hit.point;
                            Kreis.transform.localScale = new Vector3(0.03f*quot, 0.03f*quot, 1f);
                            Kreis.transform.rotation = new Quaternion(-50f, -180f, 130f, 1f);  

                            linkeSeite.SetActive(false);
                            rechteSeite.SetActive(false);
                            break;
                        case "linkeSeite":
                            gObject.transform.GetChild(0).gameObject.SetActive(false);
                            linkeSeite.SetActive(true);
                            break;
                        case "rechteSeite":
                            gObject.transform.GetChild(0).gameObject.SetActive(false);
                            rechteSeite.SetActive(true);
                            break;
                        default:
                            gObject.transform.GetChild(0).gameObject.SetActive(false);
                            linkeSeite.SetActive(false);
                            rechteSeite.SetActive(false);
                            //Debug.Log("Improper command given!");
                            break;
                    }
                }
                else
                {
                    if (gObject.transform.childCount > 0)
                    {
                        gObject.transform.GetChild(0).gameObject.SetActive(false);

                    }
                }
                Debug.DrawRay(KameraHalt.transform.position, direction * hit.distance, Color.yellow);
            }

        }
    }


    // Update is called once per second
    void UpdateEverySecond()
    {

        if (Event == false)
        {
            NonEventAnzeigen.SetActive(true);            
            linkeSeite.SetActive(false);
            rechteSeite.SetActive(false);
            speed = Mathf.Round(_carController.GetCurrentSpeedInKmH());
            Speed.text = speed + "";
            float quotientSpeed = speed / speedLimit;
            SpeedGauge.fillAmount = 0.75f * (Mathf.Round(quotientSpeed * 36) / 36);


            if (speed >= speedLimit)
            {
                SpeedGauge.color = Color.red;
                Speed.color = Color.red;
                MaxSpeed.color = Color.red;
            }
            else
            {
                SpeedGauge.color = Color.green;
                Speed.color = Color.green;
                MaxSpeed.color = Color.white;
            }

            //Datum
            var today = System.DateTime.Now;
            Date.text = today.ToString("HH:mm");
            //MaxSpeed
            MaxSpeed.text = speedLimit + "";
            Weather.text = "Baumles, CH \n 15°C";


        }
        else
        {
        }
    }

}