namespace SmartSchedule.Application.Friends.Queries.GetBlockedUsers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;

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
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetBlockedUsersListQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var blockedList = await _uow.FriendsRepository.GetBlockedUsers(data.Id, cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in blockedList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.block_first_second ?
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
