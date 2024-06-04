using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(a => a.Entity);

            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            aggregates.ToList().ForEach(a => a.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}


//FLUSSO di Esempio:

//Order model ha il metodo Create che aggiunge alla coda degli eventi di dominio il suo evento di creazione di un ordine
//l'oggetto OrderCreatedEvent passato all'array di eventi, tramite il gestore OrderCreatedEventHandler che riceverà la notifica dell'evento.
//OrderCreatedEvent implementa IDomainEvent che implementa INotification cosicchè
//possiamo usare il metodo publish del Mediatr per inviare gli eventi di dominio all interceptor del livello infrastructure 
// nel dispatcher. Questo recupera gli eventi di dominio degli aggregati e li publica (publish) per distribuirli.
// una volta publicato un evento di dominio, si deve gestire nel livello di applicazione nell'event handlers in modo che da qui
//possiamo arrivare agli eventi di integrazione in modo da poter comunicare cambiamenti significativi tra i diversi microservizi
// o diversi context.


//dall'api abbiamo una CreateOrderRequest (attiva CreateorderCommand) ----> CreateOrderHandler ----> (Add) Order ---> (al salvataggio scatta l'interceptor) ---->
//DispatchCreateEventHandler ----> (pubblichiamo l'evento di dominio)
//dal publish nel dispatcher l'evento di dominio verra inviato a OrderCreatedEventHandler ne livello di applicazione (gestore dell'evento creato) -------> 
// ed è catturato dall'handle


//---------------------------------

//FanOut Publish/Subscribe pattern
//Event Driven architecture
//Event Drive microservices ----> architettura focalizzata sugli eventi come fonte primaria di comunicazione tra microservizi
//(maggiore disaccoppiamento, scalabilità e reattività)
//Outbox Pattern