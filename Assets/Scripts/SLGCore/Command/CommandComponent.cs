using System;
using System.Collections.Generic;
using System.Linq;

public class CommandComponent
{
    Queue<Command> commandQueue = new Queue<Command>();

    public void Exec()
    {
        while(commandQueue.Count > 0)
        {
            CommandExecResult result = commandQueue.Peek().Exec();
            switch(result)
            {
                case CommandExecResult.Finish:
                    commandQueue.Dequeue();
                    continue;
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
}