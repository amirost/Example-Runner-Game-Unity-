using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroyGameobject : MonoBehaviour
{
    // THIS SCRIPT IS USED TO DESTROY UNUSED GAMEOBJECTS EITHER BY A TIMER OR WHEN THE CAMERA PASSES THE OBJECT
    public bool destroyByCamera;
    public float DistanceFromCamera;

    public bool destroyByTimer;
    public float Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (destroyByTimer) 
        {
            StartCoroutine(DestroyTimer(Timer));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (destroyByCamera)
        {
            if (Camera.main.transform.position.z - transform.position.z   > DistanceFromCamera) 
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator DestroyTimer(float timer) 
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }


}
#if UNITY_EDITOR

[CustomEditor(typeof(DestroyGameobject))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as DestroyGameobject;

        myScript.destroyByCamera = GUILayout.Toggle(myScript.destroyByCamera, "Destroy By Camera");

        if (myScript.destroyByCamera)
            myScript.DistanceFromCamera = EditorGUILayout.FloatField("Distance From Camera :", myScript.DistanceFromCamera);


        myScript.destroyByTimer = GUILayout.Toggle(myScript.destroyByTimer, "Destroy By Timer");

        if (myScript.destroyByTimer)
            myScript.Timer = EditorGUILayout.FloatField("Timer :", myScript.Timer);

    }
}
#endif