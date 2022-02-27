using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public MovingCube cubePrefab;
    public MoveDirection moveDirection;

    public void spawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.lastCube != null && MovingCube.lastCube.gameObject != GameObject.Find("startCube"))
        {
            float x = (moveDirection == MoveDirection.X || moveDirection == MoveDirection.X2 ) ? transform.position.x  : MovingCube.lastCube.transform.position.x;
            float z = (moveDirection == MoveDirection.Z || moveDirection == MoveDirection.Z2 ) ? transform.position.z  : MovingCube.lastCube.transform.position.z;

            cube.transform.position = new Vector3(x,
                MovingCube.lastCube.transform.position.y + cubePrefab.transform.localScale.y,
                z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.moveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}

public enum MoveDirection
{
    X,
    Z,
    X2,
    Z2
}
