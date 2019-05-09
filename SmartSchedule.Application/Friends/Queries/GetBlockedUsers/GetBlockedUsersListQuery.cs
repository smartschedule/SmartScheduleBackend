namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
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

    public class GetBlockedUsersListQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetBlockedUsersListQuery()
        {

        }

        public GetBlockedUsersListQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetBlockedUsersListQuery, FriendsListResponse>
        {
            private readonly SmartScheduleDbContext _context;
            private readonly IMapper _mapper;

            public Handler(SmartScheduleDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetBlockedUsersListQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var blockedList = await _context.Friends.Where(x => (x.FirstUserId.Equals(data.Id)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_first_secound)
                                                             || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both)))
                                                             || (x.SecoundUserId.Equals(data.Id)
                                                             && (x.Type.Equals(Domain.Enums.FriendshipTypes.block_scound_first)
                                                             || x.Type.Equals(Domain.Enums.FriendshipTypes.block_both))))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in blockedList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.block_first_secound ?
                        item.SecoundUser : item.FirstUser;
                    if (item.Type.Equals(Domain.Enums.FriendshipTypes.block_both))
                    {
                        user = item.FirstUserId.Equals(data.Id) ? item.SecoundUser : item.FirstUser;
                    }
                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
