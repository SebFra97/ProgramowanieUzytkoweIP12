namespace Helpers
{
    public interface IMockDataHelper
    {
        public string RandomString(int length);
        public string GenerateSurname();
        public string GenerateName();

        public int GenerateRandomRate(int end);
    }
}
