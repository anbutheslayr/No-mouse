using Godot;
using System;

public partial class Loading_screen : Control
{
    public Label label;
    public Global_vars gl;
    int i = 0;
    public override void _Ready()
    {
        gl = GetNode<Global_vars>("/root/GlobalVars");
        label = GetNode<Label>("ColorRect/Label");
        Hide();
        SceneChange(gl.SceneToLoad);
    }

    public async void SceneChange(string sceneName)
    {
        i=0;
        Show();
        {
            if(!ResourceLoader.HasCached(sceneName))
            {
                var scene = ResourceLoader.LoadInteractive(sceneName);
            
                GetTree().ChangeSceneToPacked(null);
                while (true)
                {
                    var err = scene.Poll();
                    if(err == Error.FileEof)
                    {
                        var res = scene.GetResource();
                        GetTree().ChangeSceneToPacked((PackedScene)res);
                        Hide();
                        QueueFree();
                        break; 
                    }
                    if (err == Error.Ok)
                    {
                        float progress = (float)scene.GetStage()/scene.GetStageCount();
                        label.Text = "Loading... " + ((int)(progress*100)).ToString() + "%";
                        GD.Print(scene.GetStage()+"/"+scene.GetStageCount());
                    }
                    i++;
                    if(i%((int)(scene.GetStageCount()*0.04)+1) == 0)
                    {
                        await ToSignal(GetTree(), "idle_frame");
                    }
                }
            }
            else
            {
                GetTree().ChangeSceneToFile(sceneName);
                // GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>(sceneName).Instance());
                Hide();
                QueueFree();
            }
        }

        
    }
}
