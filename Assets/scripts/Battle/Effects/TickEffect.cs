using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TickEffect : BattleEffect
{
    public int duration;

    public int maxDuration;

    private void Tick()
    {
        duration--;

        Activate();

        if(duration <= 0)
        {
            owner.RemoveEffect(this);
        }
    }

    protected virtual void Activate()
    {

    }

    public TickEffect(Character owner): base(owner)
    {
        owner.TurnStart += Owner_TurnStart;
        this.duration = maxDuration;
    }

    private void Owner_TurnStart(object sender, EventArgs e)
    {
        Tick();
    }
}

