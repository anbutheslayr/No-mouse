using Godot;
using System;

public class Popuptext : Spatial
{
    public Label3D label;
    public AnimationPlayer anim;
    public Tween tween;
    public override void _Ready()
    {
        label = GetNode<Label3D>("Label3D");
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        tween = GetNode<Tween>("Tween");
    }

    public void PlayAnim(string damage,int spread, int height ,Vector3 pos , bool self = false)
    {
        GlobalTranslation = pos;
        label.Text = damage;
        float tweenlength;
        if (self == true)
        {
            tweenlength = anim.GetAnimation("Damagedone").Length;
            anim.Play("Damagereceive");
        }
        else
        {
            tweenlength = anim.GetAnimation("Damagedone").Length;
            anim.Play("Damagedone");
        }
        var rand = new RandomNumberGenerator();
        var end_pos = new Vector3(rand.RandiRange(-spread,spread),height,rand.RandiRange(-spread, spread)) + Translation;
        tween.InterpolateProperty(this, "translation" ,Translation,end_pos , tweenlength , Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }

}
