using JobApplicationLibrary.Models;
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
            var evaluator = new ApplicationEvaluator();
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            //Act
            var result= evaluator.Evaluate(form);

           //Assert

            Assert.Equal(result, ApplicationResult.AutoRejected);
        }
    }
}
