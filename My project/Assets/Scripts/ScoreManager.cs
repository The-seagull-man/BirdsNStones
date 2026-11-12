using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ScoreManager : MonoBehaviour
{
    public int _Score;

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _Score = 0;
    }
}
