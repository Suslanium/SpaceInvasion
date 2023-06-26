using UnityEngine;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshGenerator : MonoBehaviour
{
    public void GenerateNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
