using Godot;
using System;

public class Popuptext : Spatial
{
    public Label3D label;
    public AnimationPlayer anim;
    public Tween tween;
    public float tweenlength;
    public override void _Ready()
    {
        label = GetNode<Label3D>("Label3D");
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        tween = GetNode<Tween>("Tween");
    }

    public void PlayAnim(string damage,int spread, int height ,Vector3 pos , int an)
    {
        GlobalTranslation = pos;
        label.Text = damage;
        
        switch (an)
        {
            case 0:
                tweenlength = anim.GetAnimation("Damagedone").Length;
                anim.Play("Damagedone");
                break;
            case 1:
                tweenlength = anim.GetAnimation("Damagereceive").Length;
                anim.Play("Damagereceive");
                break;
            case 2:
                tweenlength = anim.GetAnimation("Boom").Length;
                anim.Play("Boom");
                break;
        }
        var rand = new RandomNumberGenerator();
        var end_pos = new Vector3(rand.RandiRange(-spread,spread),height,rand.RandiRange(-spread, spread)) + Translation;
        tween.InterpolateProperty(this, "translation" ,Translation,end_pos , tweenlength , Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }

}
