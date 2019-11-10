using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Puzzle;
using UnityEngine;

public class ShitEnemy : MonoBehaviour, IEnemy
{

    [SerializeField] private GameObject blot;
    private float _speed = 0.01f;
    private Vector3 _target;

    public void OnHitPlayer(Player player)
    {
        player.DealDamage(Damage);
        Destroy(gameObject);
    }

    public void Die()
    {
        GameObject effect = Instantiate(blot);
        effect.transform.position = transform.position;
        GameSceneManager.Instance.score.AddScore(20);
        Destroy(gameObject);
    }

    public void Move()
    {
        transform.Translate(new Vector3(_speed, 0), Space.Self);
    }

    public void Instantiate(Side side, float? speed = null)
    {
        if (speed != null)
            _speed = (float)speed;
        transform.Rotate(new Vector3(0, 0, 90 * side.GetHashCode())); 
        _target = GameSceneManager.Instance.player.transform.position;
        switch (side)
        {
            case Side.Right: 
                transform.position = _target + Vector3.right * 10f;
                transform.right = Vector3.left;
                break;
            case Side.Left: transform.position = _target + Vector3.left * 10f;
                transform.right = Vector3.right;
                break;
            case Side.Up: 
                transform.position = _target + Vector3.up * 10f;
                transform.right = Vector3.down;
                break;
            case Side.Down: transform.position = _target + Vector3.down * 10f;
                transform.right = Vector3.up;
                break;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMouseDown()
    {
        Die();
    }

    private int _damage = 1;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value;}
    }
}
