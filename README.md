# Sample of Windows Service by Design Automation API
(Formely AutoCAD I/O)

[![.net](https://img.shields.io/badge/.net-4.5-green.svg)](http://www.microsoft.com/en-us/download/details.aspx?id=30653)
[![odata](https://img.shields.io/badge/odata-4.0-yellow.svg)](http://www.odata.org/documentation/)
[![ver](https://img.shields.io/badge/Design%20Automation%20API-2.0-blue.svg)](https://developer.autodesk.com/api/autocadio/v2/)
[![visual studio](https://img.shields.io/badge/visual%20studio-2015%2F2017-yellowgreen.svg)](https://www.visualstudio.com/)
[![License](http://img.shields.io/:license-mit-red.svg)](http://opensource.org/licenses/MIT)

## Description
Windows service sample to plot a drawing placed in a folder to PDF using Design Automation API
 
## Thumbnail
![thumbnail](/thumbnail.png) 

## Industry Background
* an utility to convert AutoCAD drawing to PDF automatically on the background. 

## Setup

### Dependencies 
* Download and install [Visual Studio](https://visualstudio.microsoft.com/downloads/). In the latest test, Visual Studio version is 2017.
* Get the auxiliary library project [design.automation-.net-library](https://github.com/Autodesk-Forge/design.automation-.net-library)

### Prerequisites
1. **Forge Account**: Learn how to create a Forge Account, activate subscription and create an app at [this tutorial](http://learnforge.autodesk.io/#/account/). Make sure to select the service **Design Automation**.
2. Make a note with the credentials (client id and client secret) of the app. 
3. as mentioned in **Dependencies**, ensure to build the auxiliary library project and get the binary dll.


## Running locally  

1. Open the project. Restore the packages of the project by [NuGet](https://www.nuget.org/. The simplest way is
  * VS2012: Projects tab >> Enable NuGet Package Restore. Then right click the project>>"Manage NuGet Packages for Solution" >> "Restore" (top right of dialog)
  * VS2013/VS2015/2017:  right click the project>>"Manage NuGet Packages for Solution" >> "Restore" (top right of dialog)
2. Add other missing references and the library of[design.automation-.net-library](https://github.com/Autodesk-Forge/design.automation-.net-library)
3. In the project settings, provide the following details:
 * Path to a local folder in your system that contains AutoCAD drawings.
 * Design Automation Client Id
 * Design Automation Client Secret
 * Bucket name in your AWS S3 Storage
  ![Picture](./assets/1.PNG)
4. Open “App.Config” file and provide AWS credentials. This will allow the sample project to access S3 storage in your AWS profile.
 ![Picture](./assets/2.png)
5. Build the sample project
6. Install the windows service using “installutil” in the command line of Visual Studio. To do this open Visual studio command prompt and run : 
     installutil < path to PlotToPDFService.exe >
7. Start the windows service. To do this from the Run windows, type services.msc and look for Plot2PDFService and start it.
  ![thumbnail](./assets/3.png)
8. You may need to switch to "[Log on local system account" of the service if you hit an error of "Access Denied".
  ![Picture](./assets/winservlogon.png)
9. Copy any drawing to the watched folder (C:\Temp by default). After some time, a PDF of the same name should be available in the watched folder.
  ![Picture](./assets/4.PNG)

## Known Issues
* as of writing, Design Automation of Forge is released with version 2. Odata is used with .NET project. In futher version, OData might not be used. 


## Further Reading 
* [Design Automation API help](https://forge.autodesk.com/en/docs/design-automation/v2/developers_guide/overview/)
* [ Intro to Design Automation API Video](https://www.youtube.com/watch?v=GWsJM344CJE&t=107s)
* [Create Windows Services in C#](https://dzone.com/articles/create-windows-services-in-c)

## License

These samples are licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.

## Written by 

Balaji Ramamoorthy 
updated by Xiaodong Liang