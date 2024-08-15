using FluentAssertions;
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
        public void Application_WhenAgeIsUnderMinAge_TransferredToAutoRejected()
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

            //Assert.Equal(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
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
            //Assert.Equal(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
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
            //Assert.Equal(ApplicationResult.AutoAccepted, result);
            result.Should().Be(ApplicationResult.AutoAccepted);
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
            //Assert.Equal(ApplicationResult.TransferredToHR, result);
            result.Should().Be(ApplicationResult.TransferredToHR);
        }

        [Fact]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            //arrange
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
            //Assert.Equal(ApplicationResult.TransferredToCTO, result);
            result.Should().Be(ApplicationResult.TransferredToCTO);

        }

        [Fact]
        public void Application_WhenAgeIsOver50_ValidationModeToDetailse()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.SetupAllProperties();// bu komut satırı ,özelleştirilmiş setup ayarının mutlakaüstünde olmalı 
            mockValidator.Setup(x => x.CountryDataProvider.CountryData.Country).Returns("SPAIN");
          
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 51 },
            };
            //act
            var result = evaluator.Evaluate(form);

            //assert
            //Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
            mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
        }

        [Fact]
        public void Application_WhenApplicantIsNull_ThrowsArgumentNullException()
        {
            //arrange
            var mockValidator = new Mock<IIdentityValidator>();
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication();

            //act
            var result = FluentActions.Invoking(() => evaluator.Evaluate(form));

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Application_WithDefaultValue_IsValidCalled()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TÜRKİYE");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 19,
                    IdentityNumber = "1234"
                },

            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void Application_WithYoungAge_IsValidNeverCalled()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TÜRKİYE");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 15
                }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert

            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()),Times.Never);
        }

    }
}


