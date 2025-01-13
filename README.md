Steps to Download and Run the Project
1. Install Prerequisites
Ensure you have the following installed on your system:
.NET 8.0 SDK: Download and install from https://dotnet.microsoft.com/download.
Visual Studio 2022 (17.8 or later): Ensure you have the workloads for ASP.NET and web development and Blazor WebAssembly installed.
2. Clone the Repository
Open a terminal or command prompt and clone the repository:
bash
Copy code
git clone <repository-url>
Replace <repository-url> with the actual Git repository URL.
3. Navigate to the Project Directory
Move into the project directory:
bash
Copy code
cd Medica.Employment
4. Open the Solution
Open the Medica.Employment.sln file in Visual Studio.
5. Set Multiple Startup Projects
In Visual Studio, right-click the solution in Solution Explorer and select Properties.
Go to the Startup Project tab.
Choose Multiple startup projects.
Set both Medica.Employment.API and Medica.Employment.UI to Start.
6. Restore Dependencies
Open the terminal in Visual Studio (or your command prompt) and restore NuGet packages:
bash
Copy code
dotnet restore
7. Run the Projects
Press F5 or select Start in Visual Studio to launch both the API and UI.
The default URLs will typically be:
API: http://localhost:16060 
UI: http://localhost:26831/
