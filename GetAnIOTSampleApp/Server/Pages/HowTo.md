## [1] Deploy on the Target:
1. _You are already runnning this web app here, as the target._
2. Select the sample from the Index _(You have probably already done that as well.)_
  - Note the **project name** _(as projname, later)_, say, **BME280.Sample**_
3. Configure that sample's circuit
4. Create a folder, say **SampleApp**
5. Get a copy of the C# source into that folder  
  - There is the clipoard option:
    - Copy the code.
    - Create a new text file named as above (the _Project C# Filename_) in the _SampleApp_ folder and paste in the contents.
  - The Download option:
    - Download the file, it gets saved in Downloads folder.
    - Move it to the _SampleApp_ folder.
6. Get a copy of the project file into the Sample folder:  
_The Rep. Project File from the repository is not suitable as it assumes the presence of the whole repository source code._
  - _So_, as per 5. get the **Use This ProjFile** (either way). Make sure it is named as above (the _Project Filename_).  
  
   _**In a terminal on the target in the Sample folder:**_  

7. Get the System.Device.GPIO and IoT.Device.Bindings packages:
  - Copy and run each of the ```dotnet add package``` commands as below in a terminal on the target in the _Sample_ folder.  
  _These are the latest nightly builds_
8. Build and run the app:   
 ```dot net run```
9. Publish there app here:  
 ```dotnet publish```  
 You can now run the app using ```<projname>.exe``` 
