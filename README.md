# Experimentation.FeatureSwitching
A simple nuget package that wraps the common functions provided by the Experimentation-API.

## Goal
This package was created with a simple goal in mind - that is wrap the common high frequency actions on the Experimentation-Api into an easy to use class (`FeatureSwitch`) that can be consumed and integrated with easily into all c# based projects. Whether they be personal or commercial or dev projects that do not make it into production. If you need to switch off code easily when faults or issues start to appear then this package is 1) out of 2) things you are going to need. The other being the Experimentation-Api set-up somewhere on your local developer pc or work network.

*To use this package you will need to host the package in an internal nuget or public package feed - i.e like Visual Studio Team Services.*

![Screenshot to Internal Package Feed](docs\teamservices.png)

## Tech Stack

The package was built using the following libraries:
- netstandard 2.0
- restsharp 

### Tech Notes 
Ofcourse adopting `netstandard 2.0` means that the lowest net framework supported by the package is `.net 4.6.1`. This is fine for all of my projects here at work but if you need to support a lower framework version then I suggest forking the project and customising it further to meet your needs. Or upgrade to `.net 4.6.1` and enjoy the benefits of `netstandard 2.0`.

## Design Notes
It is not the job of this package to provide a rich experience where by all of the administrative functions provided by the api are going to be catered for by this package. It's primary purposes is to make the systems integrators life easier by providing a nice interface to consume when calling common endpoints such as `IsSwitchEnabled()`. Of course there is nothing stopping from forking the package and extending it to wrap all of the endpoints provided by the Api. If that is what you desire then i suggest a different package for this i.e `Experimentation.FeatureSwitching.Admin` or perhaps a single page web app?
