using Godot;
using System;
using System.Linq;

public partial class UpgradeIndicator : Node3D
{
    [Export]
    private Key decreaseLevelKey;
    [Export]
    private Key increaseLevelKey;
    [Export]
    private MeshInstance3D decreaseTextMeshInscance;
    [Export]
    private MeshInstance3D increaseTextMeshInscance;
    [Export]
    private MeshInstance3D[] indicatorMaterialsInstances;
    [Export]
    private float fadeDuration;
    [Export(hint: PropertyHint.Range, "0, 1")]
    private float toOneDurationPercentage;

    private TextMesh decreaseLevelTextMesh;
    private TextMesh increaseLevelTextMesh;
    private StandardMaterial3D[] indicatorMaterials;

    public int CurrentLevel { get; private set; } = 0;

    private ShopPanel shopPanel;

    private T FindParentOfType<T>() where T : Node
    {
        Node parent = GetParent();
        while (parent != null)
        {
            if (parent is T)
            {
                return parent as T;
            }
            parent = parent.GetParent();
        }
        return null;
    }

    public override void _Ready()
    {
        shopPanel = FindParentOfType<ShopPanel>();

        indicatorMaterials = new StandardMaterial3D[indicatorMaterialsInstances.Length];

        for (int i = 0; i < indicatorMaterialsInstances.Length; i++)
        {
            MeshInstance3D instance = indicatorMaterialsInstances[i];
            StandardMaterial3D newMaterial = instance.Mesh.SurfaceGetMaterial(0).Duplicate() as StandardMaterial3D;
            instance.MaterialOverride = newMaterial;
            indicatorMaterials[i] = newMaterial;
        }

        decreaseLevelTextMesh = decreaseTextMeshInscance.Mesh.Duplicate() as TextMesh;
        decreaseTextMeshInscance.Mesh = decreaseLevelTextMesh;
        decreaseLevelTextMesh.Text = OS.GetKeycodeString(DisplayServer.KeyboardGetKeycodeFromPhysical(decreaseLevelKey));

        increaseLevelTextMesh = increaseTextMeshInscance.Mesh.Duplicate() as TextMesh;
        increaseTextMeshInscance.Mesh = increaseLevelTextMesh;
        increaseLevelTextMesh.Text = OS.GetKeycodeString(DisplayServer.KeyboardGetKeycodeFromPhysical(increaseLevelKey));
    }

    public override void _Process(double delta)
    {
        for (int i = 0; i < indicatorMaterials.Length; i++)
        {
            StandardMaterial3D material = indicatorMaterials[i];
            float target = i == CurrentLevel ? 16 : 0;
            float speed = 15 / (fadeDuration * (1 - toOneDurationPercentage));
            if (material.EmissionEnergyMultiplier < 1)
            {
                speed = 1 / (fadeDuration * toOneDurationPercentage);
            }
            material.EmissionEnergyMultiplier = Mathf.MoveToward(material.EmissionEnergyMultiplier, target, speed * (float)delta);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (shopPanel.State != ShopPanel.ShopState.Opened)
        {
            return;
        }

        if (@event is InputEventKey inputKey && inputKey.Pressed)
        {
            if (inputKey.PhysicalKeycode == decreaseLevelKey)
            {
                DecreaseLevel();
            }
            else if (inputKey.PhysicalKeycode == increaseLevelKey)
            {
                IncreaseLevel();
            }
        }
    }

    private void DecreaseLevel()
    {
        if (CurrentLevel > 0)
        {
            CurrentLevel--;
            shopPanel.ReturnPoint();
        }
    }

    private void IncreaseLevel()
    {
        if (CurrentLevel < indicatorMaterials.Length - 1 &&
            shopPanel.TryGetPoint())
        {
            CurrentLevel++;
        }
    }
}
