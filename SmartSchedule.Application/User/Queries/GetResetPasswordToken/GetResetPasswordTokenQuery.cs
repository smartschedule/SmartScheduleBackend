namespace SmartSchedule.Application.User.Queries.GetResetPasswordToken
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces;

    public class GetResetPasswordTokenQuery : IRequest<string>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<GetResetPasswordTokenQuery, string>
        {
            private IUnitOfWork _uow;
            private IHttpContextAccessor _context;
            private IJwtService _jwt;
            public Handler(IUnitOfWork uow, IHttpContextAccessor context, IJwtService jwt)
            {
                _context = context;
                _uow = uow;
                _jwt = jwt;
            }
            public async Task<string> Handle(GetResetPasswordTokenQuery request, CancellationToken cancellationToken)
            {
                var uri = GetAbsoluteUri();
                var user = await _uow.UsersRepository.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

                if (user == null)
                {
                    throw new NotFoundException("GetResetPasswordTokenQuery", request.Email);
                }
                string token = _jwt.GenerateJwtToken(user.Email, user.Id, false, true).Token;

                return uri.AbsoluteUri + "/" + token;
            }

            private Uri GetAbsoluteUri()
            {
                var request = _context.HttpContext.Request;
                UriBuilder uriBuilder = new UriBuilder();
                uriBuilder.Scheme = request.Scheme;
                uriBuilder.Host = request.Host.Host;
                uriBuilder.Path = request.Path.ToString();
                uriBuilder.Query = request.QueryString.ToString();

                return uriBuilder.Uri;
            }
        }
    }

}
