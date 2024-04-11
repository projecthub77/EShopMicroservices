using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.CQRS
{
    //-------------------------------- Gestori centralizzati -----------------------------------------------------

    public interface ICommandHandler<in TCommand>  // gestore del comando per risposte nulle (Unit)
        : ICommandHandler<TCommand, Unit> 
        where TCommand : ICommand<Unit>
    {

    }

    public interface ICommandHandler<in TCommand, TResponse> // gestore del comando per ricevere risposte
        : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse> 
        where TResponse : notnull
    {

    }
}
