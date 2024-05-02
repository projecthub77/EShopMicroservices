using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> 
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>                // In questas class si implementa l'interfaccia del comportamento della pipeline e si controllano gli evetuali errori
        where TRequest : ICommand<TResponse>                    // di validazione nella richiesta in arrivo. Per raccogliere tutti gli handlers si utilizza IEnumerable di IValidator
    {                                                           // per convalidare tutti i validatori in un unico posto, che è il comportanemto del validatore.
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context,cancellationToken)));

            var failures = validationResults
                            .Where(r => r.Errors.Any())
                            .SelectMany(r => r.Errors).ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next();                            // alla fine passiamo al gestore del delegato della richiesta successiva per continuare la pipeline.
        }
    }
}
