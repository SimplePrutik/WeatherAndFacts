using UnityEditor;
using UnityEngine;

public class ObjectHelpers : MonoBehaviour
{
    [MenuItem("GameObject/Object helpers/Do some useful")]
    public static void DoSomething()
    {
        Transform rootTransform = Selection.activeGameObject.transform;
        int childCount = Selection.activeGameObject.transform.childCount;
        if (childCount == 0)
            return;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = rootTransform.GetChild(i);
            child.localScale = child.localScale * 50f;
            child.localPosition = child.localPosition * 50f;
        }
    }
}
