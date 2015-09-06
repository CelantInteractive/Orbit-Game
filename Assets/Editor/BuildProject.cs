using UnityEditor;
using System;
using System.Collections.Generic;

class BuildProject {
    static string[] SCENES = FindEnabledEditorScenes();

    static string VERSION_NUMBER = "0.1 Pre-Release";

    static string APP_NAME = "Orbit Game";
    static string TARGET_DIR = "C:\\Builds\\" + VERSION_NUMBER;

    [MenuItem ("Custom/CI/Build Mac OS X")]
    static void PerformMacOSXBuild ()
    {
                string target_dir = APP_NAME + ".app";
                GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXIntel,BuildOptions.None);
    }
		
	[MenuItem ("Custom/CI/Build Windows 32-bit")]
    static void PerformWin32Build ()
    {
                string target_dir = APP_NAME + ".exe";
                GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows,BuildOptions.None);
    }

	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
            if (res.Length > 0) {
                    throw new Exception("BuildPlayer failure: " + res);
            }
    }
}