//UnityEngnie
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
            }

            return instance;
        }
    }

    //��� �޾Ƽ� ������ �� �� �ִ�
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
