namespace SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests
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
    using SmartSchedule.Application.DAL.Interfaces.UoW;

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
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetPendingUserFriendRequestsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var pendingList = await _uow.FriendsRepository.GetPendingFriends(data.Id, cancellationToken);
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
