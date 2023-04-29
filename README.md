# Amethyst.Plugins.Templates

Plugin templates for [Amethyst.NET](https://github.com/KinectToVR/Amethyst) implementing the [Amethyst Plugin Contract](https://github.com/KinectToVR/Amethyst/tree/main/Amethyst.Plugins.Contract).  
Install [dotnet 7](https://dotnet.microsoft.com/) and run `dotnet new install Amethyst.Plugins.Templates::1.2.1`

## Usage

You can either use the templates directly from the dotnet console:
- Tracking provider: `dotnet new amethystdeviceplugin [--Parameters...]`
- Service endpoint: `dotnet new amethystserviceplugin [--Parameters...]`

or directly from the Visual Studio (2022) 'New Project' dialog.  
(The latter is self-explanatory and thus more recommended)

You can find the plugin documentation inside the samples, inside the [contract source](https://github.com/KinectToVR/Amethyst/tree/main/Amethyst.Plugins.Contract),  
and on [our official website](https://docs.k2vr.tech/). (Everything's still in preview tho, please keep that in mind)

## License

Both the plugin contract and samples are published under [MIT](LICENSE.md) (Amethyst is GPLv3)  
to make plugin development more open and accessible to most vendors/third party.