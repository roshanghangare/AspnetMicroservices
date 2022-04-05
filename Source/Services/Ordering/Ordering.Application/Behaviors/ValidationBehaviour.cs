using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviors
{
	public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		readonly IEnumerable<IValidator<TRequest>> validators;

		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			if (validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);
				var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
				var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();

				if (failures.Count != 0)
					throw new ValidationException(failures);
			}

			return await next();
		}
	}
}
