using UnityEngine;
using System.Collections;

public class AIBehavior : MonoBehaviour 
{
    public float flipRange;
    public Transform frontCheck;
    public Transform groundCheck;
    public string groundLayerName;
    public string obstacleInteractableName;
    public string playLayerName;
    public float maxSpeed;

    protected bool isMove = false;
    protected Vector2 targetPos;
    protected int facingDirection = 1;
    protected bool isGrounded = false;
    protected bool isAIInitialized = false;
    protected bool isDiving = false;
    protected GameObject player;

    public virtual void initializeAI() { }

    public virtual void registerBehavior(System.Object obj) { }

    public virtual void playerInRange() { }

    public virtual void changeDirection(bool isForward) { }

    public virtual void move() { }

    public virtual void freeze() { }

    public virtual void damage() { }

    public virtual void dead() { }
}