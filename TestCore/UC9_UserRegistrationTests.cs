using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestCore
{
    [TestFixture]
    public class UC9_ClientRegistrationTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private ClientService _service;

        [SetUp]
        public void Setup()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _service = new ClientService(_clientRepositoryMock.Object);
        }

        public static IEnumerable<TestCaseData> RegistrationCases
        {
            get
            {
                yield return new TestCaseData(
                    new Client(0, "Jane", "jane@example.com", "password123"),
                    true
                ).SetName("Nieuwe client wordt succesvol geregistreerd");

                yield return new TestCaseData(
                    new Client(0, "John", "john@example.com", "password1234"),
                    true
                ).SetName("Andere nieuwe client wordt succesvol geregistreerd");
            }
        }

        [TestCaseSource(nameof(RegistrationCases))]
        public void Register_NewClient_ShouldCallRepositoryAdd(Client client, bool expected)
        {
            _clientRepositoryMock.Setup(r => r.Get(client.EmailAddress)).Returns((Client?)null);

            _service.Register(client);

            _clientRepositoryMock.Verify(r => r.Add(It.Is<Client>(c =>
                c.EmailAddress == client.EmailAddress &&
                c.Password == client.Password)), Times.Once);
        }

        [Test]
        public void Register_ClientWithExistingEmail_ShouldThrow()
        {
            var existingClient = new Client(1, "Jane Doe", "jane@example.com", "password123");
            _clientRepositoryMock.Setup(r => r.Get(existingClient.EmailAddress)).Returns(existingClient);

            var newClient = new Client(0, "Another Jane", "jane@example.com", "newpass");

            Assert.Throws<InvalidOperationException>(() => _service.Register(newClient));
            _clientRepositoryMock.Verify(r => r.Add(It.IsAny<Client>()), Times.Never);
        }
    }
}
