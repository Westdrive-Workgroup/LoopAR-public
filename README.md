# Westdrive LoopAR Private Repository
![Westdrive LoopAR Logo](https://raw.githubusercontent.com/farbod69/Westdrive-LoopAR/Workflow/Westworld_LoopAR_Logo.svg)

![Westdrive Version](https://img.shields.io/badge/Version-1.0.Alpha-Yellow.svg)
![Main Branch](https://img.shields.io/badge/Stable%20Branch-Master-brightgreen.svg)

## Introduction
### Important Notice:
For an easier use we have devided the project into two seperate repositories, one containing 3d assets and models called Westdrive Assets Foundation and the other
for functionalities of Westdrive which is called Westdrive Core. One can use these two repositories combined or separetely to create their own simulation outside the scope of 
senarios existing in this version of Westdrive. 

you can find the [Westdrive Assets Foundation](https://gitlab.com/farbod69/westdrive-asset-foundation) here : https://gitlab.com/farbod69/westdrive-asset-foundation

## Hardware Requirements

### Minimum Requirements
*  Intel Core i7 7th Generation
*  16 Gb RAM
*  Geforce GTX 1070Ti
*  Operating System: Windows 10 home 

### Sugessted Requirements
*  Intel Core i7/i9 8th Generation or newer
*  16 Gb RAM
*  Geforce GTX 1080Ti or better
*  Operating System: Windows 10 home 

---
Project Westdrive has been developed and tested on following Hardwares:

System 1:
![CPU](https://img.shields.io/badge/CPU%3A-CPU--Intel%20Xeon%20W--2133%20%40%203.60%20GHz-green.svg)
![RAM](https://img.shields.io/badge/RAM%3A-16%2C0%20GB-green.svg)
![Graphics Card](https://img.shields.io/badge/GPU%3A-Nvidia%20Geforce%20RTX%202080%20Ti-green.svg)
![OS](https://img.shields.io/badge/OS%3A-Microsoft%20Windows%2010%20Pro-green.svg)
![Performance Quality](https://img.shields.io/badge/Performance%3A-Good-brightgreen.svg)

---
System 2:
![CPU](https://img.shields.io/badge/CPU-Intel%20Xeon%20E5--1607%20v4%20%40%203.10GHz-green.svg)
![RAM](https://img.shields.io/badge/RAM%3A-32%2C0%20GB-green.svg)
![Graphics Card](https://img.shields.io/badge/GPU%3A-Nvidia%20Geforce%20GTX%201080%20Ti-green.svg)
![OS](https://img.shields.io/badge/OS%3A-Microsoft%20Windows%2010%20Pro-green.svg)
![Performance Quality](https://img.shields.io/badge/Performance%3A-acceptable-yellowgreen.svg)

## Software requirements for VR version

![Unity Version](https://img.shields.io/badge/Unity%20version%3A-2019.3.0f3-blue.svg)
![dot net compatibiliyy](https://img.shields.io/badge/.Net%20API%20Level%3A-2.xx-blue.svg)
![SteamVR Plugin](https://img.shields.io/badge/SteamVR%20Plugin%20Version%3A-2.2.0-green.svg)
![TobiiXR Plugin](https://img.shields.io/badge/TobiiXR%20Plugin%20Version%3A-2.2.0-green.svg)
![Sranipal Plugin](https://img.shields.io/badge/SRanipal%20Plugin%20Version%3A-2.2.0-green.svg)
![Render Pipeline](https://img.shields.io/badge/Render%20Pipeline%3A-Standard%20Render%20Pipeline-yello.svg)

## Software requirements for non-VR version

![Unity Version](https://img.shields.io/badge/Unity%20version%3A-2019.3.0f3-blue.svg)
![dot net compatibility](https://img.shields.io/badge/.Net%20API%20Level%3A-2.xx-blue.svg)
![Render Pipeline](https://img.shields.io/badge/Render%20Pipeline%3A-Standard%20Render%20Pipeline-yello.svg)

## Testing Releases:
### VR Version:
- You will need to be in possessions of HTC Vive Pro Eye
- You need SteamVR and Tobii XR installed on the system
> if you have never used Tobii Eyetracker inside HTC Vive Pro Eye before you have to first accept their term of use and enable the eye tacker from StreamVR main environment before running the LoopAR Project.

### None VR Version:
-You just need the executable(.exe) of the project. 

## remarks on the project
- the project has been tested on Nvidia 2080 Ti as well as Nvidia GTX 1070 yielding a performance stable around 60 FPS

Note: 
The executable release is meant to be a showcase of the current LoopAR Alpha capabilities. You can find a video of it [here](https://drive.google.com/file/d/1ZyuosK4ig_ErZd1YLh2ZjA08xLz1orkJ/view?usp=sharing). Westdrive is meant to be customized and built for your specific needs. So we recommend you to download the code and build it yourself after implementing your own scenarios.

Note:
We still consider LoopAR in alpha phase, This is currently under the status of MVP (Minimum Viable Product) meaning it only implements the main functionalities of LoopAR. There will be many features that will be added during the months to come. 
We will always push the stable changes to the public LoopAR repository.

## Builded version
-you can download the standalone VR version [here](https://drive.google.com/file/d/1bBagKateS1WYV0HRvFxZbwWId148Rayk/view?usp=sharing)

-you can download the standalone non VR version [here](https://drive.google.com/file/d/1bBagKateS1WYV0HRvFxZbwWId148Rayk/view?usp=sharing)

## Build prepration
in the following section, it is explained how you can clone and import the project for build and developement inside Unity environments.
### Step one: Preparing Unity
please [download](https://unity3d.com/unity/beta/2019.3.0f3) the corresponding version Unity and install it on your machine. It is highly recommended to 
download and install Unity through [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe?_ga=2.187579435.2096600450.1550663193-640931691.1544444769).
It is easier to manage various installations of Unity when using Unity Hub. You can find more information on Unity Hub [here](https://docs.unity3d.com/Manual/GettingStartedUnityHub.html).

-Download and install relevant requirement depending on VR or Non VR version of the project

#### notice:

> Please always make sure you are using the unity version mentioned here to ensure correct build and functionality of Westdrive. 

### Step two: cloning the project
First make sure you have installed a git client on you machine. If you need one you can find many of them online. Alternatively you can just [download](https://desktop.github.com/) and install the official Github client for Microsot Windows. 
> if you just want to use Project Westdrive you can alternatively dowload the project as an archive file (see below), however to contribute to the project or make your own forks you will need a Github [account](https://github.com/join?ref_cta=Sign+up&ref_loc=header+logged+out&ref_page=%2F&source=header-home).

## Tutorials
To help you download, prepare and use the Westdrive project we created a few tutorial videos in which you get lead through the beginning steps.

### How to [Unity](https://linkfollows.com) the World
### How to [Clone](https://linkfollows.com) the Reaper
### How to use the [Environment](https://drive.google.com/file/d/1GVVI5w5Kcz7SV_80ShdorcNvxPKGKMda/view?usp=sharing) and whats already there
### How to build a murder Scene [(Critical Traffic Event)](https://drive.google.com/file/d/1EoLdt2e097qRCqoIhpoXDDeCZnLinDkC/view?usp=sharing)

## Third party assets
in the following secion all used assets with their links in the unity asset store, with their corresponding functionality are listed.

## Important note:

> After our communication with creators of some payed third party assets, they have given us permission to share those assets for **Academic purposes** only in limited numbers. If you want to have access to those assets please write us an e-mail with the name of your institution and project overview so we can add you to LoopAR private repository. The following 3rd party assets has been removed from our public repository. 
- EasyRoads Version 3.0 

#### note:

> Assets are separeted in tags with paid and free, and also essential or optional. 

#### note:

> If you plan to use the paid assets in other projects, please make sure you purchase them for your organization in the Unity asset store 

#### note:

> optional assets are usualy 3d assets that can be replaced by your own designs or other 3d models. 

### List of assets
| Asset Name | Link on Asset Store | Description | Paid / Free | Essentail / Optional |
| ------ | ------ | ------ | ------ | ------ |
| + SteamVR 1.1.2.0| [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647) | main api to use HTV Vive/ Vive Pro HMDs in Unity3D | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg) |
| + TobiiXR 1.8.0| [TobiiXR SDK](https://developer.tobii.com/tobii-xr-unity-sdk-1-8-0-download-page/) | Main api to use HTV Vive Pro Eye eye tacker in Unity3D | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg) |
| + SRanipal| [SRanipal SDK](https://developer.tobii.com/tobii-xr-unity-sdk-1-8-0-download-page/) | Main SDK for HTC vive pro eye in conjunction to TobiiXR in order to use eyetracking in UnityED| free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg) |
| * SS07 | [Unlock super sport car #07 ](https://assetstore.unity.com/packages/3d/vehicles/land/unlock-super-sports-car-07-109989) | one of the car assets used in the project | paid | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| * EasyRoads3D v3 pro | [EasyRoads3D v3 pro](https://assetstore.unity.com/packages/tools/terrain/easyroads3d-pro-v3-469) |Create unique road networks directly in Unity with both built-in customizable dynamic crossing prefabs and custom crossing prefabs based on your own imported models | paid | ![note:](https://img.shields.io/badge/note-Essential-red.svg) |
| Conifers [BOTD] | [Conifers](https://assetstore.unity.com/packages/3d/vegetation/trees/conifers-botd-142076) | his package contains 4 conifers derived from Unity's Book of the Dead Demo – reworked, optimized and imported using the Custom Tree Importer to make them compatible with the "legacy" rendering pipeline. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Standard Assets | [Standard Assets](https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2017-3-32351) | This collection of assets, scripts, and example scenes can be used to kickstart your Unity learning or be used as the basis for your own projects. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Grass Flowers Pack Free | [Grass Flowers Pack Free](https://assetstore.unity.com/packages/2d/textures-materials/grass-flowers-pack-free-138810) | This pack contains 12 grass and flower textures(Resolution 1024x1024,Alpha Channel). | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Rocky Hills Environment - light pack | [Rocky Hills Environment - light pack](https://assetstore.unity.com/packages/3d/environments/landscapes/rocky-hills-environment-light-pack-89939) | High quality, low poly model pack, good for any kind of platform, mobile friendly and very aesthetic for higher quality game development. Includes models from a early project. Each model from the Legacy project comes with 2 Lod levels.| free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Terrain Tools Sample Asset Pack | [Terrain Tools Sample Asset Pack](https://assetstore.unity.com/packages/2d/textures-materials/terrain-tools-sample-asset-pack-145808) | The Terrain Tools Sample Asset Pack contains a collection of Assets to jump-start development for users interested in utilizing Unity’s growing Terrain system. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Farm Machinery | [Farm Machinery](https://assetstore.unity.com/packages/3d/vehicles/land/farm-machinery-low-poly-tractor-and-planter-94533) | This is a low poly style model pack of a farm tractor and a planter. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Wooden Box | [Wooden Box](https://assetstore.unity.com/packages/3d/props/wooden-box-670) | This package contains a wooden box in four states: One intact and three destroyed versions. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Industrial Set - Scaffolding | [Industrial Set - Scaffolding](https://sketchfab.com/3d-models/industrial-set-scaffolding-04239b73e22b4546ad2bd7cb5d47166a) | game ready scaffold set, no LODs, PBR Textures. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Wood panels | [Wood panels](https://sketchfab.com/3d-models/wood-panels-7b2a9828a34f48bf80f8c5cf8e7dae80) | 3 wood panels Assets for Cities: Skylines. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Starbucks Coffee | [Starbucks Coffee](https://sketchfab.com/3d-models/starbucks-coffee-a2081e0943c74ec49920c3180e20f349) | Starbucks Coffee Asset for Cities: Skylines. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Boulangerie de l’Opéra | [Boulangerie de l’Opéra](https://sketchfab.com/3d-models/boulangerie-de-lopera-55c29afb945147699633d1b78c18ffec) | A typical french bakery Asset for Cities: Skylines. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Pile of Planks | [Pile of Planks ](https://sketchfab.com/3d-models/pile-of-planks-freegameready-025e9f1cb3bd4256ba7570aa991be29b) | Just an optimised low poly pile of planks with PBR textures.  | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Pile of Planks | [Pile of Planks ](https://sketchfab.com/3d-models/pile-of-planks-freegameready-025e9f1cb3bd4256ba7570aa991be29b) | Just an optimised low poly pile of planks with PBR textures.  | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Picnic Tables | [Picnic Tables ](https://sketchfab.com/3d-models/picnic-tables-c962b28e2612421b9392d3ee403b9309) | Set of worn down Picnic tables. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| France - Cities: Skyline models | [France - Cities: Skyline models](https://sketchfab.com/Lost_Gecko/collections/france-cities-skylines-models) | French custom assets Lost Gecko created for the city-builder video game Cities: Skylines. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Hyundai Excavator | [Hyundai Excavator](https://sketchfab.com/3d-models/hyundai-excavator-a805e66f565a412682ac8ce45ab9ac3e) | An Excavator. Pictures taken with D5300 + 10mm Sigma + Parrot Anafi. Added LODs | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| a pile of clay | [a pile of clay](https://sketchfab.com/3d-models/a-pile-of-clay-c356e6679e8242659adee7f6c6304c37) | A good reference or gameobject to a computer game. Photogrammetry from 99 photos. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| a pile of sand | [a pile of sand](https://sketchfab.com/3d-models/a-pile-of-sand-107f6a1f6aa74449bcd32ef136311d86) | A good reference or gameobject to a computer game. Photogrammetry from 107 photos. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Microsoft Lumia 950 | [Microsoft Lumia 950 ](https://sketchfab.com/3d-models/microsoft-lumia-950-cdab940b566b42ada956bda77aa4948a) | Simple low poly model of the Microsoft Lumia 950, Windows 10 flagship. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Windmill | [Windmill](https://www.turbosquid.com/3d-models/free-windmill-video-games-3d-model/464415) | A low-poly Windmill. Textures are hand painted and the model is fully UV textured. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Observatory | [Observatory](https://www.turbosquid.com/FullPreview/Index.cfm/ID/437366) | An astronomical Observatory. | free | ![note:](https://img.shields.io/badge/note-Optional-green.svg)|
| Animated Stag Deer 3D Model | [Animated Stag Deer 3D Model](http://www.cadnav.com/3d-models/model-40791.html) | Highly detailed and rigged 3d model of a stag, with walk and attack animations. | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg)|
| HUD warning sign | [HUD warning sign](http://pngimg.com/download/87933) | Simple red triangle. | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg)|
| ** HUD sound Event | [HUD sound Event](https://www.youtube.com/watch?v=dV8S_2lwDkQ&list=PLx638JkBb2l-GbhsZpHNI-6NhXLjArMby&index=33) | Warning Sound. | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg)|
| ** HUD sound EventEnd | [HUD sound EventEnd ](https://www.soundjay.com/button/sounds/beep-09.mp3) | Normal beep. | free | ![note:](https://img.shields.io/badge/note-Essential-yellow.svg)|
### *

> This asset is removed for LoopAR public repository but still accessable through LoopAR private repository. 

### **

> These sounds has been used for testing the functionalities and will change in the next update. 

### + 
> Essential to work on both VR and non VR version of the project, please note that you can only download the latest version from the given links, however versions relavant to the project LoopAR are included in the repository

### Honorary Mentions
> We want to mention Penny de byl for her outstanding videos in Udemy which helped us alot in simulating physical cars in Unity3d
## Avatars and animations:
Avaratrs and animations in westdrive are created by us using Adobe Mixamo and Fuse cc. At the momenet westdrive is using avatars of our own creation using mentioned tools but due to their complex mesh anatomy we are replacing them with simpler low poly avatars created by us in blender soon. 

### acknowledgement to creators and team assistants

Our acknowledgement goes to the creators of all the free and paid assets mentioned above, Adobe, Unity Technologies and Blenders for their tools as well as following persons who helped us in creating and maintaining Westdrive
*  Phillip Spaniol - main graphic designer for our team, also worked on the city environment 
*  Johannes Maximilian Pingel - HUD functionalities and basics of the city environment
*  Lea Maria Kühne - worked on the sanity and realism of traffical event, also responsible for country road and main experiment scene for currect LoopAR version
*  Linus Tiemann - Main car drive functionalities, eye-tracker and input connection
*  Nora Maleki - AI functionalities of the cars and pedestrians
*  Lynn Keller - Mountain road environment, responsible for graphical sanity and quality of environments
*  Anke Haas - Autobahn environment 
*  Frederik Nienhaus - Test scene environment
*  Farbod Nosrat Nezami - technical supervisor of the project
*  Maximilian Alexander Wächter - Behavioral supervisor of the project
*  Prof. Dr. Peter König - main supervisor of the project
*  Prof. Dr. Gordon Pipa - second supervisor of the project
*  Stahlwerk Stiftung Georgsmarienhütte, University of Osnabrück and Deutsche Forschungsgemeinschaft for their financial support
> All members of the team work equaly on background research and refinment of the study
### current term of use
You are free to share, change and use Westdrive in whatever manner you like as long as you accept the following conditions:

-Westdrive LoopAR is an open source driving simulation for self driving cars, spatial navigation, embodeid cognition and similiar experiments. It is made available for scientists and anyone who is interested in research in an Virtual environement. Therefore any financial use of this tool is prohibited. 

-Assets presented here are mainly free assets from unity asset store which can be used in other projects, however if you plan to use assets that are paid please purchase them for your organization from the unity asset store. Developing team of Westdrive does not accept any responsibility regarding this matter and we are strictly against piracy. 

#### announcement:

> Currently Testdrive scene is not included in the build however it exist fully in the repo, we are polishing, and testing it in the following days and as soon as it passes the main quality checks we will include them in the build versions

## License
All Documentation content that resides under the doc/ directory of this repository is licensed under Creative Commons: [![License: CC BY-NC-SA 4.0](https://licensebuttons.net/l/by-nc-sa/4.0/80x15.png)](https://creativecommons.org/licenses/by-nc-sa/4.0/)

