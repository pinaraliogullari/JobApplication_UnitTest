using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        private const int autoAcceptedYearsOfExperiences = 15;
        private List<string> techStackList = new() { "C#", "RabbitMQ", "Microservice", "VisualStudio" };
        private readonly IIdentityValidator _identityValidator;

        public ApplicationEvaluator(IIdentityValidator identityValidator)
        {
            _identityValidator = identityValidator;
        }

        public ApplicationResult Evaluate(JobApplication form)
        {
            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            _identityValidator.ValidationMode = form.Applicant.Age > 50 ? ValidationMode.Detailed : ValidationMode.Quick;

            if (_identityValidator.CountryDataProvider.CountryData.Country != "TÜRKİYE")
                return ApplicationResult.TransferredToCTO;

            var validIdentity = _identityValidator.IsValid(form.Applicant.IdentityNumber);

            if (!validIdentity)
                return ApplicationResult.TransferredToHR;

            var sr = GetTechStackSimilarityRate(form.TechStackList);

            if (sr < 25)
                return ApplicationResult.AutoRejected;

            if (form.YearsOfExperience >= autoAcceptedYearsOfExperiences && sr > 75)
                return ApplicationResult.AutoAccepted;

            return ApplicationResult.AutoAccepted;
        }

        private int GetTechStackSimilarityRate(List<string> techStacks)
        {
            var matechedCount = techStacks
                .Where(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase))
                .Count();
            return (int)((double)(matechedCount) / techStackList.Count) * 100;
        }
        public enum ApplicationResult
        {
            AutoRejected,
            TransferredToHR,
            TransferredToLead,
            TransferredToCTO,
            AutoAccepted

        }
    }
}
