using Godot;
using System;

public class Loading_screen : Control
{
    public CanvasLayer canvasLayer;
    public Label label;
    public override void _Ready()
    {
        canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
        canvasLayer.Visible = false;
        label = GetNode<Label>("CanvasLayer/Label");
    }

    public void SceneChange( string scenelocation )
    {
        canvasLayer.Visible = true;
        var scene = ResourceLoader.LoadInteractive(scenelocation,"PackedScene");
        while(true)
        {
            var err = scene.Poll();
            if(err == Error.FileEof)
            {
                var res = scene.GetResource();
                GetTree().ChangeSceneTo((PackedScene)res); 
                canvasLayer.Visible = false;
                // break;
            }
            if (err == Error.Ok)
            {
                var progress = scene.GetStage()/scene.GetStageCount();
                label.Text = "Loading... " + (progress*100).ToString() + "%";
            }
        }
    }


}
