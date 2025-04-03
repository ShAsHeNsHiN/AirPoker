using Unity.VisualScripting;
using UnityEngine;

public static class ComponentCreator<TComponent> where TComponent : MonoBehaviour
{
    public static TComponent Create()
    {
        GameObject gameObject = new(typeof(TComponent).Name);

        gameObject.transform.AddComponent<TComponent>();

        var tComponent = gameObject.transform.GetComponent<TComponent>();

        return tComponent;
    }
}