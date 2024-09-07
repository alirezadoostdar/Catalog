namespace Catalog.Infrastructure.IntegrationEvents;

public record CatalogRemindMessage(Guid UserId,string Slug,string Message,NotifyChannel Channel);

public enum NotifyChannel
{
    SMS = 1,
    Email = 2,
    MsTeams = 3,
    Telegram = 4
}
