using UnityEngine;using HarmonyLib;using System.Collections.Generic;
[HarmonyPatch(typeof(Input))]
public class InputPatches{
 public static float VirtualH=0f;public static float VirtualV=0f;
 static Dictionary<string,KeyCode> keyMap=new Dictionary<string,KeyCode>();
 static Dictionary<string,bool> states=new Dictionary<string,bool>();
 public static void SetButton(string id,bool v){states[id]=v;}
 public static void RegisterKey(string id,KeyCode kc){keyMap[id]=kc;}
 public static void UnregisterKey(string id){keyMap.Remove(id);states.Remove(id);}
 public static void InitDefaults(){
  RegisterKey("A",KeyCode.Z);RegisterKey("B",KeyCode.X);RegisterKey("X",KeyCode.Space);
  RegisterKey("Y",KeyCode.LeftShift);RegisterKey("L1",KeyCode.E);RegisterKey("R1",KeyCode.Escape);RegisterKey("Start",KeyCode.M);
 }
 static bool IsDown(KeyCode kc){foreach(var kv in keyMap)if(kv.Value==kc&&states.ContainsKey(kv.Key)&&states[kv.Key])return true;return false;}
 [HarmonyPatch("GetAxis")][HarmonyPostfix]public static void GA(string n,ref float r){if(n=="Horizontal")r=VirtualH;else if(n=="Vertical")r=VirtualV;}
 [HarmonyPatch("GetAxisRaw")][HarmonyPostfix]public static void GAR(string n,ref float r){if(n=="Horizontal")r=VirtualH;else if(n=="Vertical")r=VirtualV;}
 [HarmonyPatch("GetKey")][HarmonyPostfix]public static void GK(KeyCode kc,ref bool r){if(IsDown(kc))r=true;}
 [HarmonyPatch("GetKeyDown")][HarmonyPostfix]public static void GKD(KeyCode kc,ref bool r){if(IsDown(kc))r=true;}
 [HarmonyPatch("GetButton")][HarmonyPostfix]public static void GB(string n,ref bool r){
  if(n=="Fire1"&&states.ContainsKey("A")&&states["A"])r=true;
  else if(n=="Fire2"&&states.ContainsKey("B")&&states["B"])r=true;
  else if(n=="Jump"&&states.ContainsKey("X")&&states["X"])r=true;
 }
}
