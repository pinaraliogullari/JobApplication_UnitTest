using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
using Xunit;
using static JobApplicationLibrary.ApplicationEvaluator;

namespace JobApplicationLibrary.UnitTest
{

    //MockBehaviour
    //var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Strict);
    //var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Loose);
    //var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Default);
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

            Assert.Equal(ApplicationResult.AutoRejected, result);
        }

        [Fact]

        public void Application_WhenNoTechStack_TransferredToAutoRejected()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("TÜRKİYE");


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19, IdentityNumber = "123" },
                TechStackList = new List<string> { "" }
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(ApplicationResult.AutoRejected, result);
        }

        [Fact]

        public void Application_WhenTechStackOver75Percent_TransferredToAutoAccepted()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("TÜRKİYE");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 45, IdentityNumber = "xyz" },
                TechStackList = new List<string> { "C#", "RabbitMQ", "Microservice", "VisualStudio" },
                YearsOfExperience = 16,
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(ApplicationResult.AutoAccepted, result);
        }

        [Fact]
        public void Application_WhenIdentityNumberIsInValid_TransferredToHR()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("TÜRKİYE");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 20 },
            };

            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(ApplicationResult.TransferredToHR, result);
        }

        [Fact]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            //assert
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("SPAIN");
           
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 20 },
            };
            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(ApplicationResult.TransferredToCTO, result);

        }

        [Fact]
        public void Application_WhenAgeIsOver50_ValidationModeToDetailse()
        {
            //assert
            var mockValidator = new Mock<IIdentityValidator>();
            //mockValidator.SetupAllProperties();// bu komut satırı ,özelleştirilmiş setup ayarının mutlakaüstünde olmalı 
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("SPAIN");
          
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 51 },
            };
            //act
            var result = evaluator.Evaluate(form);

            //assert
            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }

    }
}


