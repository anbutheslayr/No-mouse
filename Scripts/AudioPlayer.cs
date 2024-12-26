using Godot;
using System.Collections.Generic;
using System;

public class AudioPlayer : Spatial
{
    public AudioStreamPlayer Att;
    public AudioStreamPlayer BG;
    public override void _Ready()
    {
        Att = GetNode<AudioStreamPlayer>("Att");
        BG = GetNode<AudioStreamPlayer>("BG");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(GetTree().GetNodesInGroup("Enemy").Count > 0 && !BG.Playing)
		{   
            BG.Playing = true;
			Att.Playing = true;
		}
		else if(BG.Playing && GetTree().GetNodesInGroup("Enemy").Count == 0)
		{
			BG.Playing = false;
			Att.Playing = false;
		}
    }   
}
