using iwip.MongoDB;
using Xunit;

namespace iwip;

[CollectionDefinition(iwipTestConsts.CollectionDefinitionName)]
public class iwipDomainCollection : iwipMongoDbCollectionFixtureBase
{

}
