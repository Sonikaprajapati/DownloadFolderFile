using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp3.Logics.BLL
{
    public class FileDownloadBLL
    {
        public int CkycFileDownload()
        {
            DataTable dt = SelectMasterFI();
            string path = string.Empty;
            string filePath = ConfigurationManager.AppSettings["filePath"].ToString();
            string filePathfolder = ConfigurationManager.AppSettings["filePathfolder"].ToString();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    string ficode = row["FiCode"].ToString();
                    string Creator = row["Creator"].ToString();
                    string CkycRefNo = row["CkycRefNo"].ToString();
                    path = Path.Combine(filePath, CkycRefNo);
                    if (Directory.Exists(path))
                    {
                       
                        // Get the file name without extension
                        string[] entries = Directory.GetFileSystemEntries(path);
                        foreach (string entry in entries)
                        {
                            // Console.WriteLine(entry);
                            //string fileName = Path.GetFileNameWithoutExtension(entry.Trim());
                            //string fileExtension1 = Path.GetExtension(entry);
                            //// Split the file name into alphabetical characters and digits using regex
                            //MatchCollection matches = Regex.Matches(fileName, @"\d+|\D+");
                            //string result = "";

                            //// Concatenate the matches into one string
                            //foreach (Match match in matches)
                            //{
                            //    result += match.Value + " "; 
                            //}
                            //string newfilename = result + fileExtension1;
                            //string sanitizedFileName = SanitizeFileName(newfilename);
                            //string downloadFolder = @"D:\IFLOW_CKYC\IFLOW_CKYC\CKYC_Images\CustomerSourcePath\";
                            //string destinationPath = Path.Combine(downloadFolder, sanitizedFileName);
                            //File.Copy(entry.Trim(), destinationPath, true);
                            //Console.WriteLine(result.Trim()+ fileExtension1);

                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(entry);
                            string fileExtension = Path.GetExtension(entry);
                            // Split the file name into components
                            string[] fileNameComponents = fileNameWithoutExtension.Split('_');

                            // Ensure that we have at least three components (prefix, number, suffix)
                            if (fileNameComponents.Length == 3)
                            {
                                string prefix = fileNameComponents[0];
                                string number = fileNameComponents[1];
                                string suffix = fileNameComponents[2];

                                string concatenatedFileName = $"{prefix}_{number}_{suffix}{fileExtension}";

                                //string downloadFolder = @"D:\IFLOW_CKYC\IFLOW_CKYC\CKYC_Images\CustomerSourcePath\";
                                string downloadFolder = Path.Combine(filePathfolder, CkycRefNo);
                                string destinationPath = Path.Combine(downloadFolder, concatenatedFileName);

                                // Copy the file to the download folder
                                string destinationDirectory = Path.GetDirectoryName(destinationPath);
                                if (!Directory.Exists(destinationDirectory))
                                {
                                    Directory.CreateDirectory(destinationDirectory);
                                }

                                File.Copy(entry, destinationPath, true); // Overwrite if file already exists
                                Console.WriteLine($"File copied to: {destinationPath}");
                                
                            }
                            else if (fileNameComponents.Length == 4)
                            {
                                string prefix = fileNameComponents[0];
                                string number = fileNameComponents[1];
                                string suffix = fileNameComponents[2];
                                string suffixlast = fileNameComponents[3];
                                string newString = number.Replace("old", "");

                                string concatenatedFileName = $"{prefix}_{newString}_{suffix}_{suffixlast}{fileExtension}";

                                //string downloadFolder = @"D:\IFLOW_CKYC\IFLOW_CKYC\CKYC_Images\CustomerSourcePath\";
                                string downloadFolder = Path.Combine(filePathfolder, CkycRefNo);
                                string destinationPath = Path.Combine(downloadFolder, concatenatedFileName);

                                // Copy the file to the download folder
                                string destinationDirectory = Path.GetDirectoryName(destinationPath);
                                if (!Directory.Exists(destinationDirectory))
                                {
                                    Directory.CreateDirectory(destinationDirectory);
                                }

                                File.Copy(entry, destinationPath, true); // Overwrite if file already exists
                                Console.WriteLine($"File copied to: {destinationPath}");
                            }
                            else
                            {
                                Console.WriteLine("File Not found");
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Folder not found.");
                    }

                }

            }
            return 0;
        }
        private string SanitizeFileName(string fileName)
        {
            // Remove leading and trailing spaces
            fileName = fileName.Trim();

            // Replace invalid characters with underscore (_)
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            return fileName;
        }

        public DataTable SelectMasterFI()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["defaultconnection"].ToString()))
            {
                string query = @"select * from CkycReqLog  where RespJson  like '%IMRJ%' and CkycNo is null ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    da.Fill(dt);
                    con.Close();
                }
            }
            return dt;
        }
        
    }
}
