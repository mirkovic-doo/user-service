using Cassandra.Mapping;
using UserService.Domain;

namespace UserService.Infrastructure;

public class DbMappingProfile : Mappings
{
    public DbMappingProfile()
    {
        For<User>()
                .TableName("users")
                .PartitionKey(u => u.Id)
                .ClusteringKey(u => u.FirebaseId)
                .ClusteringKey(u => u.Username)
                .ClusteringKey(u => u.Email)
                .Column(u => u.Id, cm => cm.WithName("id"))
                .Column(u => u.FirebaseId, cm => cm.WithName("firebase_id"))
                .Column(u => u.Username, cm => cm.WithName("username"))
                .Column(u => u.Email, cm => cm.WithName("email"))
                .Column(u => u.FirstName, cm => cm.WithName("first_name"))
                .Column(u => u.LastName, cm => cm.WithName("last_name"))
                .Column(u => u.Country, cm => cm.WithName("country"))
                .Column(u => u.City, cm => cm.WithName("city"))
                .Column(u => u.PostCode, cm => cm.WithName("post_code"))
                .Column(u => u.Street, cm => cm.WithName("street"))
                .Column(u => u.HouseNumber, cm => cm.WithName("house_number"))
                .Column(u => u.IsGuest, cm => cm.WithName("is_guest"));
    }
}
