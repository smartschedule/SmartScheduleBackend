namespace SmartSchedule.Application.Friends.Queries.GetFriends
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

    public class GetFriendsListQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetFriendsListQuery()
        {

        }

        public GetFriendsListQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetFriendsListQuery, FriendsListResponse>
        {
            private readonly SmartScheduleDbContext _context;
            private readonly IMapper _mapper;

            public Handler(SmartScheduleDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetFriendsListQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var friendsList = await _context.Friends.Where(x => (x.FirstUserId.Equals(data.Id)
                                                             || x.SecoundUserId.Equals(data.Id))
                                                             && x.Type.Equals(Domain.Enums.FriendshipTypes.friends))
                                                             .Include(x => x.FirstUser)
                                                             .Include(x => x.SecoundUser)
                                                             .ToListAsync(cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in friendsList)
                {
                    var user = item.FirstUserId.Equals(data.Id) ? item.SecoundUser : item.FirstUser;
                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
