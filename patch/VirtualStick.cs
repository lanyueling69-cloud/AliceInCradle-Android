using UnityEngine;using UnityEngine.UI;using UnityEngine.EventSystems;
public class VirtualStick:MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler{
 public RectTransform background;public RectTransform handle;public float radius=100f;
 float pressTime=0f;
 public void OnPointerDown(PointerEventData e){pressTime=Time.time;OnDrag(e);}
 public void OnDrag(PointerEventData e){
  if(EditMode.Active){EditMode.DragMove(background,e);return;}
  Vector2 pos;RectTransformUtility.ScreenPointToLocalPointInRectangle(background,e.position,e.pressEventCamera,out pos);
  pos=Vector2.ClampMagnitude(pos,radius);handle.anchoredPosition=pos;
  InputPatches.VirtualH=pos.x/radius;InputPatches.VirtualV=pos.y/radius;
 }
 public void OnPointerUp(PointerEventData e){
  if(EditMode.Active)return;
  if(Time.time-pressTime>3f)EditMode.Toggle();
  handle.anchoredPosition=Vector2.zero;InputPatches.VirtualH=0;InputPatches.VirtualV=0;
 }
}
