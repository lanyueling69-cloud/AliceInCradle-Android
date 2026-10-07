using UnityEngine;
public class LayoutSaver{
 static string K="layout_";
 public static void SaveAll(){
  var vc=GameObject.Find("VC");if(vc==null)return;
  foreach(Transform child in vc.transform){
   var rt=child.GetComponent<RectTransform>();if(rt==null)continue;
   string id=child.name;
   PlayerPrefs.SetFloat(K+id+"_x",rt.anchoredPosition.x);
   PlayerPrefs.SetFloat(K+id+"_y",rt.anchoredPosition.y);
  }
  PlayerPrefs.Save();Debug.Log("Layout saved");
 }
 public static void LoadLayout(RectTransform rt,string id){
  if(rt==null)return;
  float x=PlayerPrefs.GetFloat(K+id+"_x",rt.anchoredPosition.x);
  float y=PlayerPrefs.GetFloat(K+id+"_y",rt.anchoredPosition.y);
  rt.anchoredPosition=new Vector2(x,y);
 }
}
