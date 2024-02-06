using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class StateFactory<EState> where EState : Enum
{
    public Dictionary<EState, IState> States { get; protected set; }

    public IState this[EState state]
    {
        get { return States[state]; }
    }
}
