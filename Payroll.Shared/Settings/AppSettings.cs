namespace Payroll.Shared.Settings
{
    public class AppSettings
    {
        public string OutPutFilePath { get; set; }
        public string InputFilePath { get; set; }
        public string MySqlConnectionString { get; set; }
        public string PgpPrivateKey { get; set; }
        public string PgpPassShared { get; set; }
        public string PgpPublicKey { get; set; }
        public SftpSettings SftpSettings { get; set; }
    }
}
