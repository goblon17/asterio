using Godot;
using System;
using System.Linq;

[GlobalClass]
public abstract partial class AsteroidBase : RigidBody3D
{
    [Export]
    private float health;
    [Export]
    private float damage;
    [Export]
    private MeshInstance3D mesh;
    [Export]
    private CollisionShape3D collisionShape;

    public event Action<AsteroidBase> Died;

    public float Damage => damage;

    public override void _Ready()
    {
        Scale = Vector3.One * 0.01f;
        Tween tween = CreateTween();
        tween.TweenProperty(this, Node3D.PropertyName.Scale.ToString(), Vector3.One, 1);
    }

    protected T SpawnLocalObject<T>(PackedScene prefab, Vector3 pos, Vector3 normal) where T : Node3D
    {
        T instance = prefab.Instantiate<T>();
        AddChild(instance);
        instance.GlobalPosition = pos;
        instance.Quaternion = new Quaternion(Vector3.Up, normal);
        return instance;
    }

    public void TakeDamage(float dmg, Vector3 pos, Vector3 normal)
    {
        TakeDamageInternal(dmg, pos, normal);

        if ((health -= dmg) <= 0)
        {
            Die();
        }
    }

    protected abstract void TakeDamageInternal(float dmg, Vector3 pos, Vector3 normal);

    public void Die()
    {
        DieInternal();

        Died?.Invoke(this);
        QueueFree();
    }

    protected abstract void DieInternal();

    public void SetAsteroid(AsteroidShape shape)
    {
        mesh.Mesh = shape.Mesh;
        collisionShape.Shape = shape.CollisionShape;
    }
}
