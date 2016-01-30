using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class ScoreInfo
{
	public int score = 0;
	public string name;
}
public class ScoreData
{
	public List<ScoreInfo> scoreList = new List<ScoreInfo>();
}
	
public class ScoreList : MonoBehaviour 
{
	private static int listSize = 5;
	private static ScoreList instance = null;
	private static string encryptionKey = "24dg2f1d";

	public ScoreData data = new ScoreData();

	public static ScoreList Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject obj = new GameObject("ScoreList");
				obj.hideFlags = HideFlags.HideAndDontSave;
				instance = obj.AddComponent<ScoreList>();
			}
			return instance;
		}
	}

	public void AddScore(int score, string name)
	{
		ScoreInfo info = new ScoreInfo();
		info.score = score;
		info.name = name;
		data.scoreList.Add(info);

		if (data.scoreList.Count > listSize)
			data.scoreList.RemoveAt(data.scoreList.Count - 1);
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
		Load();
	}

	void Start () 
	{
	}

	void Update () 
	{	
	}

	public void Save ()
	{
		EncryptAndSerialize<ScoreData>(GetSavePath(), data, encryptionKey);
	}

	public void Load ()
	{
		if (!File.Exists(GetSavePath()))
			return;

		data = DecryptAndDeserialize<ScoreData>(GetSavePath(), encryptionKey);
	}

	private string GetSavePath()
	{
		return Application.persistentDataPath + "/highscore.dat";
	}
		
	public static void EncryptAndSerialize<T>(string filename, T obj, string encryptionKey)
	{
		var key = new DESCryptoServiceProvider();
		var e = key.CreateEncryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
		using (var fs = File.Open(filename, FileMode.Create))
		using (var cs = new CryptoStream(fs, e, CryptoStreamMode.Write))
			(new XmlSerializer(typeof(T))).Serialize(cs, obj);
	}

	public static T DecryptAndDeserialize<T>(string filename, string encryptionKey)
	{
		var key = new DESCryptoServiceProvider();
		var d = key.CreateDecryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
		using (var fs = File.Open(filename, FileMode.Open))
		using (var cs = new CryptoStream(fs, d, CryptoStreamMode.Read))
			return (T)(new XmlSerializer(typeof(T))).Deserialize(cs);
	}
}
