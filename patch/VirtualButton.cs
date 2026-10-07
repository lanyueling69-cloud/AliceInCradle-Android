using UnityEngine;using UnityEngine.EventSystems;
public class VirtualButton:MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler{
 public string BtnId;public KeyCode Key=KeyCode.None;
 float pressTime=0f;
 public void OnPointerDown(PointerEventData e){
  pressTime=Time.time;
  if(EditMode.Active){EditMode.BeginDrag(gameObject,e);return;}
  InputPatches.SetButton(BtnId,true);
 }
 public void OnPointerUp(PointerEventData e){
  if(EditMode.Active){if(Time.time-pressTime>2f)EditMode.RequestDelete(gameObject);return;}
  InputPatches.SetButton(BtnId,false);
 }
 public void OnDrag(PointerEventData e){if(EditMode.Active)EditMode.DragMove(GetComponent<RectTransform>(),e);}
}
