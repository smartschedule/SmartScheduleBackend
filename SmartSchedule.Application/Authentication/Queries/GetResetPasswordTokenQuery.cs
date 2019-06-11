﻿namespace SmartSchedule.Application.Authentication.Queries.GetResetPasswordToken
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Interfaces;

    public class GetResetPasswordTokenQuery : IRequest<string>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<GetResetPasswordTokenQuery, string>
        {
            private readonly IUnitOfWork _uow;
            private readonly IHttpContextAccessor _context;
            private readonly IJwtService _jwt;
            private readonly IEmailService _email;
            public Handler(IUnitOfWork uow, IHttpContextAccessor context, IJwtService jwt, IEmailService email)
            {
                _context = context;
                _uow = uow;
                _jwt = jwt;
                _email = email;
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
                await _email.SendEmail(user.Email, "Reset Password", uri.AbsoluteUri + "/" + token);

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
