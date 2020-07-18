## Context
- Targeting a Raspberry Pi that has:
  - .NET Core SDL 3.1 installed
  - Going to build the app on the target

### [1] Have UI access to the target
1. Run this web app on the target
1. Run the app in a browser on the target
1. Select the sample from the Index
2. Configure the sample's circuit
3. Create folder under teh share on the target, say **Sample**
4. Get a copy of the C# source in tha tfolder
  - There is the clipoard option
    - Copy the code
    - Create a new .cs file in the Sample folder and paste the contents
  - The Download option
    - Download the file, it gets saved in Downloads folder
    - Move it to the Sample folder
 5. Get a copy of the project file
   - The project file from the repository is not suitable as it assumes the presence of the whole repository source code.
   - As per 4. get the Use projFile, give it a ,csproj extension

 7. Build and run the app:   
 ```dot net run```
   


- Two