# Automation Framework

The purpose of this Automation Framework it is to make process of testing more faster. And provide visible reporting for
better understanding app quality.

## Structure:

**Web UIAutomation** framework contains two part:

1. **PlanA.Web.Core** - core part contains all needed infrastructure need for test run. Also this part provides
   additional methods for work with WedDriver and WebElements.

![](/Readme/PlanACore.png)

2. **<ProjectName>WebUITests** - project contains all ata needed for test run.

![WebTests.png](/Readme/WebTests.png)

- _Features folder_ - contains all feature files (test cases) separated by app tabs.
- _TestData folder_ - contains additional test data which can be used during test execution.
- _appsettings.json_ - configuration file. Used for changing base parameters for tests, such as app URL, login user,
  default waits and others.

![appsetings.png](/Readme/appsetings.png)

## Project set up:

1. Download and install .net 6.0 on your PC

   1.1. Download 6.0 .net version from [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
  
![dotnet.png](/Readme/dotnet.png)

   1.2. Install .net 6.0 on your PC

2. Update you Chrome browser
3. Clone repository from 

## Test Run:

1. Navigate to the <ProjectName>WebUIAutomation folder
   
![WebUIFolder.png](/Readme/WebUIFolder.png)

2.1. Open CMD for Windows

![cmd.png](/Readme/cmd.png)

2.2 Open terminal for Mac

![terminal.png](/Readme/terminal.jpg)

3. Execute command:

- To run all tests `dotnet test`
- To run specific test suit:
    - Run suits by specific category `dotnet test --filter Category=<Category_Name>`
    - Run suits that have a few selected
      categories `dotnet test --filter "Category=<Category_Name1> & Category=<Category_Name2>`
    - Run suits that have one of selected
      categories `dotnet test --filter "Category=<Category_Name1> | Category=<Category_Name2>`

![comand.png](/Readme/comand.png)

## Categories:

Categories there are the tag with which tests can be marked. Category can be sat to the whole file or for some specific
test.
Category use for configuring test run and also for set upping pre and post conditions for tests.

![Categories.png](/Readme/Categories.png)
![Category_test.png](/Readme/Category_test.png)

## Reporting:

To generate report after test execution:
1. Navigate to the **<ProjectName>WebUIAutomation\<ProjectName>WebUITests\bin\Debug\net6.0** folder

![net6_folder.png](/Readme/net6_folder.png)

2. Open CMD

![net6_cmd.png](/Readme/net6_cmd.png)

3. Execute command: `livingdoc test-assembly SolaireWebUITests.dll -t TestExecution.json`

Report will be generated in **<ProjectName>WebUIAutomation\<ProjectName>WebUITests\bin\Debug\net6.0** folder. Screenshots and videos will be stored in folders Screenshot and Video.

![folders.png](/Readme/folders.png)

![report.png](/Readme/report.png)

