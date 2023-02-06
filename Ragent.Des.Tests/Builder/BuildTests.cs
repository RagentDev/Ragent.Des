using Ragent.Des.Tests.Builder.Interfaces;
using Ragent.Des.Tests.Builder.Services;

namespace Ragent.Des.Tests.Builder;

[TestFixture]
public class BuildTests
{

    [SetUp]
    public void SetUp()
    {
        
    }

    [Test]
    public void AddServiceWithType()
    {
        var manager = new DesBuilder()
            .AddService<BasicService>()
            .Build();

        var service = manager.GetService<BasicService>();

        Assert.That(service.GetScore(), Is.EqualTo(0));
    }

    [Test]
    public void AddServiceObjectWithType()
    {
        var manager = new DesBuilder()
            .AddService<BasicService>(new BasicService(10))
            .Build();

        var service = manager.GetService<BasicService>();

        Assert.That(service.GetScore(), Is.EqualTo(10));
    }

    [Test]
    public void AddInterfaceServiceWithType()
    {
        var manager = new DesBuilder()
            .AddService<IBasicService, BasicService>()
            .Build();

        var service = manager.GetService<IBasicService>();

        Assert.That(service.GetScore(), Is.EqualTo(0));
    }

    [Test]
    public void AddInterfaceServiceObjectWithType()
    {
        var manager = new DesBuilder()
            .AddService<IBasicService, BasicService>(new BasicService(10))
            .Build();

        var service = manager.GetService<IBasicService>();

        Assert.That(service.GetScore(), Is.EqualTo(10));
    }

    [Test]
    public void AddSameInterfaceAndObject()
    {
        try
        {
            var manager = new DesBuilder()
                .AddService<BasicService>()
                .AddService<BasicService>(new BasicService(10))
                .AddService<IBasicService, BasicService>()
                .AddService<IBasicService, BasicService>(new BasicService(20))
                .Build();

            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.That(ex, Is.InstanceOf<Exception>());
        }
    }
    
}