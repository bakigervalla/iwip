using iwip.MongoDB;
using Xunit;

namespace iwip;

[CollectionDefinition(iwipTestConsts.CollectionDefinitionName)]
public class iwipApplicationCollection : iwipMongoDbCollectionFixtureBase
{

}
