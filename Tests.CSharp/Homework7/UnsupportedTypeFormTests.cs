using Hw7.Models;
using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.CSharp.Homework7.Shared;

namespace Tests.CSharp.Homework7
{
    public class UnsupportedTypeFormTests : IClassFixture<WebApplicationFactory<Hw7.Program>>
    {
        private readonly HttpClient _client;
        private readonly string _url = "/Test/UnsupportedModel";

        public UnsupportedTypeFormTests(WebApplicationFactory<Hw7.Program> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task PostUnsupportedForm_ModelWithRequiredProps_OnePropertyTypeIsNotSupported()
        {
            //arrange
            var model = new UnsupportedModel
            {
                FirstName = TestHelper.LongString,
                LastName = TestHelper.LongString,
                MiddleName = TestHelper.LongString,
                Age = 15,
                Sex = Sex.Male,
                Date = new DateOnly()
            };
            var response = await TestHelper.SendUnsupportedForm(_client, _url, model);

            //act
            var result = TestHelper.GetLabelForPropertyById(response, "Date") == null ? false : true;

            //assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetUnsupportedForm_ModelWithRequiredProps_OnePropertyTypeIsNotSupported()
        {
            //arrange
            var model = new UnsupportedModel
            {
                FirstName = TestHelper.LongString,
                LastName = TestHelper.LongString,
                MiddleName = TestHelper.LongString,
                Age = 15,
                Sex = Sex.Male,
                Date = new DateOnly()
            };
            var response = await TestHelper.GetFormHtml(_client, _url);

            //act
            var result = TestHelper.GetLabelForPropertyById(response, "Date") == null ? false : true;

            //assert
            Assert.True(result);
        }
    }
}
