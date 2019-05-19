namespace SmartSchedule.Test.Calendars
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shouldly;
    using SmartSchedule.Application.Calendar.Commands.UpdateCalendar;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;
    using SmartSchedule.Test.Infrastructure;
    using Xunit;

    [Collection("TestCollection")]
    public class UpdateCalendarCommandTests
    {
        private readonly IUnitOfWork _uow;

        public UpdateCalendarCommandTests(TestFixture fixture)
        {
            _uow = fixture.Context;
        }

        [Fact]
        public async Task UpdateCalendarShouldUpdateCalendarInDbContext()
        {
            var requestData = new UpdateCalendarRequest
            {
                Id = 2,
                Name = "kalendarz2",
                ColorHex = "#aabbcc"
            };
            var command = new UpdateCalendarCommand(requestData);
       
            var commandHandler = new UpdateCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None);

            var calendar = await _uow.CalendarsRepository.GetByIdAsync(2);

            calendar.Name.ShouldBe(command.Data.Name);
            calendar.ColorHex.ShouldBe(command.Data.ColorHex);
        }

        [Fact]
        public async Task UpdateCalendarProvidingEmptyDataShouldNotUpdateCalendarInDbContext()
        {
            var requestData = new UpdateCalendarRequest
            {
                Id = 2,
                Name = "",
                ColorHex = "#aabbcc"
            };
            var command = new UpdateCalendarCommand(requestData);

            var commandHandler = new UpdateCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task UpdateCalendarProvidingNotExistindIdShouldNotUpdateCalendarInDbContext()
        {
            var requestData = new UpdateCalendarRequest
            {
                Id = 20000,
                Name = "",
                ColorHex = "#aabbcc"
            };
            var command = new UpdateCalendarCommand(requestData);

            var commandHandler = new UpdateCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }

        [Theory]
        [InlineData("#fffffz")]
        [InlineData("ffffff")]
        [InlineData("fff")]
        [InlineData("#0123456")]
        [InlineData("#01234")]
        [InlineData("#0123")]
        [InlineData("#01")]
        [InlineData("#0")]
        public async Task UpdateCalendarProvidingWrongColorShouldNotUpdateCalendarInDbContextProvidingWrongColor(string color)
        {
            var requestData = new UpdateCalendarRequest
            {
                Id = 2,
                Name = "testowanazwa",
                ColorHex = color
            };
            var command = new UpdateCalendarCommand(requestData);

            var commandHandler = new UpdateCalendarCommand.Handler(_uow);

            await commandHandler.Handle(command, CancellationToken.None).ShouldThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
