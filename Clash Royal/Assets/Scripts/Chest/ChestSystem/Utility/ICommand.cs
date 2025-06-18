//Command Pattern to execute and undo

namespace ChestSystem.Utility
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}

    