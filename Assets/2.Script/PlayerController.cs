//UnityEngine
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            JudgeManager.Instance.JudgeNote((int)LineType.One);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            JudgeManager.Instance.JudgeNote((int)LineType.Two);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            JudgeManager.Instance.JudgeNote((int)LineType.Three);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            JudgeManager.Instance.JudgeNote((int)LineType.Four);
        }

        //if(Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    //UI 를 눌렀을때는 터치 이벤트를 막음
        //    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        //    if(touch.phase == TouchPhase.Began)
        //    {
        //        JudgeManager.Instance.JudgeNote();
        //    }
        //}
    }
}
