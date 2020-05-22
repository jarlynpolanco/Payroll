namespace Payroll.Shared.Models
{
    public class AppSettings
    {
        public string OutPutFilePath { get; set; }
        public string InputFilePath { get; set; }
        public string MySqlConnectionString { get; set; }
        public SftpSettings SftpSettings { get; set; }
    }
}
