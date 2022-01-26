using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles;      // List of obstacles prefabs
    
    public bool RealTimeSpawner;            // if enabled, will instantiate the obstacles while player is moving
    public bool SpawnAtStart;               // if enabled will instantiate the obstacles at the start of the scene

    public float DistanceBetweenObstacles;  // Distance between two obstacles
    public bool FixedDistance;              // if enabled the distance between two obstacles will be always the same

    public bool RandomRangeDistance;        // if enabled the distance between two obstacles will be random in range [minDistance, maxDistance]
    public float minDistance;
    public float maxDistance;

    public Vector3 startSpawnPosition;      // Position of the first obstacle
    public Vector3 endSpawnPosition;        // limit for the spawn obstacles

    private GameObject lastSpawnedObstacle;

    // Parameters for real time spawning
    public Transform player;                
    public float DistanceFromPlayer;

   
    // Parameters for spawning at the start
    public int numberObstacles;
    

    void Start()
    {
        SpawnObstacle(startSpawnPosition);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RealTimeSpawner)                                                                                // Check if realTimeSpawn is enabled
        {
            float distance = Vector3.Distance(player.position, lastSpawnedObstacle.transform.position);     // Get the distance between the last instantiated obstacle and the player
            if (distance < DistanceFromPlayer)                                                              // if distance is lower than DistanceFromPlayer then we instantiate a new obstacle
            {   
                SpawnObstacle(startSpawnPosition);
                
            }
            
        }
    }

    void SpawnObstacle(Vector3 startPosition) 
    {
        if (startPosition.z < endSpawnPosition.z)                                                           // Check if the next spawn position hasn't reached the limit yet
        {
            int i = Random.Range(0, obstacles.Count);                                                       // Randomly chose an obstacle

            Vector3 position = new Vector3(                                                                 // Set the position of the obstacle
                obstacles[i].transform.position.x,
                obstacles[i].transform.position.y,
                startPosition.z
                );

            lastSpawnedObstacle = Instantiate(obstacles[i], position, obstacles[i].transform.rotation);     

            startSpawnPosition = new Vector3(                                                               // Set the position where the next obstacle will be instantiated
                lastSpawnedObstacle.transform.position.x,
                lastSpawnedObstacle.transform.position.y,
                lastSpawnedObstacle.transform.position.z + SetRandomDistanceBetweenObstacles()
                );

            if (SpawnAtStart)                                                                               // if SpawnAtStart is enabled we decrease the remaining obstacles to instantiate
            {                                                                                               // and we do a recursive call for "SpawnObstacle"
                numberObstacles--;
                if (numberObstacles > 0)
                {
                    SpawnObstacle(startSpawnPosition);
                }
            }
        }
    }

    float SetRandomDistanceBetweenObstacles() 
    {
        if (RandomRangeDistance)
            return Random.Range(minDistance, maxDistance);

        else if (FixedDistance)
            return DistanceBetweenObstacles;

        else return 0;
    }


}
#region Custom Editor Code
#if UNITY_EDITOR

[CustomEditor(typeof(ObstacleSpawner))]
public class ObstacleSpawnerCustomEditor : Editor
{
    public GameObject[] obstacles;
    ScriptableObject scriptableObj;
    SerializedObject serialObj;

    private void OnEnable()
    {
        scriptableObj = this;
        serialObj = new SerializedObject(scriptableObj);
    }
    override public void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var myScript = target as ObstacleSpawner;

        EditorGUILayout.LabelField("Obstacles");
        List<GameObject> listobstacles = myScript.obstacles;

        int size = Mathf.Max(0, EditorGUILayout.IntField("Size", listobstacles.Count));

        while (size > listobstacles.Count)
        {
            listobstacles.Add(null);
        }

        while (size < listobstacles.Count)
        {
            listobstacles.RemoveAt(listobstacles.Count - 1);
        }

        for (int i = 0; i < listobstacles.Count; i++)
        {
            listobstacles[i] = EditorGUILayout.ObjectField("Obstacle " + i + ":", listobstacles[i], typeof(GameObject), true) as GameObject;
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Distance between obstacles");
        EditorGUILayout.BeginHorizontal();
        myScript.FixedDistance = GUILayout.Toggle(myScript.FixedDistance, "Fixed Distance");
        if (myScript.FixedDistance) 
        {
            myScript.RandomRangeDistance = false;
            myScript.DistanceBetweenObstacles = EditorGUILayout.FloatField("Distance : ", myScript.DistanceBetweenObstacles);

        }
        EditorGUILayout.EndHorizontal();
        
        myScript.RandomRangeDistance = GUILayout.Toggle(myScript.RandomRangeDistance, "Range Distance");
        if (myScript.RandomRangeDistance)
        {
            myScript.FixedDistance = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min : ", GUILayout.MaxWidth(50));
            myScript.minDistance = EditorGUILayout.FloatField(myScript.minDistance);
            EditorGUILayout.LabelField("Max : ", GUILayout.MaxWidth(50));
            myScript.maxDistance = EditorGUILayout.FloatField(myScript.maxDistance);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();


        myScript.startSpawnPosition = EditorGUILayout.Vector3Field("Start Position :", myScript.startSpawnPosition) ;
        myScript.endSpawnPosition = EditorGUILayout.Vector3Field("End Position :", myScript.endSpawnPosition);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        myScript.RealTimeSpawner = GUILayout.Toggle(myScript.RealTimeSpawner, "Real-time spawning obstacles");

        if (myScript.RealTimeSpawner)
        {
            myScript.SpawnAtStart = false;
            myScript.player = EditorGUILayout.ObjectField("Player :", myScript.player, typeof(Transform), true) as Transform;
            myScript.DistanceFromPlayer = EditorGUILayout.FloatField("Distance player - last obstacle:", myScript.DistanceFromPlayer);

            EditorGUILayout.Separator();
        }

        myScript.SpawnAtStart = GUILayout.Toggle(myScript.SpawnAtStart, "Spawn obstacles at start");
        if (myScript.SpawnAtStart)
        {
            myScript.RealTimeSpawner = false;
            myScript.numberObstacles = EditorGUILayout.IntField("Number of obstacles :", myScript.numberObstacles);
            //myScript.FirstObstaclePosition = EditorGUILayout.FloatField("Position of the first obstacle (Z axis) :", myScript.FirstObstaclePosition);
        }

    }

}
#endif
#endregion