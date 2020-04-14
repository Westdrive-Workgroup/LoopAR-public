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
    List<string> objectLinks = new List<string>();
    List<string> objectRechts = new List<string>();
    public CarController _carController;
    public GameObject KreisFuerEvents;
    //notMVP
    //public GameObject PfeilFuersEvent;
    private GameObject Kreis;
    //notMVP
    //private GameObject Pfeil;
    private int p;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Event == false)
        {
            //Event start
            Event = true;
            NonEventAnzeigen.SetActive(false);
        }

        if (currentCollider.Contains(other.gameObject) == false)
        {
            currentCollider.Add(other.gameObject);
            Kreis = Instantiate(KreisFuerEvents);
            Kreis.transform.SetParent(other.gameObject.transform, false);
            //notMVP
            //Pfeil = Instantiate(PfeilFuersEvent);
            //notMVP
            //Pfeil.transform.SetParent(Kreis.transform, false);
            //notMVP
            //Pfeil.SetActive(false);


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
            if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
            if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }

            //not any more 
        }

    }


    // Update is called once per frame
    void LateUpdate()
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
                //Vector3 direction_arrow = KameraHalt.transform.forward;
                RaycastHit hit;
                RaycastHit hit_arrow;

                // Does the ray intersect any objects  and object has a child
                if (Physics.Raycast(KameraHalt.transform.position, direction, out hit, Mathf.Infinity) && (gObject.transform.childCount > 0))
                {
                    switch (hit.collider.gameObject.name)
                    {
                        //make sure that Event collider layer is on IgnoreRaycast!!!
                        case "Canvas":
                            gObject.transform.GetChild(0).gameObject.SetActive(true);
                            float dist = Vector3.Distance(gObject.transform.position, hit.point);
                            float quot = (5 / dist) + 1;
                            //set Kreis position and rotation
                            Kreis = gObject.transform.Find("Magical_Circle(Clone)").gameObject;
                            Kreis.transform.position = hit.point;
                            Kreis.transform.localScale = new Vector3(0.03f * quot, 0.03f * quot, 1f);
                            Kreis.transform.rotation = Quaternion.Euler(hit.collider.gameObject.transform.rotation.x, hit.collider.gameObject.transform.rotation.y,
                             hit.collider.gameObject.transform.rotation.z);


                            //Wenn das object vor dir ist, ist es nicht hinter dir
                            if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
                            if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }
                            if (objectLinks.Contains(gObject.gameObject.name) == true)
                            {
                                objectLinks.Remove(gObject.gameObject.name);
                            }
                            if (objectRechts.Contains(gObject.gameObject.name) == true)
                            {
                                objectRechts.Remove(gObject.gameObject.name);
                            }
                            //notMVP
                            //
                            //if (Physics.Raycast(KameraHalt.transform.position,Vector3.LerpUnclamped(KameraHalt.transform.forward,direction,0.015f), out hit_arrow, Mathf.Infinity))
                            //{
                            // float distancePoints=Vector3.Distance(hit_arrow.point, hit.point) ;
                            // if (distancePoints > 0.5f)
                            // {Pfeil.SetActive(true);
                            //     Debug.Log(distancePoints);


                            //     Pfeil = Kreis.transform.Find("PfeilFuerEvent(Clone)").gameObject;
                            //     float x = gObject.gameObject.transform.localScale.x;
                            //     float y = gObject.gameObject.transform.localScale.y;
                            //     float z = gObject.gameObject.transform.localScale.z;
                            //     //Pfeil.transform.localScale =new Vector3(Pfeil.transform.localScale.x/x+0.1f,Pfeil.transform.localScale.y/y+0.1f,Pfeil.transform.localScale.z/z+0.1f);

                            //     float step = speed * Time.deltaTime; // calculate distance to move
                            //     Pfeil.transform.position = Vector3.MoveTowards(hit_arrow.point, hit.point, step);

                            //     Pfeil.transform.forward = hit.point - hit_arrow.point;
                            // }else{
                            // Pfeil.SetActive(false);
                            // }





                            break;
                        case "linkeSeite":
                            gObject.transform.GetChild(0).gameObject.SetActive(false);

                            if (objectLinks.Contains(gObject.gameObject.name) == false)
                            {
                                objectLinks.Add(gObject.gameObject.name);
                            }
                            if (objectLinks.Count > 0)
                            {
                                linkeSeite.SetActive(true);
                            }
                            break;
                        case "rechteSeite":
                            gObject.transform.GetChild(0).gameObject.SetActive(false);
                            //Debug.Log("Rechts ist was vor der Liste");
                            if (objectRechts.Contains(gObject.gameObject.name) == false)
                            {
                                objectRechts.Add(gObject.gameObject.name);
                                //Debug.Log(objectRechts+ "   neues Object rechts"+ gObject.gameObject.name);
                            }
                            if (objectRechts.Count > 0)
                            {
                                rechteSeite.SetActive(true);
                                //Debug.Log("Rechts ist was");
                            }

                            break;
                        default:

                            if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
                            if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }

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
                Debug.DrawRay(KameraHalt.transform.position, KameraHalt.transform.forward * hit.distance * 20, Color.yellow);
                Debug.DrawRay(KameraHalt.transform.position, direction * hit.distance * 20, Color.green);
                Debug.DrawRay(KameraHalt.transform.position, Vector3.LerpUnclamped(KameraHalt.transform.forward, direction, 0.015f) * hit.distance * 20, Color.red);
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
            Weather.text = "Westbrück, CH \n 22°C";


        }
        else
        {
        }
    }

}