using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TileBasedGeneration : MonoBehaviour
{
    [SerializeField] private float PlayerDistanceSpawnLevelPart = 19f;
    [SerializeField] private GameObject Player;

    [Header("Intro")]
    [SerializeField] private int startingSpawnLevelParts = 3;
    [SerializeField] private Transform IntroLevel;

    [Header("Act's")]
    [SerializeField] private const float IntroStart = 0f;
    [SerializeField] public int RoomsSinceStart = -1;

    [Header("Act's Level prefab Lists")]
    [SerializeField] private Transform[] HillGenerationPrefabList;

    private Transform chosenLevelPrefab;
    private Vector3 lastEndPosition;
    [SerializeField] private Vector3 startPosition;

    private void Awake()
    {
        SpawnLevelPrefab();

        for (int i = 0; i < startingSpawnLevelParts; i++)
        {
            SpawnLevelPrefab();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(Player.gameObject.transform.position, lastEndPosition) < PlayerDistanceSpawnLevelPart)
        {
            SpawnLevelPrefab();
        }
    }

    private void SpawnLevelPrefab()
    {
        if (RoomsSinceStart == 0)
        {
            chosenLevelPrefab = IntroLevel;
        }

        if (RoomsSinceStart > 0)
        {
            chosenLevelPrefab = HillGenerationPrefabList[Random.Range(0, HillGenerationPrefabList.Length)];
        }

        if (chosenLevelPrefab == null)
        {
            Debug.Log("Something went wrong");
        }

        if (chosenLevelPrefab == IntroLevel)
        {
            Transform lastLevelPartTransform = SpawnLevelPrefab(chosenLevelPrefab, startPosition);
            lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
        } else
        {
            Transform lastLevelPartTransform = SpawnLevelPrefab(chosenLevelPrefab, lastEndPosition);
            lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
        }
    }

    private Transform SpawnLevelPrefab(Transform levelPart, Vector3 spawnPostition)
    {
        RoomsSinceStart++;
        Transform levelPrefabTransform = Instantiate(levelPart, spawnPostition, Quaternion.identity);
        return levelPrefabTransform;
    }
}