//Command Pattern to execute and undo
public interface ICommand
{
    void Execute();
    void Undo();
}