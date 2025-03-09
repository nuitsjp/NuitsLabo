using System.Reflection;
using NTwain;
using NTwain.Data;

// can use the utility method to create appId or make one yourself
var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetExecutingAssembly());

// new it up and handle events
var session = new TwainSession(appId);

session.TransferReady += (sender, eventArgs) =>
{

};
session.DataTransferred += (sender, eventArgs) =>
{

};

// finally open it
session.Open();

var myDS = session.FirstOrDefault()!;