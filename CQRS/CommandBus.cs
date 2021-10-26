using System;

namespace CQRS
{
    public class CommandBus
    {
        private readonly IServiceProvider container;

        public CommandBus(IServiceProvider container)
        {
            this.container = container;
        }

        public void Handle<T>(T command) where T : ICommand
        {
            ICommandHandler<T> commandHandler = container.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;


            if (commandHandler != null)
            {
                commandHandler.Handle(command);
            }
            else throw new Exception("Metoda " + typeof(T).Name + " jest nieobsłużona.");
        }
    }
}
