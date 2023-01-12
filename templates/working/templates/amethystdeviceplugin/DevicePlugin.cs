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
[Export(typeof(ITrackingDevice))]
[ExportMetadata("Name", "__PluginName__")]
[ExportMetadata("Guid", "__PluginGuid__")]
public class __PluginNameSafe__ : ITrackingDevice
{
    // Import this interface to invoke AME methods
    [Import(typeof(IAmethystHost))] private IAmethystHost Host { get; set; }

#if (EnableSettings)
    // This is the root of your settings UI
    private Page InterfaceRoot { get; set; }

    // This is a sample text block to show how to use the localization system
    private TextBlock SettingTextBlock { get; set; }
#endif

    // This will tell Amethyst to disable all position filters on joints managed by this plugin
#if (BlockFiltering)
    public bool IsPositionFilterBlockingEnabled => true;
#else
    public bool IsPositionFilterBlockingEnabled => false;
#endif

    // This will tell Amethyst not to auto-manage on joints managed by this plugin
    // Includes: velocity, acceleration, angular velocity, angular acceleration
#if (OverridePhysics)
    public bool IsPhysicsOverrideEnabled => true;
#else
    public bool IsPhysicsOverrideEnabled => false;
#endif

    // This will tell Amethyst not to auto-update this device
    // You should register some timer to update your device yourself if so
#if (SelfUpdate)
    public bool IsSelfUpdateEnabled => true;
#else
    public bool IsSelfUpdateEnabled => false;
#endif

    // Mark this as false ALSO if your device supports 360 tracking by itself
#if (SupportFlip)
    public bool IsFlipSupported => true;
#else
    public bool IsFlipSupported => false;
#endif

    // This will allow Amethyst to calculate rotations by itself, additionally
#if (SupportAppOri)
    public bool IsAppOrientationSupported => true;
#else
    public bool IsAppOrientationSupported => false;
#endif

    // Settings UI root / MUST BE OF TYPE Microsoft.UI.Xaml.Controls.Page
    // Return new() of your implemented Page, and that's basically it!
#if (EnableSettings)
    public object SettingsInterfaceRoot => InterfaceRoot;
#else
    public object SettingsInterfaceRoot => null;
#endif

    // Is the device connected/started?
    public bool IsInitialized { get; set; } = false;
    
    // This should be updated on every frame,
    // along with joint devices
    // -> will lead to global tracking loss notification
    // if set to false at runtime some-when
    public bool IsSkeletonTracked => true;

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
    public int DeviceStatus { get; set; } = -1;

    // The link to launch when 'View Docs' is clicked while
    // this device is reporting a status error.
    // Supports custom protocols, e.g. "host://link"
    // [Note: launched via Launcher.LaunchUriAsync]
    public Uri ErrorDocsUri => null;

    // Device status string: to get your resources, use RequestLocalizedString
    public string DeviceStatusString => Host.RequestLocalizedString(
        IsInitialized ? "/Statuses/Success" : "/Statuses/Failure");

    // Joints' list / you need to (should) update at every update() call
    // Each must have its own role or _Manual to force user's manual set
    // Adding and removing trackers will cause an automatic UI refresh
    // note: only when the device HAS BEEN initialized WASN'T shut down
    public ObservableCollection<TrackedJoint> TrackedJoints { get; } = new()
    {
        new TrackedJoint
        {
            Name = TrackedJointType.JointManual.ToString(),
            Role = TrackedJointType.JointManual
        }
    };

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

    // This initializes/connects the device
    public void Initialize()
    {
        Host?.Log($"Tried to initialize __PluginName__!");

        IsInitialized = true;
        DeviceStatus = 0;
    }

    // This is called when the device is closed
    public void Shutdown()
    {
        Host?.Log($"Tried to shut down __PluginName__!");

        IsInitialized = false;
        DeviceStatus = -1;
    }

    // This is called to update the device (each loop)
    public void Update()
    {
        // Update all prepended joints with the pose of the HMD
        // Or if not available for some reason, .Zero and .Identity
        TrackedJoints.ToList().ForEach(x =>
        {
            x.TrackingState = TrackedJointState.StateTracked;
            x.Position = Host?.HmdPose.Position ?? Vector3.Zero;
            x.Orientation = Host?.HmdPose.Orientation ?? Quaternion.Identity;
        });
    }

    // Signal the joint eg psm_id0 that it's been selected
    public void SignalJoint(int jointId)
    {
        Host?.Log($"Tried to signal joint with ID: {jointId}!");
    }
}