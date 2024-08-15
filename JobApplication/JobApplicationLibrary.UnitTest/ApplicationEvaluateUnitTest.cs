using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
using Xunit;
using static JobApplicationLibrary.ApplicationEvaluator;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {

        //UnitOfWork_Condition_ExpectedResult
        [Fact]
        public void Application_WhenAgeIsUnderMınAge_TransferredToAutoRejected()
        {
            //Arrange
            var evaluator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            //Act
            var result = evaluator.Evaluate(form);

            //Assert

            Assert.Equal(result, ApplicationResult.AutoRejected);
        }

        [Fact]

        public void Application_WhenNoTechStack_TransferredToAutoRejected()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19, IdentityNumber = "123" },
                TechStackList = new List<string> { "" }
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(result, ApplicationResult.AutoRejected);
        }

        [Fact]

        public void Application_WhenTechStackOver75Percent_TransferredToAutoAccepted()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 45, IdentityNumber = "xyz" },
                TechStackList = new List<string> { "C#", "RabbitMQ", "Microservice", "VisualStudio" },
                YearsOfExperience = 16
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(result, ApplicationResult.AutoAccepted);
        }

        [Fact]
        public void Application_WhenIdentityNumberIsInValid_TransferredToHR()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 20 }
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(result, ApplicationResult.TransferredToHR);
        }
    }
}
