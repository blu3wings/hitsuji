using UnityEngine;
using System.Collections;

public class CharacterBase : MonoBehaviour 
{
    public enum Type
    {
        PLAYER,
        FRIEND,
        ENEMY
    }
    
    public float moveForce = 365f;
    public float jumpForce = 10f;
    public float maxSpeed = 5f;
    public float minSpeed = .3f;
    public bool usePhysics = true;
    public bool isDead = false;

    protected Vector2 _screenForward = Vector2.up;
    protected Vector2 _screenRight = Vector2.right;
    protected Vector2 _direction = Vector2.zero;
    protected bool isGrounded = false;    
    protected bool isJumped = false;
    protected bool facingRight = false;
    protected bool allowMove = false;

    public virtual void dead() { }
}