using System;
using System.Collections.Generic;
using System.Linq;

public class Unit
{
    public Place CurPlace { get; private set; }
    public Province NextProvince { get; private set; }
    public int MoveProgress { get; private set; }

    Queue<Command> commands = new Queue<Command>();

    public Unit(Place curPlace)
    {
        CurPlace = curPlace;
        CurPlace.AddStayUnit(this);
    }

    public void SetCurPlace(Place curPlace)
    {
        CurPlace.RemoveStayUnit(this);

        CurPlace = curPlace;
        CurPlace.AddStayUnit(this);

        NextProvince = null;
        MoveProgress = 0;
    }

    public void SetNextProvince(Province nextProvince)
    {
        NextProvince = nextProvince;
        MoveProgress = 0;
    }

    public void IncrementMoveProgress()
    {
        if(NextProvince != null)
        {
            MoveProgress++;
            if(NextProvince.GetMovementCost() == MoveProgress)
            {
                SetCurPlace(NextProvince);
            }
        }
    }

    public bool HaveCommand()
    {
        return commands.Count > 0;
    }

    public void ExecCommand()
    {
        while(HaveCommand())
        {
            CommandExecResult result = commands.Peek().Execute(this);
            if(result == CommandExecResult.Finish)
            {
                commands.Dequeue();
                continue;
            }
            else if(result == CommandExecResult.FinishAndBreak)
            {
                commands.Dequeue();
                break;
            }
            else if(result == CommandExecResult.Continue)
            {
                break;
            }
            else
            {
                Abort();
            }
        }
    }

    protected virtual void Abort()
    {
        commands.Clear();
    }

    public Command GetPeekCommand()
    {
        return commands.Peek();
    }

    public void EnqueueCommand(Command command)
    {
        commands.Enqueue(command);
    }
}