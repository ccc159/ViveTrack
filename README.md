# ViveTrack
ViveTrack is a **grasshopper plugin** that allows you to track your **HTC Vive** devices [6DoF](https://en.wikipedia.org/wiki/Six_degrees_of_freedom?oldformat=true), for instance your HMD, controllers, trackers and even customized trackers. It is vastly simplified for specially architecture students and you can basiclly plug&use without complicated setup.


### Usage
Currently ViveTrack is only used for tracking 6DoF positions of Vive devices, which can be used for Indoor Positioning, for instance tracking your DIY robots, drones, users hands and other stuff that you want to get position into Rhino/Grasshopper.

### Advantages
* High Precision : 2 lighthouses detected 1mm discrepency; 1 lighthouse detected 1.6mm discrepency (NOT PROVED)
* High Frequency : ca.250Hz
* High Range: 5m x 5m(Official), 8m x 8m(tested online)
* High Freedom of Tracking: Can be tracked in some extrem positions which normal tracking system are difficult to track
### Disadvantages
* Hardware : You need at least 1 lighthouse and 1 tracker.
* Setup: You need to setup your tracking devices before using, although it's amazingly simple.

### Prerequisites

There are a few things you need to install and setup before using ViveTrack:

 * [optional]**.Net Framework 4.6** (This usually comes with Windows 10. You can check from Apps&Features in windows 10 if it's installed)
 * **Install SteamVR and Setup Vive**: Please follow the official [HTC Vive Pre: Installation Guide](https://support.steampowered.com/kb_article.php?ref=2001-UXCM-4439) to setup your vive. Tow lighthouses are recommended to be fixed on the wall to avoid errors due to lighthouse vibration.
 * [optional] If you don't have HMD or don't want use HMD in your tracking system, you have to disable your "requireHmd" setting in your SteamVR. **1.** update your SteamVR to **Beta** Version.