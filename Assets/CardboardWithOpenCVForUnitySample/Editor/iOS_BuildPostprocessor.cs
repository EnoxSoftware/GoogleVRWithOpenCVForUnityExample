#if UNITY_5 && UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

namespace CardboardWithOpenCVForUnitySample
{
    public class iOS_BuildPostprocessor : MonoBehaviour
    {

        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {

                string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

                PBXProject proj = new PBXProject();
                proj.ReadFromString(System.IO.File.ReadAllText(projPath));

                string target = proj.TargetGuidByName("Unity-iPhone");

                // Add our framework directory to the framework include path
                proj.AddFrameworkToProject(target, "Security.framework", false);

                //set ENABLE_BITCODE
                proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");


                File.WriteAllText(projPath, proj.WriteToString());
            }
        }
    }
}
#endif