﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("AWSManager is NULL!");
            }
            return _instance;
        }
    }

    public string S3Region = RegionEndpoint.USEast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_s3Client == null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                "us-east-2:aa404cd0-16c4-4031-8dda-dcdd726a9b7a", // Identity Pool ID
                RegionEndpoint.USEast2 // Region
                ), _S3Region);
            }
            return _s3Client;
        }
    }

    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        // ResultText is a label used for displaying status information
        /*S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("Bucket name: " + s3b.BucketName);
                });
            }
            else
            {
                Debug.Log("AWS error");
            }
        });*/
    }

    public void UploadToS3(string filePath, string caseID)
    {
        FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "service-adjustment-app-jason",
            Key = "case#" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("Successfully posted to bucket.");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("Exception occured during uploading: " + responseObj.Exception);
            }
        });
    }

    public void GetList(string caseNumber, Action onComplete = null)
    {
        string target = "case#" + caseNumber;

        ListObjectsRequest request = new ListObjectsRequest()
        {
            BucketName = "service-adjustment-app-jason"
        };

        S3Client.ListObjectsAsync(request, (requestObj) =>
        {
            if (requestObj.Exception == null)
            {
                bool caseFound = requestObj.Response.S3Objects.Any(obj => obj.Key == target);

                if(caseFound == true)
                {
                    Debug.Log("Case found!");
                    S3Client.GetObjectAsync("service-adjustment-app-jason", target, (responseObj) =>
                    {
                        //read the data and apply to a case to be used

                        //check if response stream is null
                        if (responseObj.Response.ResponseStream != null)
                        {
                            //byte array to store the data from file
                            byte[] data = null;

                            //use stream reader to read response data
                            using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                            {
                                //access a memory stream
                                using (MemoryStream memory = new MemoryStream())
                                {
                                    //populate data byte array with memstream data
                                    var buffer = new byte[512];
                                    var bytesRead = default(int);

                                    while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        memory.Write(buffer, 0, bytesRead);
                                    }
                                    data = memory.ToArray();
                                }
                            }
                            //convert bytes to a case object
                            using (MemoryStream memory = new MemoryStream(data))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                Case downloadedCase = (Case)bf.Deserialize(memory);
                                Debug.Log("Downloaded case name: " + downloadedCase.name);
                                UIManager.Instance.activeCase = downloadedCase;
                                if (onComplete != null)
                                    onComplete();
                            }
                        }
                    });
                }
                else
                {
                    Debug.Log("Case not found");
                }
            }
            else
            {
                Debug.Log("Error getting list of items from S3: " + requestObj.Exception);
            }
        });
    }
}
