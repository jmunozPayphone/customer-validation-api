using CustomerValidation.ApplicationCore.Enums;
using CustomerValidation.ApplicationCore.Features.Customers.Commands;
using CustomerValidation.ApplicationCore.Features.Customers.Queries;
using CustomerValidation.ApplicationCore.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerValidation.ApplicationCore.UnitTests.Features.Customers.Commands;

public class AssessCustomerRiskCommandHandlerTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenMediatorIsNull()
    {
        // Arrange
        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AssessCustomerRiskCommandHandler(null!, logger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenLoggerIsNull()
    {
        // Arrange
        var mediator = new Mock<IMediator>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AssessCustomerRiskCommandHandler(mediator.Object, null!));
    }

    [Fact]
    public async Task Handle_ShouldReturnRejected_WhenCustomerScoreIsNull()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        _ = mediator.Setup(m => m.Send(It.IsAny<GetCustomerCreditScoreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCustomerCreditScoreResult?)null);

        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();
        var handler = new AssessCustomerRiskCommandHandler(mediator.Object, logger.Object);

        var command = new AssessCustomerRiskCommand("John Doe", new Document("1234567890"), new Amount(500));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(CustomerScoreStatus.Rejected), result.Value.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnRejected_WhenScoreIsBelow500()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCustomerCreditScoreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCustomerCreditScoreResult(400));

        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();
        var handler = new AssessCustomerRiskCommandHandler(mediator.Object, logger.Object);

        var command = new AssessCustomerRiskCommand("John Doe", new Document("1234567890"), new Amount(500));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(nameof(CustomerScoreStatus.Rejected), result.Value.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnApproved_WhenScoreBetween500And699_AndTxAmountBelow1000()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCustomerCreditScoreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCustomerCreditScoreResult(650));

        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();
        var handler = new AssessCustomerRiskCommandHandler(mediator.Object, logger.Object);

        var command = new AssessCustomerRiskCommand("Jane Doe", new Document("1234567890"), new Amount(999));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(nameof(CustomerScoreStatus.Approved), result.Value.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnRejected_WhenScoreBetween500And699_AndTxAmount1000OrMore()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCustomerCreditScoreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCustomerCreditScoreResult(650));

        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();
        var handler = new AssessCustomerRiskCommandHandler(mediator.Object, logger.Object);

        var command = new AssessCustomerRiskCommand("Jane Doe", new Document("1234567890"), new Amount(10000));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(nameof(CustomerScoreStatus.Rejected), result.Value.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnApproved_WhenScore700OrAbove()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetCustomerCreditScoreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCustomerCreditScoreResult(750));

        var logger = new Mock<ILogger<AssessCustomerRiskCommandHandler>>();
        var handler = new AssessCustomerRiskCommandHandler(mediator.Object, logger.Object);

        var command = new AssessCustomerRiskCommand("Max Power", new Document("1234567890"), new Amount(5000));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(nameof(CustomerScoreStatus.Approved), result.Value.Status);
    }
}
