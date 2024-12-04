using NUnit.Framework;
using Moq;
using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.TestsIES.Services
{
    [TestFixture]
    public class CampaignServiceTests
    {
        private Mock<ICampaignRepository> _campaignRepositoryMock;
        private CampaignService _campaignService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the mock repository before each test
            _campaignRepositoryMock = new Mock<ICampaignRepository>();
            _campaignService = new CampaignService(_campaignRepositoryMock.Object);
        }

        [Test]
        public void AddCampaign_ValidCampaign_AddsCampaignSuccessfully()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "New Campaign",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                    new CampaignStageModel { Name = "Stage 2", RequireMasterTask = true, MasterTaskDescription = "Master Task 2", IsConversionStage = true },
                }
            };
            int numberOfStages = 2;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNull(result, "Expected no validation errors for a valid campaign.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.Is<CampaignModel>(c => c == campaign)), Times.Once, "Expected AddCampaign to be called once with the valid campaign.");
        }

        [Test]
        public void AddCampaign_MissingCampaignName_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "", // Missing name
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                }
            };
            int numberOfStages = 1;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for missing campaign name.");
            Assert.Contains("Please enter a campaign name.", result, "Expected specific error message for missing campaign name.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_IncorrectNumberOfStages_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with insufficient stages",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                }
            };
            int numberOfStages = 2; // Expected 2 stages, provided 1

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for incorrect number of stages.");
            Assert.Contains("Please define all stages.", result, "Expected specific error message for incorrect number of stages.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_StageNameTooLong_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with long stage name",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                    new CampaignStageModel { Name = "This stage name is definitely longer than twenty characters", RequireMasterTask = false },
                }
            };
            int numberOfStages = 2;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for stage names exceeding 20 characters.");
            Assert.Contains("Stage names must be 20 characters or fewer.", result, "Expected specific error message for long stage names.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_DuplicateStageNames_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with duplicate stage names",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                }
            };
            int numberOfStages = 2;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for duplicate stage names.");
            Assert.Contains("Stage names must be unique.", result, "Expected specific error message for duplicate stage names.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_MultipleConversionStages_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with multiple conversion stages",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false, IsConversionStage = true },
                    new CampaignStageModel { Name = "Stage 2", RequireMasterTask = false, IsConversionStage = true },
                }
            };
            int numberOfStages = 2;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for multiple conversion stages.");
            Assert.Contains("Only one stage can be marked as the conversion stage.", result, "Expected specific error message for multiple conversion stages.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_MissingMasterTaskDescription_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with missing master task description",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = true, MasterTaskDescription = "" }, // Missing description
                }
            };
            int numberOfStages = 1;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for missing master task description.");
            Assert.Contains("Please provide a master task description for stage 'Stage 1'.", result, "Expected specific error message for missing master task description.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }

        [Test]
        public void AddCampaign_StageRequiresMasterTaskWithDescription_AddsCampaignSuccessfully()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with master task",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = true, MasterTaskDescription = "Master Task 1" },
                }
            };
            int numberOfStages = 1;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNull(result, "Expected no validation errors when master task description is provided.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.Is<CampaignModel>(c => c == campaign)), Times.Once, "Expected AddCampaign to be called once with the valid campaign.");
        }

        [Test]
        public void AddCampaign_NonUniqueStageNames_ReturnsError()
        {
            // Arrange
            var campaign = new CampaignModel
            {
                Name = "Campaign with non-unique stage names",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false },
                    new CampaignStageModel { Name = "Stage 2", RequireMasterTask = false },
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = false }, // Duplicate name
                }
            };
            int numberOfStages = 3;

            // Act
            var result = _campaignService.AddCampaign(campaign, numberOfStages);

            // Assert
            Assert.IsNotNull(result, "Expected validation errors for non-unique stage names.");
            Assert.Contains("Stage names must be unique.", result, "Expected specific error message for non-unique stage names.");
            _campaignRepositoryMock.Verify(r => r.AddCampaign(It.IsAny<CampaignModel>()), Times.Never, "Expected AddCampaign not to be called when validation fails.");
        }
    }
}
