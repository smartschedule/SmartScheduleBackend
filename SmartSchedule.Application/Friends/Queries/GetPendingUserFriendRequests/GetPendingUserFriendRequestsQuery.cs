﻿namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Application.Interfaces.UoW;

    public class GetPendingUserFriendRequestsQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetPendingUserFriendRequestsQuery()
        {

        }

        public GetPendingUserFriendRequestsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetPendingUserFriendRequestsQuery, FriendsListResponse>
        {
            private readonly IUnitOfWork _context;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetPendingUserFriendRequestsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var pendingList = await _context.Friends.Where(x => (x.FirstUserId.Equals(data.Id)
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first))
                                                             || (x.SecoundUserId.Equals(data.Id)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in pendingList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.pending_secound_first ?
                        item.SecoundUser : item.FirstUser;

                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
