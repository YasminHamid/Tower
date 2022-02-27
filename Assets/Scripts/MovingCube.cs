using System;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube currentCube { get; private set; }
    public static MovingCube lastCube { get; private set; }
    public MoveDirection moveDirection { get; set; }

    public static event Action onCubeStop = delegate { };
    public static event Action onGameOver = delegate { };


    public float moveSpeed = 1f;

    private void OnEnable()
    {
        if (lastCube == null)
            lastCube = GameObject.Find("startCube").GetComponent<MovingCube>();

        currentCube = this;

        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(lastCube.transform.localScale.x, transform.localScale.y, lastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    public void stop()
    {
        moveSpeed = 0;
        float hangover = getHangover();

        float max = (moveDirection == MoveDirection.Z || moveDirection == MoveDirection.Z2) ? lastCube.transform.localScale.z : lastCube.transform.localScale.x;

        if (Mathf.Abs(hangover) >= max)
        {
            lastCube = null;
            currentCube = null;
            onGameOver();
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();

        }
        else
        {
            float direction = hangover > 0 ? 1f : -1f;
            
            if (hangover != 0)
            {
                onCubeStop();
                if (moveDirection == MoveDirection.Z || moveDirection == MoveDirection.Z2)
                    splitCubeOnZ(hangover, direction);
                else
                    splitCubeOnX(hangover, direction);
            }
                
            lastCube = this;

        }
    }

    private float getHangover()
    {
        if( moveDirection == MoveDirection.Z || moveDirection == MoveDirection.Z2)
            return transform.position.z - lastCube.transform.position.z;
        else
            return transform.position.x - lastCube.transform.position.x;
    }

    private void splitCubeOnX(float hangover, float direction)
    {
        float newXSize = lastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPosition = lastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        spawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void splitCubeOnZ(float hangover, float direction)
    {
        float newZSize = lastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = lastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

        spawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void spawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(moveDirection == MoveDirection.Z || moveDirection == MoveDirection.Z2)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveDirection)
        {
            case MoveDirection.X:
                transform.position += transform.right * Time.deltaTime * moveSpeed;
                break;
            case MoveDirection.Z:
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
                break;
            case MoveDirection.X2:
                transform.position += transform.right * Time.deltaTime * moveSpeed * -1;
                break;
            case MoveDirection.Z2:
                transform.position += transform.forward * Time.deltaTime * moveSpeed * -1;
                break;
        }
    }
}
