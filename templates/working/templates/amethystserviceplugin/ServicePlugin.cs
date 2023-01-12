using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Numerics;
using Amethyst.Plugins.Contract;
using Microsoft.UI.Xaml.Controls;

// To learn more about Amethyst, plugins and the plugin API,
// see https://docs.k2vr.tech/ or https://github.com/KinectToVR

namespace __PluginNameSafe__;

// This is the main class of your plugin
// Metadata includes the 'Name' and 'Guid'
// You can also add a 'Publisher' and a 'Website'
[Export(typeof(IServiceEndpoint))]
[ExportMetadata("Name", "__PluginName__")]
[ExportMetadata("Guid", "__PluginGuid__")]
public class __PluginNameSafe__ : IServiceEndpoint
{
    // Import this interface to invoke AME methods
    [Import(typeof(IAmethystHost))] private IAmethystHost Host { get; set; }

#if (EnableSettings)
    // This is the root of your settings UI
    private Page InterfaceRoot { get; set; }

    // This is a sample text block to show how to use the localization system
    private TextBlock SettingTextBlock { get; set; }
#endif

    // Settings UI root / MUST BE OF TYPE Microsoft.UI.Xaml.Controls.Page
    // Return new() of your implemented Page, and that's basically it!
#if (EnableSettings)
    public object SettingsInterfaceRoot => InterfaceRoot;
#else
    public object SettingsInterfaceRoot => null;
#endif

    // To support settings daemon and register the layout root,
    // the device must properly report it first
    // -> will lead to showing an additional 'settings' button
    // Note: each device has to save its settings independently
    // and may use the K2AppData from the Paths' class
    // Tip: you can hide your device's settings by marking this as 'false',
    // and change it back to 'true' when you're ready
#if (EnableSettings)
    public bool IsSettingsDaemonSupported => true;
#else
    public bool IsSettingsDaemonSupported => false;
#endif

    // These will indicate the device's status [OK is (int)0]
    // Both should be updated either on call or as frequent as possible
    public int ServiceStatus { get; set; } = -1;

    // The link to launch when 'View Docs' is clicked while
    // this device is reporting a status error.
    // Supports custom protocols, e.g. "host://link"
    // [Note: launched via Launcher.LaunchUriAsync]
    public Uri ErrorDocsUri => null;

    // Device status string: to get your resources, use RequestLocalizedString
    public string ServiceStatusString => Host.RequestLocalizedString(
        ServiceStatus == 0 ? "/Statuses/Success" : "/Statuses/Failure");

    // Additional supported tracker types set
    // The mandatory ones are: waist, left foot, and right foot
    public SortedSet<TrackerType> AdditionalSupportedTrackerTypes =>
        new()
        {
            __SupportedTrackerTypes__
        };

    // Mark as true to tell the user that they need to restart
    // in case they want to add more trackers after spawning
    // This is the case with OpenVR, where settings need to be reloaded
#if (RestartOnChanges)
    public bool IsRestartOnChangesNeeded => true;
#else
    public bool IsRestartOnChangesNeeded => false;
#endif

    // Controller input actions, for calibration and others
    // Also provides support for flip/freeze quick toggling
    // Leaving this null will result in marking the
    // manual calibration and input actions support as [false]
#if (SupportInputActions)
    public InputActions ControllerInputActions { get; set; } = new()
    {
        CalibrationConfirmed = (_, _) => { },
        CalibrationModeChanged = (_, _) => { },
        SkeletonFlipToggled = (_, _) => { },
        TrackingFreezeToggled = (_, _) => { }
    };
#else
    public InputActions ControllerInputActions { get; set; } = null;
#endif

    // For AutoStartAmethyst: check if it's even possible
    // Mark as true to state that starting your app/service
    // can automatically start Amethyst at the same time
#if (CanAutoStartAmethyst)
    public bool CanAutoStartAmethyst => true;
#else
    public bool CanAutoStartAmethyst => false;
#endif

    // Check or set if starting the service should auto-start Amethyst
    // This is only available for a few actual cases, like OpenVR
    public bool AutoStartAmethyst { get => false; set { } }

    // Check or set if closing the service should auto-close Amethyst
    // This is only available for a few actual cases, like OpenVR
    public bool AutoCloseAmethyst { get => false; set { } }

    // Check if Amethyst is shown in the service dashboard or similar
    // This is only available for a few actual cases, like OpenVR
    // Leave as true if your service doesn't support this by default
    public bool IsAmethystVisible => true;

    // Check running system name, this is important for input
    // e.g. "Oculus" | "VIVE" | "Index" | "WMR" | ...
    public string TrackingSystemName => "__PluginName__";

    // Get the absolute pose of the HMD, calibrated against the play space
    // Return null if unknown to the service or unavailable
    // You'll need to provide this to support automatic calibration
    public (Vector3 Position, Quaternion Orientation)? HeadsetPose => null;

    // Implement if your service supports custom toasts
    // Services like OpenVR can show internal toasts
    public void DisplayToast((string Title, string Text) message)
    {
        Host?.Log($"{message.Title} {message.Text} by __PluginName__!");
    }

    // Find an already-existing tracker and get its pose
    // For no results found return null, also check if it's from amethyst
    public TrackerBase GetTrackerPose(string contains, bool canBeFromAmethyst = true) => null;

    // This is called right before the pose compose
    public void Heartbeat()
    {
        // ignored
    }

    // This initializes/connects to the service
    public int Initialize()
    {
        Host?.Log($"Tried to initialize __PluginName__!");

        ServiceStatus = 0;
        return 0; // S_OK
    }

    // This is called after the app loads the plugin
    public void OnLoad()
    {
        Host?.Log($"Loading __PluginName__ now!");
#if (EnableSettings)

        SettingTextBlock = new TextBlock
        {
            Text = Host?.RequestLocalizedString("/Elements/SampleText")
        };

        InterfaceRoot = new Page
        {
            Content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children = { SettingTextBlock }
            }
        };
#endif
    }

    // This is called when the service is closed
    public void Shutdown()
    {
        Host?.Log($"Tried to shut down __PluginName__!");

        ServiceStatus = -1;
    }

    // Request a restart of the tracking endpoint service (return success?)
    public bool? RequestServiceRestart(string reason, bool wantReply = false) => null;

    // Set tracker states, add/spawn if not present yet
    // Default to the serial, update the role if needed
    // Returns the same vector with paired success property (or null)
    public async Task<IEnumerable<(TrackerBase Tracker, bool Success)>> SetTrackerStates(
        IEnumerable<TrackerBase> trackerBases, bool wantReply = true)
    {
        // Imitiate some work being done, or skip everything when not checked
        return wantReply ? trackerBases.Select(x => (x, true)) : null;
    }

    // Update tracker positions and physics components
    // Check physics against null, they're passed as optional
    // Returns the same vector with paired success property (or null)
    public async Task<IEnumerable<(TrackerBase Tracker, bool Success)>> UpdateTrackerPoses(
        IEnumerable<TrackerBase> trackerBases, bool wantReply = true)
    {
        // Imitiate some work being done, or skip everything when not checked
        return wantReply ? trackerBases.Select(x => (x, true)) : null;
    }

    // Check connection: status, serialized status, combined ping time
    public async Task<(int Status, string StatusMessage, long PingTime)> TestConnection()
    {
        // Assume everything is fine
        return (0, "OK", 0L);
    }
}