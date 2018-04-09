using System;
using System.Collections.Generic;
using System.Linq;

public class CommandComponent : Component
{
    Queue<Command> commandQueue = new Queue<Command>();

    public void Exec()
    {
        if(commandQueue.Count > 0)
        {
            CommandExecResult result = commandQueue.Peek().Exec(this);
            switch(result)
            {
                case CommandExecResult.Finish:
                    commandQueue.Dequeue();
                    break;
                case CommandExecResult.FinishAndBreak:
                    commandQueue.Dequeue();
                    break;
                case CommandExecResult.Continue:
                    break;
                case CommandExecResult.Abort:
                    commandQueue.Dequeue();
                    break;
            }
        }
    }

    public bool HaveCommand()
    {
        return commandQueue.Count > 0;
    }

    public void AddCommand(Command command)
    {
        commandQueue.Enqueue(command);
    }

    public Command Peek()
    {
        return commandQueue.Peek();
    }
}