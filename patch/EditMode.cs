using UnityEngine;using UnityEngine.UI;using UnityEngine.EventSystems;
public class EditMode:MonoBehaviour{
 public static bool Active=false;
 public static System.Action OnLayoutChanged;
 static RectTransform dragging;static Vector2 offset;
 static GameObject addBtn;
 static readonly KeyCode[] Options={KeyCode.Z,KeyCode.X,KeyCode.C,KeyCode.V,KeyCode.Space,KeyCode.LeftShift,KeyCode.RightShift,KeyCode.E,KeyCode.Q,KeyCode.F,KeyCode.R,KeyCode.Escape,KeyCode.Tab,KeyCode.M,KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.RightArrow};
 public static void Toggle(){
  Active=!Active;
  var vc=GameObject.Find("VC");
  if(vc!=null){
   foreach(Transform child in vc.transform){
    var img=child.GetComponent<Image>();
    if(img!=null)img.color=Active?new Color(1,1,0,0.4f):new Color(1,0.3f,0.3f,0.45f);
   }
   if(Active)ShowAddBtn(vc.transform);
   else if(addBtn!=null){Destroy(addBtn);addBtn=null;}
  }
  if(!Active)OnLayoutChanged?.Invoke();
  Debug.Log("EditMode: "+(Active?"ON":"OFF"));
 }
 static void ShowAddBtn(Transform parent){
  if(addBtn!=null)return;
  addBtn=new GameObject("AddBtn");addBtn.transform.SetParent(parent,false);
  var img=addBtn.AddComponent<Image>();img.color=new Color(0,1,0,0.6f);
  var r=addBtn.GetComponent<RectTransform>();r.anchorMin=new Vector2(1,0);r.anchorMax=new Vector2(1,0);r.pivot=new Vector2(0.5f,0.5f);r.sizeDelta=new Vector2(120,120);r.anchoredPosition=new Vector2(-100,900);
  var t=addBtn.AddComponent<Text>();t.text="+";t.alignment=TextAnchor.MiddleCenter;t.color=Color.white;t.fontSize=60;
  var btn=addBtn.AddComponent<Button>();btn.onClick.AddListener(AddNewButton);
 }
 static void AddNewButton(){
  var vc=GameObject.Find("VC");if(vc==null)return;
  int idx=PlayerPrefs.GetInt("custom_btn_count",0);
  string id="C"+idx;
  KeyCode kc=Options[idx%Options.Length];
  var g=new GameObject(id);g.transform.SetParent(vc.transform,false);
  var img=g.AddComponent<Image>();img.color=new Color(1,1,0,0.4f);
  var r=g.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.zero;r.pivot=new Vector2(0.5f,0.5f);r.sizeDelta=new Vector2(130,130);r.anchoredPosition=new Vector2(500,500);
  var t=g.AddComponent<Text>();t.text=kc.ToString();t.alignment=TextAnchor.MiddleCenter;t.color=Color.white;t.fontSize=24;
  var btn=g.AddComponent<VirtualButton>();btn.BtnId=id;btn.Key=kc;
  InputPatches.RegisterKey(id,kc);
  PlayerPrefs.SetInt("custom_btn_count",idx+1);
  PlayerPrefs.Save();
 }
 public static void RequestDelete(GameObject g){
  string id=g.name;
  if(id.StartsWith("C")){InputPatches.UnregisterKey(id);PlayerPrefs.Save();}
  Destroy(g);Debug.Log("Deleted "+id);
 }
 public static void BeginDrag(GameObject g,PointerEventData e){
  dragging=g.GetComponent<RectTransform>();
  RectTransformUtility.ScreenPointToLocalPointInRectangle(dragging.parent as RectTransform,e.position,e.pressEventCamera,out offset);
  offset=dragging.anchoredPosition-offset;
 }
 public static void DragMove(RectTransform rt,PointerEventData e){
  if(rt==null)return;
  Vector2 pos;RectTransformUtility.ScreenPointToLocalPointInRectangle(rt.parent as RectTransform,e.position,e.pressEventCamera,out pos);
  rt.anchoredPosition=pos+offset;
 }
}
