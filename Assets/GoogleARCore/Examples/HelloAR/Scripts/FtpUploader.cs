using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class FtpUploader : MonoBehaviour
{
    //public string FTPHost = "ftp://140.114.76.108";
    //public string FTPUserName = "team1";
    //public string FTPPassword = "y3Pkn";
    public string FTPHost = "ftp://ftp.dlptest.com/";
    public string FTPUserName = "dlpuser@dlptest.com";
    public string FTPPassword = "e73jzTRTNqCN9PYAAjjn";
    public string FilePath;

    public void UploadFile(byte[] fileContents)
    {
        FilePath = Application.persistentDataPath + "/Input.png";
        //Debug.Log("Path: " + FilePath);


        //WebClient client = new System.Net.WebClient();
        //Uri uri = new Uri(FTPHost + new FileInfo(FilePath).Name);


        //client.UploadProgressChanged += new UploadProgressChangedEventHandler(OnFileUploadProgressChanged);
        //client.UploadFileCompleted += new UploadFileCompletedEventHandler(OnFileUploadCompleted);
        //client.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
        //client.UploadFileAsync(uri, WebRequestMethods.Ftp.UploadFile, FilePath);
        // Get the object used to communicate with the server.
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPHost + new FileInfo(FilePath).Name);
        request.Method = WebRequestMethods.Ftp.UploadFile;

        // This example assumes the FTP site uses anonymous logon.
        request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);

        // Copy the contents of the file to the request stream.
        //byte[] fileContents;
        using (StreamReader sourceStream = new StreamReader(FilePath))
        {
            //fileContents = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());

        }

        request.ContentLength = fileContents.Length;

        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(fileContents, 0, fileContents.Length);
        }

        //using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        //{
        //    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
        //}
    }

    //void OnFileUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
    //{
    //    Debug.Log("Uploading Progress: " + e.ProgressPercentage);
    //}

    //void OnFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
    //{
    //    Debug.Log("File Uploaded");
    //}

    //void Start()
    //{
    //    UploadFile();
    //}
}