using NUnit.Framework;
using Moq;
using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.TestsIES.Services
{
    [TestFixture]
    public class PipelineServiceTests
    {
        private Mock<IPipelineRepository> _pipelineRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<ICampaignRepository> _campaignRepositoryMock;
        private Mock<IActivityLogRepository> _activityLogRepositoryMock;
        private ContactService _contactService;
        private CampaignService _campaignService;
        private PipelineService _pipelineService;

        [SetUp]
        public void SetUp()
        {
            _pipelineRepositoryMock = new Mock<IPipelineRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _campaignRepositoryMock = new Mock<ICampaignRepository>();
            _activityLogRepositoryMock = new Mock<IActivityLogRepository>();

            _contactService = new ContactService(_contactRepositoryMock.Object, _activityLogRepositoryMock.Object);
            _campaignService = new CampaignService(_campaignRepositoryMock.Object);

            // Instantiate the PipelineService with real service instances and mocked repositories
            _pipelineService = new PipelineService(
                _pipelineRepositoryMock.Object,
                _contactRepositoryMock.Object,
                _campaignRepositoryMock.Object,
                _contactService,
                _campaignService);
        }

        [Test]
        public void UpdatePipelineStage_ValidUpdate_UpdatesSuccessfully()
        {
            // Arrange
            int pipelineId = 1;
            string newStageName = "Stage 2";

            var existingPipeline = new PipelineModel
            {
                Id = pipelineId,
                CampaignId = 100,
                ContactId = 200,
                ActiveStage = "Stage 1",
                Status = "Active"
            };

            var campaign = new CampaignModel
            {
                Id = 100,
                Name = "Test Campaign",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", IsConversionStage = true },
                    new CampaignStageModel { Name = "Stage 2", IsConversionStage = false }
                }
            };

            var contact = new Contact
            {
                Id = 200,
                Name = "John Doe",
                Type = ContactType.Lead
            };

            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns(existingPipeline);
            _campaignRepositoryMock.Setup(r => r.GetCampaign(100)).Returns(campaign);
            _contactRepositoryMock.Setup(r => r.GetContact(200)).Returns(contact);
            _pipelineRepositoryMock.Setup(r => r.UpdatePipeline(It.IsAny<PipelineModel>()));
            _contactRepositoryMock.Setup(r => r.UpdateContact(It.IsAny<Contact>()));

            // Act
            _pipelineService.UpdatePipelineStage(pipelineId, newStageName);

            // Assert
            Assert.AreEqual(newStageName, existingPipeline.ActiveStage, "ActiveStage should be updated to the new stage.");
            _pipelineRepositoryMock.Verify(r => r.UpdatePipeline(It.Is<PipelineModel>(p => p == existingPipeline)), Times.Once, "UpdatePipeline should be called once with the updated pipeline.");
            // Since previous stage was a conversion stage and contact was a lead, contact type should be updated
            Assert.AreEqual(ContactType.Customer, contact.Type, "Contact type should be updated to Customer.");
            _contactRepositoryMock.Verify(r => r.UpdateContact(It.Is<Contact>(c => c == contact)), Times.Once, "UpdateContact should be called once with the updated contact.");
        }

        [Test]
        public void UpdatePipelineStage_PipelineNotFound_ThrowsException()
        {
            // Arrange
            int pipelineId = 1;
            string newStageName = "Stage 2";

            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns((PipelineModel)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _pipelineService.UpdatePipelineStage(pipelineId, newStageName));
            Assert.AreEqual("Pipeline not found", ex.Message);
            _pipelineRepositoryMock.Verify(r => r.UpdatePipeline(It.IsAny<PipelineModel>()), Times.Never, "UpdatePipeline should not be called when pipeline is not found.");
        }

        [Test]
        public void CreateNewPipeline_InvalidCampaignOrContact_ThrowsArgumentException()
        {
            // Arrange
            var pipeline = new PipelineModel
            {
                CampaignId = 0, // Invalid
                ContactId = 0    // Invalid
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _pipelineService.CreateNewPipeline(pipeline));
            Assert.AreEqual("Invalid Campaign or Contact selection.", ex.Message);
            _pipelineRepositoryMock.Verify(r => r.AddPipeline(It.IsAny<PipelineModel>()), Times.Never, "AddPipeline should not be called when validation fails.");
        }

        [Test]
        public void CreateNewPipeline_ValidPipeline_AddsSuccessfully()
        {
            // Arrange
            var pipeline = new PipelineModel
            {
                CampaignId = 100,
                ContactId = 200
            };

            var campaign = new CampaignModel
            {
                Id = 100,
                Name = "Test Campaign",
                Stages = new List<CampaignStageModel>
                {
                    new CampaignStageModel { Name = "Stage 1", RequireMasterTask = true, MasterTaskDescription = "Master Task 1" },
                    new CampaignStageModel { Name = "Stage 2", RequireMasterTask = false }
                }
            };

            _campaignService = new CampaignService(_campaignRepositoryMock.Object);
            _campaignRepositoryMock.Setup(s => s.GetCampaign(100)).Returns(campaign);
            _pipelineRepositoryMock.Setup(r => r.AddPipeline(It.IsAny<PipelineModel>()))
                .Callback<PipelineModel>(p => p.Id = 1); // Simulate setting the ID

            // Act
            var result = _pipelineService.CreateNewPipeline(pipeline);

            // Assert
            Assert.AreEqual(1, result, "Pipeline ID should be set after creation.");
            Assert.AreEqual("Stage 1", pipeline.ActiveStage, "ActiveStage should be initialized to the first campaign stage.");
            Assert.AreEqual(1, pipeline.Tasks.Count, "One master task should be created.");
            _pipelineRepositoryMock.Verify(r => r.AddPipeline(It.Is<PipelineModel>(p => p == pipeline)), Times.Once, "AddPipeline should be called once with the valid pipeline.");
        }

        [Test]
        public void ToggleStage_PipelineNotActive_ThrowsInvalidOperationException()
        {
            // Arrange
            int pipelineId = 1;
            string targetStage = "Stage 2";

            var pipeline = new PipelineModel
            {
                Id = pipelineId,
                Status = "Closed",
                ActiveStage = "Stage 1",
                CampaignId = 100,
                ContactId = 200
            };

            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns(pipeline);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _pipelineService.ToggleStage(pipelineId, targetStage));
            Assert.AreEqual("Cannot change stages of a pipeline that is not active.", ex.Message);
            _pipelineRepositoryMock.Verify(r => r.UpdatePipeline(It.IsAny<PipelineModel>()), Times.Never, "UpdatePipeline should not be called when pipeline is not active.");
        }

        [Test]
        public void ToggleTaskComplete_ValidTask_TogglesCompletion()
        {
            // Arrange
            var task = new TaskModel
            {
                Id = 1,
                PipelineId = 100,
                IsCompleted = false
            };

            var existingTask = new TaskModel
            {
                Id = 1,
                PipelineId = 100,
                IsCompleted = false
            };

            var pipeline = new PipelineModel
            {
                Id = 100,
                Status = "Active"
            };

            _pipelineRepositoryMock.Setup(r => r.GetTaskById(1)).Returns(existingTask);
            _pipelineRepositoryMock.Setup(r => r.GetPipeline(100)).Returns(pipeline);
            _pipelineRepositoryMock.Setup(r => r.UpdateTask(It.IsAny<TaskModel>()));

            // Act
            _pipelineService.ToggleTaskComplete(task);

            // Assert
            Assert.IsTrue(existingTask.IsCompleted, "Task should be marked as completed.");
            _pipelineRepositoryMock.Verify(r => r.UpdateTask(It.Is<TaskModel>(t => t == existingTask)), Times.Once, "UpdateTask should be called once with the updated task.");
        }

        [Test]
        public void AddTask_InvalidDescription_ThrowsArgumentException()
        {
            // Arrange
            string description = " ";
            DateTime createdDate = DateTime.Today;
            DateTime deadline = DateTime.Today.AddDays(7);
            int pipelineId = 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _pipelineService.AddTask(description, createdDate, deadline, pipelineId));
            Assert.AreEqual("Task description cannot be empty.", ex.Message);
            _pipelineRepositoryMock.Verify(r => r.AddTask(It.IsAny<TaskModel>()), Times.Never, "AddTask should not be called when description is invalid.");
        }

        [Test]
        public void AddTask_PipelineNotActive_ThrowsInvalidOperationException()
        {
            // Arrange
            string description = "New Task";
            DateTime createdDate = DateTime.Today;
            DateTime deadline = DateTime.Today.AddDays(7);
            int pipelineId = 1;

            var pipeline = new PipelineModel
            {
                Id = pipelineId,
                Status = "Closed"
            };

            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns(pipeline);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _pipelineService.AddTask(description, createdDate, deadline, pipelineId));
            Assert.AreEqual("Cannot add tasks to a pipeline that is not active.", ex.Message);
            _pipelineRepositoryMock.Verify(r => r.AddTask(It.IsAny<TaskModel>()), Times.Never, "AddTask should not be called when pipeline is not active.");
        }

        [Test]
        public void AddTask_ValidTask_AddsSuccessfully()
        {
            // Arrange
            string description = "New Task";
            DateTime createdDate = DateTime.Today;
            DateTime deadline = DateTime.Today.AddDays(7);
            int pipelineId = 1;

            var pipeline = new PipelineModel
            {
                Id = pipelineId,
                Status = "Active",
                ActiveStage = "Stage 1"
            };

            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns(pipeline);
            _pipelineRepositoryMock.Setup(r => r.AddTask(It.IsAny<TaskModel>()));

            // Act
            _pipelineService.AddTask(description, createdDate, deadline, pipelineId);

            // Assert
            _pipelineRepositoryMock.Verify(r => r.AddTask(It.Is<TaskModel>(t =>
                t.Description == description &&
                t.CreatedDate == createdDate &&
                t.Deadline == deadline &&
                t.IsMasterTask == false &&
                t.Stage == "Stage 1" &&
                t.IsCompleted == false &&
                t.PipelineId == pipelineId
            )), Times.Once, "AddTask should be called once with the correct task details.");
        }

        [Test]
        public void EndPipelineWithWin_PipelineAtLastStageWithCompletedMasterTask_EndsSuccessfully()
        {
            // Arrange
            int pipelineId = 1;
            var pipeline = new PipelineModel
            {
                Id = pipelineId,
                CampaignId = 100,
                ContactId = 200,
                ActiveStage = "Stage 2",
                Status = "Active",
                Tasks = new List<TaskModel>
        {
            new TaskModel { Id = 1, Stage = "Stage 2", IsMasterTask = true, IsCompleted = true }
        }
            };

            var campaign = new CampaignModel
            {
                Id = 100,
                Name = "Test Campaign",
                Stages = new List<CampaignStageModel>
        {
            new CampaignStageModel { Name = "Stage 1", IsConversionStage = false },
            new CampaignStageModel { Name = "Stage 2", IsConversionStage = false }
        }
            };

            // Setup repository to return pipeline and campaign
            _pipelineRepositoryMock.Setup(r => r.GetPipeline(pipelineId)).Returns(pipeline);
            _campaignRepositoryMock.Setup(r => r.GetCampaign(100)).Returns(campaign);
            _pipelineRepositoryMock.Setup(r => r.UpdatePipeline(It.IsAny<PipelineModel>()));

            // Act
            _pipelineService.EndPipelineWithWin(pipelineId);

            // Assert
            Assert.AreEqual("Won", pipeline.Status, "Pipeline status should be updated to 'Won'.");
            _pipelineRepositoryMock.Verify(r => r.UpdatePipeline(It.Is<PipelineModel>(p => p == pipeline)), Times.Once, "UpdatePipeline should be called once with the updated pipeline.");
        }
    }
}
