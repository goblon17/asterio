using Godot;
using System;

public partial class GameMusic : Node
{
	public enum Music { Normal, Radio, None }

	public static GameMusic Instance { get; private set; }

	[Export]
	private float fadeDuration;

	private Music music;

	private int normalIdx;
	private int radioIdx;

    public override void _Ready()
    {
		normalIdx = AudioServer.GetBusIndex("Normal");
		radioIdx = AudioServer.GetBusIndex("Radio");

		Instance = this;

		SetMusic(Music.Radio, true);
    }

    public override void _Process(double delta)
    {
        float normalDb = AudioServer.GetBusVolumeDb(normalIdx);
        float radioDb = AudioServer.GetBusVolumeDb(radioIdx);

		float normalVol = Mathf.DbToLinear(normalDb);
		float radioVol = Mathf.DbToLinear(radioDb);

        normalVol = Mathf.MoveToward(normalVol, music == Music.Normal ? 1 : 0.001f, 1 / fadeDuration * (float)delta);
        radioVol = Mathf.MoveToward(radioVol, music == Music.Radio ? 1 : 0.001f, 1 / fadeDuration * (float)delta);

		normalDb = Mathf.LinearToDb(normalVol);
		radioDb = Mathf.LinearToDb(radioVol);

        AudioServer.SetBusVolumeDb(normalIdx, normalDb);
        AudioServer.SetBusVolumeDb(radioIdx, radioDb);
    }

    public void SetMusic(Music music, bool instant)
	{
		this.music = music;

		if (instant)
		{
			AudioServer.SetBusVolumeDb(normalIdx, (float)Mathf.LinearToDb(music == Music.Normal ? 1 : 0.001));
			AudioServer.SetBusVolumeDb(radioIdx, (float)Mathf.LinearToDb(music == Music.Radio ? 1 : 0.001));
		}
	}
}
