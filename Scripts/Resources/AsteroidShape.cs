using Godot;
using System;

[GlobalClass]
public partial class AsteroidShape : Resource
{
    [Export]
    private ArrayMesh mesh;
    [Export]
    private ConcavePolygonShape3D collisionShape;

    public ArrayMesh Mesh => mesh;
    public ConcavePolygonShape3D CollisionShape => collisionShape;
}
