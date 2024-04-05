using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.UnitTest
{
	public class ApplicationEvalueateUnitTest
	{

		//isimlendirme: UnitOfWork_Condition_ExpectedResult
		//Ýsimlendirme:Condition_Result
		[Test]
		public void Application_WithUnderAge_TransferredAutoRejected()
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

			//Action
			var appResult=evaluator.Evaluate(form);

			//Assert
			Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
		}
		
	}
}