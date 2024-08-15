using JobApplicationLibrary.Models;
using Xunit;
using static JobApplicationLibrary.ApplicationEvaluator;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        private readonly ApplicationEvaluator _evaluator;

        public ApplicationEvaluateUnitTest()
        {
            _evaluator = new();
        }

        //UnitOfWork_Condition_ExpectedResult
        [Fact]
        public void Application_WhenAgeIsUnderMınAge_TransferredToAutoRejected()
        {
            //Arrange
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            //Act
            var result = _evaluator.Evaluate(form);

            //Assert

            Assert.Equal(result, ApplicationResult.AutoRejected);
        }

        [Fact]

        public void Application_WhenNoTechStack_TransferredToAutoRejected()
        {
            //arrange
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=19},
                TechStackList = new List<string> { "" }
            };

            //act
            var result = _evaluator.Evaluate(form);

            //assert
            Assert.Equal(result, ApplicationResult.AutoRejected);
        }

        [Fact]

        public void Application_WhenTechStackOver75Percent_TransferredToAutoAccepted()
        {
            //arrange
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=45},
                TechStackList = new List<string> { "C#", "RabbitMQ", "Microservice", "VisualStudio" },
                YearsOfExperience=16
            };

            //act
            var result = _evaluator.Evaluate(form);

            //assert
            Assert.Equal(result, ApplicationResult.AutoAccepted);
        }
    }
}
