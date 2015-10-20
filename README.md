workflow-windowsservice-autocad.io
==================================

Windows service sample to plot a drawing placed in a folder using AutoCAD IO V2

As this sample includes a reference to "library-dotnet-autocad.io", please build
that sample following the instruction provided in that sample. Here is the link :
https://github.com/Developer-Autodesk/library-dotnet-autocad.io

After you have built the library project, 
Open the PlotToPDF windows service sample project in Visual Studio 2012
Add reference to AutoCADIOUtil library
In the project settings, provide the following details:
-	AutoCAD IO Client Id
-	AutoCAD IO Client Secret
-	Bucket name in your AWS S3 Storage
 
![Picture](https://github.com/Developer-Autodesk/workflow-windowsservice-autocad.io/blob/master/assets/1.PNG)
 
Open “App.Config” file and provide AWS credentials. This will allow the sample project to access S3 storage in your AWS profile.
 
![Picture](https://github.com/Developer-Autodesk/workflow-windowsservice-autocad.io/blob/master/assets/2.png)
 
Build the sample project

Install the windows service using “installutil”. 
To do this open Visual studio command prompt and run : installutil <path to PlotToPDF.exe”

Start the windows service. To do this from the Run windows, type services.msc and look for Plot2PDFService and start it.

![Picture](https://github.com/Developer-Autodesk/workflow-windowsservice-autocad.io/blob/master/assets/3.png)

Copy any drawing to the watched folder (C:\Temp by default). 
After some time, a PDF of the same name should be available in the watched folder.

![Picture](https://github.com/Developer-Autodesk/workflow-windowsservice-autocad.io/blob/master/assets/4.PNG)
