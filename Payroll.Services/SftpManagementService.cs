using Microsoft.Extensions.Options;
using Payroll.Shared.Models;
using Renci.SshNet;
using System.IO;

namespace Payroll.Services
{
    public class SftpManagementService
    {
        private readonly SftpSettings _sftpSettings;
        private readonly IOptions<AppSettings> _appSettings;
        private ConnectionInfo _connectionInfo;

        public SftpManagementService(IOptions<AppSettings> appSettings) 
        {
            _sftpSettings = appSettings.Value.SftpSettings;
            _appSettings = appSettings;
        }

        public void SftpUploadFile(Stream fileStream, string fileName) 
        {
            var sftpConnection = GetSftpConnection();

            using (var sftp = new SftpClient(sftpConnection)) 
            {
                sftp.Connect();
                sftp.UploadFile(fileStream, fileName);
                sftp.ChangePermissions(fileName, 777);
                sftp.Disconnect();
            }
        }

        public string SftpDownloadFile(string fileName)
        {
            var sftpConnection = GetSftpConnection();
            var outPutFileName = Path.Combine(_appSettings.Value.OutPutFilePath, fileName);
            using (var sftp = new SftpClient(sftpConnection))
            {
                sftp.Connect();
                using (Stream fileStream = File.Create(outPutFileName))
                {
                    sftp.DownloadFile(fileName, fileStream);
                }
                sftp.Disconnect();
            }
            return outPutFileName;
        }

        private ConnectionInfo GetSftpConnection() 
        {
            if(_connectionInfo == null)
                _connectionInfo = new ConnectionInfo(_sftpSettings.SftpHost, _sftpSettings.SftpPort, _sftpSettings.SftpUserName,
                       new PasswordAuthenticationMethod(_sftpSettings.SftpUserName, _sftpSettings.SftpPassword));

            return _connectionInfo;
        }   
    }
}
