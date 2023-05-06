
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
 
public class ATTPostProcessBuild
{
    // Set the IDFA request description:
    const string k_TrackingDescription = "私たちは、あなたが使用しているアプリに基づいて、あなたにとって最も興味深いアプリや製品の広告を表示しようとしています。";
 
    [PostProcessBuild(0)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
        if (buildTarget == BuildTarget.iOS) {
            AddPListValues(pathToXcode);
        }
    }
 
    // Implement a function to read and write values to the plist file:
    static void AddPListValues(string pathToXcode) {
        // Retrieve the plist file from the Xcode project directory:
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plistObj = new PlistDocument();
 
 
        // Read the values from the plist file:
        plistObj.ReadFromString(File.ReadAllText(plistPath));
 
        // Set values from the root object:
        PlistElementDict plistRoot = plistObj.root;
 
        // Set the description key-value in the plist:
        plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);
 
        // Save changes to the plist:
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }
}