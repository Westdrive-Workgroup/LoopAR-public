using UnityEngine;
using System.Collections;

public class TrafficLightsEU : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                      Traffic-Lights Script by Andre "AEG" B�rger of VIS-Games 2011
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    public bool traffic_light_active = true;

    public bool traffic_light_out_of_order_mode = false;

    public enum Crossway_Direction
    {
        DIRECTION1,
        DIRECTION2,
    };
    public Crossway_Direction crossway_direction = Crossway_Direction.DIRECTION1;
    
    public int direction_time = 5;

    public bool use_real_lights = true;
    public bool traffic_light_type_single = true;
    public float real_lights_range = 10.0f;
    public bool cast_shadows = true;
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    int traffic_lights_mode;
    float traffic_lights_counter;
    int traffic_lights_last_frame_mode;

    Renderer traffic_light_renderer;

    int direction1_light_red_mat_index;
    int direction1_light_yellow_mat_index;
    int direction1_light_green_mat_index;

    Light lightsource_red1;
    Light lightsource_yellow1;
    Light lightsource_green1;
    Light lightsource_red2;
    Light lightsource_yellow2;
    Light lightsource_green2;

//-------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------
bool[] traffic_lights_status_table =
{
    //---------------------------
    // normal mode

    //-- Mode 0: 
    true,   // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    false,  // direction 2 red
    false,  // direction 2 yellow
    true,   // direction 2 green

    //-- Mode 1: 
    true,   // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    false,  // direction 2 red
    true,   // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 2: 
    true,   // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    true,   // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 3: 
    true,   // direction 1 red
    true,   // direction 1 yellow
    false,  // direction 1 green

    true,   // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 4: 
    false,  // direction 1 red
    false,  // direction 1 yellow
    true,   // direction 1 green

    true,   // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 5: 
    false,  // direction 1 red
    true,   // direction 1 yellow
    false,  // direction 1 green

    true,   // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 6: 
    true,   // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    true,   // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode 7: 
    true,   // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    true,   // direction 2 red
    true,   // direction 2 yellow
    false,  // direction 2 green

    //---------------------------
    // out of order mode

    //-- Mode: 8 
    false,  // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    false,  // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

    //-- Mode: 9 
    false,  // direction 1 red
    true,   // direction 1 yellow
    false,  // direction 1 green

    false,  // direction 2 red
    true,   // direction 2 yellow
    false,  // direction 2 green

    //---------------------------
    // inactive

    //-- Mode: 10
    false,  // direction 1 red
    false,  // direction 1 yellow
    false,  // direction 1 green

    false,  // direction 2 red
    false,  // direction 2 yellow
    false,  // direction 2 green

};

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    traffic_light_renderer = gameObject.GetComponent<Renderer>();
    
    lightsource_red1    = transform.Find("LightRed1").GetComponent<Light>();
    lightsource_yellow1 = transform.Find("LightYellow1").GetComponent<Light>();
    lightsource_green1  = transform.Find("LightGreen1").GetComponent<Light>();

    if(traffic_light_type_single == false)
    {
        lightsource_red2    = transform.Find("LightRed2").GetComponent<Light>();
        lightsource_yellow2 = transform.Find("LightYellow2").GetComponent<Light>();
        lightsource_green2  = transform.Find("LightGreen2").GetComponent<Light>();
    }

    if(use_real_lights == false)
    {
        Destroy(lightsource_red1);
        Destroy(lightsource_yellow1);
        Destroy(lightsource_green1);
        
        if(traffic_light_type_single == false)
        {
            Destroy(lightsource_red2);
            Destroy(lightsource_yellow2);
            Destroy(lightsource_green2);
        }        
    }

    //-- global light parameters if real lights are enabled
    if(use_real_lights == true)
    {
        lightsource_red1.GetComponent<Light>().range    = real_lights_range;  
        lightsource_yellow1.GetComponent<Light>().range = real_lights_range;  
        lightsource_green1.GetComponent<Light>().range  = real_lights_range;  

        if(cast_shadows == false)
        {
            lightsource_red1.GetComponent<Light>().shadows    = LightShadows.None;
            lightsource_yellow1.GetComponent<Light>().shadows = LightShadows.None;
            lightsource_green1.GetComponent<Light>().shadows  = LightShadows.None;
        }

        if(traffic_light_type_single == false)
        {
            lightsource_red2.GetComponent<Light>().range    = real_lights_range;  
            lightsource_yellow2.GetComponent<Light>().range = real_lights_range;  
            lightsource_green2.GetComponent<Light>().range  = real_lights_range;  

            if(cast_shadows == false)
            {
                lightsource_red1.GetComponent<Light>().shadows    = LightShadows.None;
                lightsource_yellow1.GetComponent<Light>().shadows = LightShadows.None;
                lightsource_green1.GetComponent<Light>().shadows  = LightShadows.None;
            }
        }
    }

    


}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Start()
{
    if(traffic_light_out_of_order_mode == false)
        traffic_lights_mode = 0;
    else
        traffic_lights_mode = 8;

    if(traffic_light_active == false)
        traffic_lights_mode = 10;
    
    traffic_lights_counter = 0.0f;
    traffic_lights_last_frame_mode = -1;
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Update()
{
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 0: Direction 1: Red / Direction 2 Green     
    //
    if(traffic_lights_mode == 0)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f * direction_time)
        {
            traffic_lights_mode = 1;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 1: Direction 1: Red / Direction 2 Yellow    
    //
    if(traffic_lights_mode == 1)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 2;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 2: Direction 1: Red / Direction 2 Red    
    //
    if(traffic_lights_mode == 2)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 3;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 3: Direction 1: Red+Yellow / Direction 2 Red    
    //
    if(traffic_lights_mode == 3)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 4;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 4: Direction 1: Green / Direction 2 Red    
    //
    if(traffic_lights_mode == 4)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f * direction_time)
        {
            traffic_lights_mode = 5;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 5: Direction 1: Yellow / Direction 2 Red    
    //
    if(traffic_lights_mode == 5)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 6;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 6: Direction 1: Red / Direction 2 Red    
    //
    if(traffic_lights_mode == 6)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 7;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 7: Direction 1: Red / Direction 2 Red+Yellow   
    //
    if(traffic_lights_mode == 7)
    {
        traffic_lights_counter+= Time.deltaTime * 2.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 0;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 8: Direction 1: OFF / Direction 2 OFF
    //
    if(traffic_lights_mode == 8)
    {
        traffic_lights_counter+= Time.deltaTime * 3.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 9;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Mode 9: Direction 1: Yellow / Direction 2 Yellow
    //
    if(traffic_lights_mode == 9)
    {
        traffic_lights_counter+= Time.deltaTime * 3.0f;
        if(traffic_lights_counter >= 2.0f)
        {
            traffic_lights_mode = 8;
            traffic_lights_counter = 0.0f;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------

   
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------
    //
    // Set Material Brightness
    //
    if(traffic_lights_last_frame_mode != traffic_lights_mode)
    {
        //-- set direction 1 light red
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 0 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[2].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            {
                lightsource_red1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_red2.GetComponent<Light>().enabled = true;    
            }           
        }
        else
        { 
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[2].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            {
                lightsource_red1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_red2.GetComponent<Light>().enabled = false;    
            }           
        }

        //-- set direction 1 light yellow
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 1 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[3].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_yellow1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_yellow2.GetComponent<Light>().enabled = true;    
            }
        } 
        else
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[3].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_yellow1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_yellow2.GetComponent<Light>().enabled = false;    
            }
        }
        
        //-- set direction 1 light green
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 2 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[4].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_green1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_green2.GetComponent<Light>().enabled = true;    
            }
        }
        else
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[4].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_green1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_green2.GetComponent<Light>().enabled = false;    
            }
        }


        //-- set direction 2 light red
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 0 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[2].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_red1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_red2.GetComponent<Light>().enabled = true;    
            }
        }
        else
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[2].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_red1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_red2.GetComponent<Light>().enabled = false;    
            }
        }


        //-- set direction 2 light yellow
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 1 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[3].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_yellow1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_yellow2.GetComponent<Light>().enabled = true;    
            }
        }
        else
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[3].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_yellow1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_yellow2.GetComponent<Light>().enabled = false;    
            }
        }

        //-- set direction 2 light green
        if(traffic_lights_status_table[(traffic_lights_mode * 6) + 2 + ((int)(crossway_direction) * 3)] == true)
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[4].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_green1.GetComponent<Light>().enabled = true;    
                if(traffic_light_type_single == false)
                    lightsource_green2.GetComponent<Light>().enabled = true;    
            }
        }
        else
        {
            traffic_light_renderer.transform.GetComponent<Renderer>().materials[4].color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            if(use_real_lights == true)
            { 
                lightsource_green1.GetComponent<Light>().enabled = false;    
                if(traffic_light_type_single == false)
                    lightsource_green2.GetComponent<Light>().enabled = false;    
            }
        }
        traffic_lights_last_frame_mode = traffic_lights_mode;   
    }
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
