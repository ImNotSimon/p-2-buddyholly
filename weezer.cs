using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using BepInEx;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;


namespace BuddyHolly
{
    [BepInPlugin("ImNotSimon.BuddyHollyP-2", "Buddy Holly for P-2", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static Harmony harmony;

        internal static AssetBundle BuddyHollyAssetBundle;

        private void Awake()
        {
            Debug.Log("weezer room starting");

            //load the asset bundle
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "buddyholly";
            {
                BuddyHollyAssetBundle = AssetBundle.LoadFromFile(Path.Combine(ModPath(), resourceName));
            }

            //start harmonylib to swap assets
            harmony = new Harmony("imnotsimon.BuddyHolly");
            harmony.PatchAll();
        }

        public static string ModPath()
        {
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar));
        }


        //use map info to inject data
        [HarmonyPatch(typeof(StockMapInfo), "Awake")]
        internal class Patch01
        {
            static void Postfix(StockMapInfo __instance)
            {
                //try to find dialog in scene and replace it
                foreach (var source in Resources.FindObjectsOfTypeAll<AudioSource>())
                {
                    if (source.clip)
                    {
                        if (source.clip.GetName() == "Deep Drone 5B")
                        {
                            Debug.Log("Replacing cerb room music");
                            source.clip = BuddyHollyAssetBundle.LoadAsset<AudioClip>("buddyholly.wav");
                        }
                    }

                }
            }
     }
  }
}
