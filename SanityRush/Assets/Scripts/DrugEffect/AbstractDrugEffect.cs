using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class AbstractDrugEffect
{
    public DrugType Type { get; set; }

    public virtual void StartEffect()
    {
        //nothing
    }

    public virtual void UpdateEffect()
    {
        //nothing
    }

    public virtual void EndEffect()
    {
        //nothing
    }
}