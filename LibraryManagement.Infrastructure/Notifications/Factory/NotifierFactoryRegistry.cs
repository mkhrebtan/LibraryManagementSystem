using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Notifications.Factory.Factories;

namespace LibraryManagement.Infrastructure.Notifications.Factory;

internal static class NotifierFactoryRegistry
{
    private static Dictionary<NotificationPreference, NotifierFactory> _factories = new();

    public static void RegisterFactory(NotificationPreference preference, NotifierFactory factory)
    {
        if (_factories.ContainsKey(preference))
        {
            throw new InvalidOperationException($"Factory for {preference} is already registered.");
        }
        _factories[preference] = factory;
    }

    public static NotifierFactory GetFactory(NotificationPreference preference)
    {
        if (_factories.TryGetValue(preference, out var factory))
        {
            return factory;
        }
        throw new KeyNotFoundException($"No factory registered for {preference}.");
    }
}
