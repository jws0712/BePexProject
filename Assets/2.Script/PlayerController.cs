//UnityEngine
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            JudgeManager.Instance.JudgeNote();
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //UI �� ���������� ��ġ �̺�Ʈ�� ����
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

            if(touch.phase == TouchPhase.Began)
            {
                JudgeManager.Instance.JudgeNote();
            }
        }
    }
}
