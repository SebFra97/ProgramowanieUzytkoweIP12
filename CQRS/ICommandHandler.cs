namespace CQRS
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }
}
