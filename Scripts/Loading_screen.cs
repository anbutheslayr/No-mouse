using Godot;
using System;

public class Loading_screen : Control
{
    public ColorRect colorRect;
    public Label label;
    public override void _Ready()
    {
        colorRect = GetNode<ColorRect>("ColorRect");
        colorRect.Visible = false;
        label = GetNode<Label>("ColorRect/Label");
    }

    public void SceneChange( string scenelocation )
    {
        colorRect.Visible = true;
        var scene = ResourceLoader.LoadInteractive(scenelocation,"PackedScene");
        while(true)
        {
            var err = scene.Poll();
            if(err == Error.FileEof)
            {
                var res = scene.GetResource();
                GetTree().ChangeSceneTo((PackedScene)res); 
                colorRect.Visible = false;
                break;
            }
            if (err == Error.Ok)
            {
                var progress = scene.GetStage()/scene.GetStageCount();
                label.Text = "Loading... " + (progress*100).ToString() + "%";
            }
        }
    }


}
