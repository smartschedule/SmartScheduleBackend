﻿namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;
    using SmartSchedule.Persistence;

    public class GetUserFriendRequestsQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserFriendRequestsQuery()
        {

        }

        public GetUserFriendRequestsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetUserFriendRequestsQuery, FriendsListResponse>
        {
            private readonly SmartScheduleDbContext _context;
            private readonly IMapper _mapper;

            public Handler(SmartScheduleDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<FriendsListResponse> Handle(GetUserFriendRequestsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var friendRequestList = await _context.Friends.Where(x => (x.FirstUserId.Equals(data.Id)
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                                                             || (x.SecoundUserId.Equals(data.Id)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in friendRequestList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.pending_first_secound ?
                        item.SecoundUser : item.FirstUser;

                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
