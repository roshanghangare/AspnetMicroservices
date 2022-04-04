using System.Threading.Tasks;
using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastructure
{
	interface IEmailService
	{
		Task<bool> SendEmail(Email email);
	}
}
