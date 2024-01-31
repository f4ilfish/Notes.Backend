using MediatR;
using FluentValidation;

namespace Notes.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validatords;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validatords = validators;

        public Task<TResponse> Handle(TRequest request, 
            CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) 
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validatords
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
            return next();
        }
    }
}
