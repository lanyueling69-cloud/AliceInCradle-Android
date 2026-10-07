using UnityEngine;using UnityEngine.UI;using System.IO;using HarmonyLib;
public class RuntimePatch:MonoBehaviour{
 static string R=Application.persistentDataPath;
 static bool hasPad=false;
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
 static void Init(){
  Directory.SetCurrentDirectory(R);
  Directory.CreateDirectory(R+"/SaveData");
  Directory.CreateDirectory(R+"/Config");
  Directory.CreateDirectory(R+"/AppData/LocalLow/NanameHacha/AliceInCradle");
  QualitySettings.vSyncCount=0;Application.targetFrameRate=60;QualitySettings.antiAliasing=4;
  try{InputPatches.InitDefaults();Harmony.CreateAndPatchAll(typeof(InputPatches));}catch(System.Exception e){Debug.LogError("Harmony: "+e.Message);}
  var g=new GameObject("RP");DontDestroyOnLoad(g);g.AddComponent<RuntimePatch>();
 }
 void Start(){Invoke("Build",0.6f);}
 void Update(){
  var n=Input.GetJoystickNames();bool now=false;
  foreach(var x in n)if(!string.IsNullOrEmpty(x))now=true;
  if(now!=hasPad){hasPad=now;var vc=GameObject.Find("VC");if(vc!=null)vc.SetActive(!hasPad);}
 }
 void Build(){
  var c=new GameObject("VC");DontDestroyOnLoad(c);
  var cv=c.AddComponent<Canvas>();cv.renderMode=RenderMode.ScreenSpaceOverlay;cv.sortingOrder=9999;
  var s=c.AddComponent<CanvasScaler>();s.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;s.referenceResolution=new Vector2(1920,1080);s.matchWidthOrHeight=0.5f;
  c.AddComponent<GraphicRaycaster>();
  var sa=Screen.safeArea;
  Stick(c.transform,new Vector2(sa.xMin+300,sa.yMin+300),"stick");
  Btn(c.transform,"A",new Vector2(sa.xMax-200,sa.yMin+200),"A",KeyCode.Z);
  Btn(c.transform,"B",new Vector2(sa.xMax-350,sa.yMin+120),"B",KeyCode.X);
  Btn(c.transform,"X",new Vector2(sa.xMax-200,sa.yMin+380),"X",KeyCode.Space);
  Btn(c.transform,"Y",new Vector2(sa.xMax-350,sa.yMin+300),"Y",KeyCode.LeftShift);
  Btn(c.transform,"L1",new Vector2(sa.xMax-500,sa.yMin+200),"L1",KeyCode.E);
  Btn(c.transform,"R1",new Vector2(sa.xMax-500,sa.yMin+380),"R1",KeyCode.Escape);
  Btn(c.transform,"Start",new Vector2(sa.xMax-200,sa.yMin+560),"Start",KeyCode.M);
  EditMode.OnLayoutChanged+=()=>{LayoutSaver.SaveAll();};
 }
 void Stick(Transform p,Vector2 pos,string id){
  var g=new GameObject(id);g.transform.SetParent(p,false);
  var bg=g.AddComponent<Image>();bg.color=new Color(1,1,1,0.2f);
  var r=g.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.zero;r.pivot=new Vector2(0.5f,0.5f);r.sizeDelta=new Vector2(240,240);r.anchoredPosition=pos;
  var h=new GameObject("Handle");h.transform.SetParent(g.transform,false);
  var hi=h.AddComponent<Image>();hi.color=new Color(1,1,1,0.5f);
  var hr=h.GetComponent<RectTransform>();hr.sizeDelta=new Vector2(100,100);
  var vs=g.AddComponent<VirtualStick>();vs.background=r;vs.handle=hr;vs.radius=100f;
  LayoutSaver.LoadLayout(r,id);
 }
 void Btn(Transform p,string n,Vector2 pos,string id,KeyCode kc){
  var g=new GameObject(id);g.transform.SetParent(p,false);
  var i=g.AddComponent<Image>();i.color=new Color(1,0.3f,0.3f,0.45f);
  var r=g.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.zero;r.pivot=new Vector2(0.5f,0.5f);r.sizeDelta=new Vector2(130,130);r.anchoredPosition=pos;
  var t=g.AddComponent<Text>();t.text=n;t.alignment=TextAnchor.MiddleCenter;t.color=Color.white;t.fontSize=28;
  var btn=g.AddComponent<VirtualButton>();btn.BtnId=id;btn.Key=kc;
  LayoutSaver.LoadLayout(r,id);
 }
}
