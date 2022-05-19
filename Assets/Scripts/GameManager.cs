
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;

    public static GameManager GetInstance()
    {
        return mInstance;
    }

    public HeroController hero;

    private void Awake()
    {
        mInstance = this;
    }
}
