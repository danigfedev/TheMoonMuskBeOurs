using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerCustomUI : Editor
{

    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("IMPORTANT: Every Pool has a tag property that matches its prefab's tag. Use the button bellow to set the tags. It can be done manually", MessageType.Info);
        
        //=== GUI button ===
        ObjectPooler pool = (ObjectPooler)target;
        if (GUILayout.Button("Update pools' tags"))
        {
            pool.UpdatePoolTags();
        }

        DrawDefaultInspector();

    }
}
