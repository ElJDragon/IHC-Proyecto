using GestionIncidentes.Application.EventHandlers;
using GestionIncidentes.Application.Features.Knowledge;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using Moq;
using Xunit;

namespace GestionIncidentes.Tests.ApplicationTests;

public class KnowledgeAndNotificationTests
{
    [Fact]
    public async Task SearchKnowledge_ShouldIncrementUsageCount()
    {
        // Arrange
        var mockRepo = new Mock<IKnowledgeRepository>();
        var entries = new List<KnowledgeEntry>
        {
            KnowledgeEntry.Create("Test", "Problem", "Solution", "Hardware", Guid.NewGuid())
        };
        
        mockRepo.Setup(r => r.SearchAsync(It.IsAny<string>()))
                .ReturnsAsync(entries);
        
        var handler = new SearchKnowledgeQueryHandler(mockRepo.Object);
        var query = new SearchKnowledgeQuery("Test");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        mockRepo.Verify(r => r.UpdateAsync(It.IsAny<KnowledgeEntry>()), Times.Once);
    }

    [Fact]
    public async Task OnIncidentReported_ShouldCreateNotifications()
    {
        // Arrange
        var mockNotifRepo = new Mock<INotificationRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        
        var diticUser = User.Create("ditic@test.com", "pass", "DITIC User", Guid.NewGuid(), "DITIC");
        mockUserRepo.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<User> { diticUser });

        var handler = new OnIncidentReportedHandler(mockNotifRepo.Object, mockUserRepo.Object);
        var @event = new IncidentReported(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        mockNotifRepo.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public void KnowledgeEntry_ShouldIncrementUsage()
    {
        // Arrange
        var entry = KnowledgeEntry.Create("Title", "Problem", "Solution", "Software", Guid.NewGuid());
        var initialCount = entry.UsageCount;

        // Act
        entry.IncrementUsage();

        // Assert
        Assert.Equal(initialCount + 1, entry.UsageCount);
    }

    [Fact]
    public void Notification_ShouldMarkAsRead()
    {
        // Arrange
        var notification = Notification.Create(Guid.NewGuid(), "Title", "Message", "Type");
        Assert.False(notification.IsRead);

        // Act
        notification.MarkAsRead();

        // Assert
        Assert.True(notification.IsRead);
    }

    [Fact]
    public void AuditLog_ShouldCreateWithAllProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entityId = Guid.NewGuid();

        // Act
        var log = AuditLog.Create(userId, "TestAction", "TestEntity", entityId, "details", "127.0.0.1");

        // Assert
        Assert.Equal(userId, log.UserId);
        Assert.Equal("TestAction", log.Action);
        Assert.Equal("TestEntity", log.EntityType);
        Assert.Equal(entityId, log.EntityId);
        Assert.Equal("details", log.Details);
        Assert.Equal("127.0.0.1", log.IpAddress);
    }
}
