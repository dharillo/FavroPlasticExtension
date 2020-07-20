# Favro - Plastic SCM integration

[![Build Status](https://travis-ci.org/dharillo/FavroPlasticExtension.svg?branch=master)](https://travis-ci.org/dharillo/FavroPlasticExtension)

## Important

This project is still under development. Some functionalities may not work as expected.

## Installation

There are two options to install the plugin in your local client. You can build the plugin library from the source code or you can download an already built library from the [distribution](distribution) folder.

### Build from source

Prerequisites:

- [Visual Studio 2019 Community](https://visualstudio.microsoft.com/en/vs/)
- Git

Steps:

1. Clone this repository into your local computer.
2. Open the solution *FavroPlasticExtension.sln*
3. Select the *Release* configuration
4. Build the *FavroPlasticExtension* project
5. Copy the *FavroPlasticExtension.dll* library to your Plastic SCM extensions folder. Usually located at *C:\Program Files\PlasticSCM5\client\extensions*. There you will need to create a new folder for this extension library named *favro* and copy the dll inside.
6. Modify the customextensions.conf file adding the following line at the end ```Favro=extensions/favro/FavroPlasticExtension.dll```

If you want to debug the plugin:

1. Clone this repository into your local computer.
2. Open the solution *FavroPlasticExtension.sln* as Administrator
3. Select the *Debug* configuration
4. Select the project *FavroPlasticExtension* as startup project
5. Launch the Visual Studio debugger (F5), if your Plastic install directory is the default it will work without problems

## Plugin configuration

Once installed, we will be able to configure the plugin using the *Preferences* window of the *Plastic SCM* client. Once there, go to the *Issue Tracker* section. An option to select Favro as issue tracker will be available.

The parameters that you will need to configure are:

- **User**: the email used to log into *Favro*
- **Password**: Your Favro password
- **Organization**: The hash identifier of your organization in the Favro page. You can know this ID easly by looking at the Favro URL once logged in. The URL will be something similar to: ```https://favro.com/organization/<organization_id>/<board_id>```. The *organization_id* part is the one needed.

## References

The code of this project was developed using the following documentation

- [Plastic SCM extensions guide](https://www.plasticscm.com/documentation/extensions/plastic-scm-version-control-task-and-issue-tracking-guide#WritingPlasticSCMcustomextensions)
- [Favro API Reference](https://favro.com/developer/)
- [Liquid Planner Plastic SCM Extension](https://github.com/dharillo/LiquidPlannerPlasticExtension)
